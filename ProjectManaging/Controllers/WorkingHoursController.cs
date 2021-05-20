using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Interfaces;
using ProjectManaging.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class WorkingHoursController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        /*static List<JobSpentModel> jobs;*/
        IConnectDB DB;

        public WorkingHoursController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<string> GetJobID()
        {
            List<string> job_ids = new List<string>();
            this.DB = new ConnectDB();
            SqlConnection con = DB.Connect();
            con.Open();
            string str_cmd = "select Job_ID from Job";
            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string id = dr["Job_ID"].ToString().Trim();
                    job_ids.Add(id);
                }
                dr.Close();
            }
            con.Close();
            return job_ids;
        }

        [HttpGet]
        public JsonResult GetJobNumbers()
        {
            return Json(GetJobID());
        }
    }
}
