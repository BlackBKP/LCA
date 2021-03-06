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
        ISpentPerWeek SPW;

        public HomeController()
        {
            this.SPW = new SpentPerWeekService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSpentCostPerWeeks()
        {
            List<List<SpentPerWeekModel>> projects = new List<List<SpentPerWeekModel>>();
            List<SpentPerWeekModel> spws = SPW.GetSpentCostPerWeeks();
            string[] job_id = spws.OrderByDescending(o => o.job_id).Select(s => s.job_id).Distinct().ToArray();
            for(int i = 0; i < job_id.Count(); i++)
            {
                projects.Add(spws.Where(w => w.job_id == job_id[i]).Select(s => s).OrderBy(o=>o.year).ThenBy(t=>t.month).ThenBy(tt=>tt.week).ToList());
            }
            return Json(projects);
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
