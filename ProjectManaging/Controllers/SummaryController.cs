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
    public class SummaryController : Controller
    {
        IConnectDB DB;

        public IActionResult Index()
        {
            return View();
        }

        public List<JobSummaryModel> Query()
        {
            this.DB = new ConnectDB();
            List<JobSummaryModel> jobs = new List<JobSummaryModel>();
            SqlConnection con = DB.Connect();
            con.Open();

            string str_cmd = "select job.Job_ID, " +
                                    "job.Estimated_Budget, " +
                                    "s1.Labor_Cost, " +
                                    "s1.OT_Labor_Cost, " +
                                    "s1.Accommodation_Cost, " +
                                    "s1.Compensation_Cost, " +
                                    "s1.Cost_to_Date, " +
                                    "(cast(job.Estimated_Budget as int) - cast(s1.Cost_to_Date as int)) as Remaining_Cost, " +
                                    "((cast(s1.Cost_to_Date as float) / cast(job.Estimated_Budget as float)) *100) as Cost_Usage, " +
                                    "job.Work_Completion, " +
                                    "s2.Hours, " +
                                    "s3.OT_1_5, " +
                                    "s3.OT_3, " +
                                    "(s2.Hours + s3.OT_1_5 + s3.OT_3) as Total_Man_Hour, " +
                                    "job.No_Of_Labor, " +
                                    "(cast(s1.Cost_to_Date as float) / (s2.Hours + s3.OT_1_5 + s3.OT_3)) as avg_labor_cost_per_hour " +
                                    "from job left join (select job_ID, SUM(cast(Labor_Cost as int))as Labor_Cost, SUM(cast(OT_Labor_Cost as int)) as OT_Labor_Cost, " +
                                    "SUM(cast(Accommodation_Cost as int)) as Accommodation_Cost, SUM(cast(Compensation_Cost as int)) as Compensation_Cost, " +
                                    "(SUM(cast(Labor_Cost as int))  + SUM(cast(OT_Labor_Cost as int)) + SUM(cast(Accommodation_Cost as int)) + SUM(cast(Compensation_Cost as int))) as Cost_to_Date " +
                                    "from Labor_Costs group by job_ID) as s1 ON s1.job_ID = job.job_ID left join (select job_ID,SUM(Hours) as Hours " +
                                    "from Hour group by job_ID) as s2 ON s2.job_ID = job.job_ID left join (select job_ID,SUM(OT_1_5) as OT_1_5 , " +
                                    "SUM(OT_3) as OT_3 from OT group by job_ID) as s3 ON s3.job_ID = job.job_ID";
            SqlCommand cmd = new SqlCommand(str_cmd, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    JobSummaryModel job = new JobSummaryModel()
                    {
                        job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                        estimated_budget = dr["Estimated_Budget"] != DBNull.Value ? Convert.ToInt32(dr["Estimated_Budget"]) : 0,
                        labor_cost = dr["Labor_Cost"] != DBNull.Value ? Convert.ToInt32(dr["Labor_Cost"]) : 0,
                        ot_labor_cost = dr["OT_Labor_Cost"] != DBNull.Value ? Convert.ToInt32(dr["OT_Labor_Cost"]) : 0,
                        accomodation_cost = dr["Accommodation_Cost"] != DBNull.Value ? Convert.ToInt32(dr["Accommodation_Cost"]) : 0,
                        compensation_cost = dr["Compensation_Cost"] != DBNull.Value ? Convert.ToInt32(dr["Compensation_Cost"]) : 0,
                        cost_to_date = dr["Cost_to_Date"] != DBNull.Value ? Convert.ToInt32(dr["Cost_to_Date"]) : 0,
                        //remainning_cost = dr[]
                        cost_usage = dr["Cost_Usage"] != DBNull.Value ? Convert.ToInt32(dr["Cost_Usage"]) : 0,
                    };
                    jobs.Add(job);
                }
                dr.Close();
            }
            con.Close();




            return jobs;
        }

        [HttpGet]
        public JsonResult GetJobsSummary()
        {
            List<JobSummaryModel> jobs = new List<JobSummaryModel>();

            /*
            //Job 1
            JobSummaryModel job = new JobSummaryModel();
            job.job_id = "J21-0001";
            job.estimate_budget = 10000000;
            job.labor_cost = 3000000;
            job.labor_cost_ot = 2000000;
            job.accomodation_cost = 1000000;
            job.compensation_cost = 1000000;
            job.cost_to_date = job.labor_cost + job.labor_cost_ot + job.accomodation_cost + job.compensation_cost;
            job.remainning_cost = job.estimate_budget - job.cost_to_date;
            job.percent_cost_usage = (job.cost_to_date / job.estimate_budget) * 100;
            job.percent_work_completion = 80;
            job.normal_manhour = 10000;
            job.x15_manhour = 6000;
            job.x30_manhour = 3000;
            job.total_manhour = job.normal_manhour + job.x15_manhour + job.x30_manhour;
            job.number_of_labor = 100;
            job.avg_labor_cost_per_hour = (job.cost_to_date / job.total_manhour) / job.number_of_labor;
            jobs.Add(job);

            //Job 2
            job = new JobSummaryModel();
            job.job_id = "J21-0002";
            job.estimate_budget = 500000;
            job.labor_cost = 120000;
            job.labor_cost_ot = 0;
            job.accomodation_cost = 0;
            job.compensation_cost = 0;
            job.cost_to_date = job.labor_cost + job.labor_cost_ot + job.accomodation_cost + job.compensation_cost;
            job.remainning_cost = job.estimate_budget - job.cost_to_date;
            job.percent_cost_usage = (job.cost_to_date / job.estimate_budget) * 100;
            job.percent_work_completion = 80;
            job.normal_manhour = 4800;
            job.x15_manhour = 0;
            job.x30_manhour = 0;
            job.total_manhour = job.normal_manhour + job.x15_manhour + job.x30_manhour;
            job.number_of_labor = 3;
            job.avg_labor_cost_per_hour = (job.cost_to_date / job.total_manhour) / job.number_of_labor;
            jobs.Add(job);*/

            return Json(jobs);
        }
    }
}
