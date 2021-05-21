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
            List<List<MPHModel>> lmphs = GetMPHModels();
            return Json(lmphs);
        }

        public List<List<MPHModel>> GetMPHModels()
        {
            this.DB = new ConnectDB();
            List<MPHModel> mphs = new List<MPHModel>();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Hour.Job_ID, " +
                                    "Hour.Week, " +
                                    "Hour.Month, " +
                                    "FORMAT(Hour.Working_Day,'yyyy') as Year, " +
                                    "(case when sum(Hour.Hours) is null then 0 else sum(Hour.Hours) end) as Normal, " +
                                    "(case when sum(s1.OT_1_5) is null then 0 else sum(s1.OT_1_5) end) as OT_1_5, " +
                                    "(case when sum(s1.OT_3) is null then 0 else sum(s1.OT_3) end) as OT_3, " +
                                    "sum(case when sum(Hour.Hours) is null then 0 else sum(Hour.Hours) end + case when sum(s1.OT_1_5) is null then 0 else sum(s1.OT_1_5) end  + case when sum(s1.OT_3)is null then 0 else sum(s1.OT_3) end ) OVER (partition by Hour.job_ID ORDER BY Hour.job_ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) as Acc_Hour " +
                                    "from Hour " +
                                    "left join (select job_ID,Employee_ID,Recording_time,Month,week,OT_1_5,OT_3 from OT) as s1 ON s1.job_ID = Hour.job_ID and s1.Employee_ID = Hour.Employee_ID and s1.Recording_time = Hour.Working_Day and s1.Month = Hour.Month and s1.week = Hour.week group by Hour.job_ID,FORMAT(Hour.Working_Day,'yyyy'),Hour.Month,Hour.Week";

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
                        year = dr["Year"] != DBNull.Value ? Convert.ToInt32(dr["Year"]) : 0,
                        normal = dr["Normal"] != DBNull.Value ? Convert.ToInt32(dr["Normal"]) : 0,
                        ot_1_5 = dr["OT_1_5"] != DBNull.Value ? Convert.ToInt32(dr["OT_1_5"]) : 0,
                        ot_3 = dr["OT_3"] != DBNull.Value ? Convert.ToInt32(dr["OT_3"]) : 0,
                        acc_hour = dr["Acc_Hour"] != DBNull.Value ? Convert.ToInt32(dr["Acc_Hour"]) : 0,
                    };
                    mphs.Add(mph);
                }
                dr.Close();
            }
            con.Close();

            List<List<MPHModel>> lmphs = new List<List<MPHModel>>();
            string[] job_id = mphs.Select(s => s.job_id).Distinct().ToArray();
            for(int i = 0; i < job_id.Count(); i++)
            {
                lmphs.Add(mphs.Where(w => w.job_id == job_id[i]).Select(s => s).OrderBy(y => y.year).ThenBy(m => m.month).ThenBy(w => w.week).ToList());
            }
            return lmphs;
        }
    }
}
