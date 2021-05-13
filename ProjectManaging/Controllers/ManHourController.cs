using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Interfaces;
using ProjectManaging.Models;
using ProjectManaging.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class ManHourController : Controller
    {
        IConnectDB DB;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            List<MPHModel> mphs = GetMPHModels();
            return Json(mphs);
        }

        public List<MPHModel> GetMPHModels()
        {
            this.DB = new ConnectDB();
            List<MPHModel> mphs = new List<MPHModel>();
            SqlConnection con = DB.Connect();
            con.Open();
            string str_cmd = "select Hour.job_ID, " +
                                    "week, " +
                                    "Month, " +
                                    "SUM(Hours) as Normal, " +
                                    "SUM(s1.OT_1_5) as OT_1_5, " +
                                    "SUM(s1.OT_3) as OT_3, " +
                                    "SUM(SUM(Hour.Hours + s1.OT_1_5 + s1.OT_3))OVER(partition by Hour.job_ID ORDER BY Hour.job_ID, Month, week ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) as acc_hour " +
                                    "from Hour left join (select job_ID, OT_1_5, OT_3 from OT) as s1 ON s1.job_ID = Hour.job_ID group by Hour.job_ID,Month,week order by job_id, Month, week";
            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    MPHModel mph = new MPHModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        week = dr["Week"] != DBNull.Value ? Convert.ToInt32(dr["Week"]) : 0,
                        month = dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        normal = dr["Normal"] != DBNull.Value ? Convert.ToInt32(dr["Normal"]) : 0,
                        ot_1_5 = dr["OT_1_5"] != DBNull.Value ? Convert.ToInt32(dr["OT_1_5"]) : 0,
                        ot_3 = dr["OT_3"] != DBNull.Value ? Convert.ToInt32(dr["OT_3"]) : 0,
                        acc_hour = dr["acc_hour"] != DBNull.Value ? Convert.ToInt32(dr["acc_hour"]) : 0,
                    };
                    mphs.Add(mph);
                }
                dr.Close();
            }
            con.Close();
            return mphs;
        }
    }
}
