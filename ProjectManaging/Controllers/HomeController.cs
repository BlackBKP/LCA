using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Interfaces;
using ProjectManaging.Models;
using ProjectManaging.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class HomeController : Controller
    {
        IHome Home;

        public HomeController()
        {
            this.Home = new HomeService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSpentCostPerWeeks()
        {
            List<List<SpentPerWeekModel>> projects = new List<List<SpentPerWeekModel>>();
            List<SpentPerWeekModel> spws = Home.GetSpentCostPerWeeks();
            string[] job_id = spws.Select(s => s.job_id).Distinct().ToArray();
            for(int i = 0; i < job_id.Count(); i++)
            {
                projects.Add(spws.Where(w => w.job_id == job_id[i]).Select(s => s).OrderBy(o=>o.year).ThenBy(t=>t.month).ThenBy(tt=>tt.week).ToList());
            }
            return Json(projects);
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            return Json(Home.GetEmployees());
        }

        [HttpGet]
        public JsonResult GetHours()
        {
            return Json(Home.GetHours());
        }

        [HttpGet]
        public JsonResult GetJobs()
        {
            return Json(Home.GetJobs());
        }

        [HttpGet]
        public JsonResult GetLaborCosts()
        {
            return Json(Home.GetLaborCosts());
        }

        [HttpGet]
        public JsonResult GetOvertimes()
        {
            return Json(Home.GetOvertimes());
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
