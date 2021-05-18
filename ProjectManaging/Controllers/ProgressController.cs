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
    public class ProgressController : Controller
    {
        IConnectDB DB;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            List<ProgressModel> pg = GetProgressModels();
            List<List<ProgressModel>> pgs = new List<List<ProgressModel>>();
            string[] job_id = pg.Select(s => s.job_id).Distinct().ToArray();
            for(int i = 0; i < job_id.Count(); i ++)
            {
                pgs.Add(pg.Where(w => w.job_id == job_id[i]).Select(s => s).OrderBy(y => y.job_year).ThenBy(m => m.month).ToList());
            }
            return Json(pgs);
        }

        public List<ProgressModel> GetProgressModels()
        {
            List<ProgressModel> pgs = new List<ProgressModel>();
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Progress.Job_ID, " +
                                    "job.Job_Number, " +
                                    "job.Job_Name, " +
                                    "job.Estimated_Budget, " +
                                    "Progress.Job_Progress, " +
                                    "Progress.Month, " +
                                    "job.Job_Year " +
                                    "from Progress left join job on job.job_ID = Progress.Job_ID";

            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ProgressModel pg = new ProgressModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        job_number = dr["Job_Number"] != DBNull.Value ? dr["Job_Number"].ToString() : "",
                        job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                        estimated_budget = dr["Estimated_Budget"] != DBNull.Value ? Convert.ToInt32(dr["Estimated_Budget"]) : 0,
                        job_progress = dr["Job_Progress"] != DBNull.Value ? Convert.ToInt32(dr["Job_Progress"]) : 0,
                        month = dr["Month"] != DBNull.Value ? Convert.ToInt32(dr["Month"]) : 0,
                        job_year = dr["Job_Year"] != DBNull.Value ? Convert.ToInt32(dr["Job_Year"]) : 0
                    };
                    pgs.Add(pg);
                }
                dr.Close();
            }
            con.Close();
            return pgs;
        }
    }
}
