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
        IJobSummary JobSummary;

        public SummaryController()
        {
            this.JobSummary = new JobSummaryService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobsSummary()
        {
            List<JobSummaryModel> jobs = JobSummary.GetJobsSummary();
            return Json(jobs);
        }
    }
}
