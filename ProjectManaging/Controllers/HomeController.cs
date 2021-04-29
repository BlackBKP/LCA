﻿using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobs()
        {
            List<JobModel> jobs = new List<JobModel>();

            //Job 01
            JobModel job = new JobModel()
            {
                job_id = "J21-0001",
                pm = "Mister A",
                budget = 10,
                fortnight = new string[] { "JAN 1", "JAN 2", "FEB 1", "FEB 2", "MAR 1", "MAR 2" },
                progress = new double[] { 15, 30, 50, 60, 70, 85 },
                spent = new double[] { 1, 2, 4, 5, 7, 8 },
            };
            jobs.Add(job);

            //Job 02
            job = new JobModel()
            {
                job_id = "J21-0002",
                pm = "Mister B",
                budget = 5,
                fortnight = new string[] { "JAN 1", "JAN 2", "FEB 1", "FEB 2", "MAR 1", "MAR 2" },
                progress = new double[] { 15, 30, 40, 60, 80, 100 },
                spent = new double[] { 0.5, 1, 2, 2.5, 3, 3.5 },
            };
            jobs.Add(job);

            return Json(jobs);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
