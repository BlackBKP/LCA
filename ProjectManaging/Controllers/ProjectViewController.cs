using Microsoft.AspNetCore.Hosting;
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
    public class ProjectViewController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        IConnectDB DB;

        public ProjectViewController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobs()
        {
            List<JobModel> jobs = new List<JobModel>();
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Job_ID," +
                                    "Job_Number, " +
                                    "Job_Name, " +
                                    "Estimated_Budget, " +
                                    "Job_Year " +
                                    "from Job";

            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    JobModel job = new JobModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        job_number = dr["Job_Number"] != DBNull.Value ? dr["Job_Number"].ToString() : "",
                        job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                        estimated_budget = dr["Estimated_Budget"] != DBNull.Value ? Convert.ToInt32(dr["Estimated_Budget"]) : 0,
                        job_year = dr["Job_Year"] != DBNull.Value ? Convert.ToInt32(dr["Job_Year"]) : 0,
                    };
                    jobs.Add(job);
                }
                dr.Close();
            }
            return Json(jobs);
        }

        [HttpGet]
        public JsonResult GetJobsByYear(string year)
        {
            List<JobModel> jobs = new List<JobModel>();
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select Job_ID," +
                                    "Job_Number, " +
                                    "Job_Name, " +
                                    "Estimated_Budget, " +
                                    "Job_Year " +
                                    "from Job";

            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    JobModel job = new JobModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        job_number = dr["Job_Number"] != DBNull.Value ? dr["Job_Number"].ToString() : "",
                        job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                        estimated_budget = dr["Estimated_Budget"] != DBNull.Value ? Convert.ToInt32(dr["Estimated_Budget"]) : 0,
                        job_year = dr["Job_Year"] != DBNull.Value ? Convert.ToInt32(dr["Job_Year"]) : 0,
                    };
                    jobs.Add(job);
                }
                dr.Close();
            }
            jobs = jobs.Where(w => w.job_id.Substring(1, 2) == year.Substring(2, 2)).Select(s => s).ToList();
            
            return Json(jobs);
        }
    }
}
