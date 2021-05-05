using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class SummaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobsSummary()
        {
            List<JobSummaryModel> jobs = new List<JobSummaryModel>();

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
            jobs.Add(job);

            return Json(jobs);
        }
    }
}
