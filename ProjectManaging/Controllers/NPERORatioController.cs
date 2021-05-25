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
    public class NPERORatioController : Controller
    {
        IConnectDB DB;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            return Json(Query());
        }

        public List<NORModel> Query()
        {
            this.DB = new ConnectDB();
            List<NORModel> nprs = new List<NORModel>();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Hour.Job_ID, " +
                                    "SUM(Hours)as Normal, " +
                                    "s1.OT " +
                                    "from Hour " +
                                    "left join (select job_ID,(SUM(isnull(OT_1_5,0)) + SUM(isnull(OT_3,0))) as OT from OT group by Job_ID) as s1 ON s1.job_ID = Hour.job_ID group by Hour.Job_ID,s1.OT order by Job_ID";

            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    NORModel npr = new NORModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        normal = dr["Normal"] != DBNull.Value ? Convert.ToInt32(dr["Normal"]) : 0,
                        overtime = dr["OT"] != DBNull.Value ? Convert.ToInt32(dr["OT"]) : 0,
                    };
                    nprs.Add(npr);
                }
                dr.Close();
            }
            con.Close();
            nprs = nprs.OrderByDescending(o => o.job_id).ToList();
            return nprs;
        }

        public List<NORModel> GetNORModels()
        {
            List<NORModel> nors = new List<NORModel>();

            //Job 1
            NORModel nor = new NORModel()
            {
                job_id = "J21-0001",
                normal = 200,
                overtime = 40,
            };
            nors.Add(nor);

            //Job 2
            nor = new NORModel()
            {
                job_id = "J21-0002",
                normal = 100,
                overtime = 20,
            };
            nors.Add(nor);

            //Job 3
            nor = new NORModel()
            {
                job_id = "J21-0003",
                normal = 150,
                overtime = 15,
            };
            nors.Add(nor);
            return nors;
        }

        [HttpGet]
        public JsonResult GetNPOR()
        {
            List<NORModel> nors = GetNORModels();
            return Json(nors);
        }
    }
}
