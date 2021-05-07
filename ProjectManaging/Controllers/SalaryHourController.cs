using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class SalaryHourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            return Json("GGEZ");
        }

        public List<SPHModel> GetSPHModels()
        {
            List<SPHModel> sphs = new List<SPHModel>();
            SPHModel sph;

            //Job 1
            sph = new SPHModel()
            {
                job_id = "J21-0001",
                weeks = new string[] { "JAN 1", "JAN 2", "FEB 1", "FEB 2", "MAR 1", "MAR 2" },
                salaries = new int[] { 44, 40, 47, 43, 40 , 40 }
            };
            sphs.Add(sph);

            //Job 2
            sph = new SPHModel()
            {
                job_id = "J21-0002",
                weeks = new string[] { "JAN 1", "JAN 2", "FEB 1" },
                salaries = new int[] { 40, 42, 41 }
            };
            sphs.Add(sph);

            return sphs;
        }
    }
}
