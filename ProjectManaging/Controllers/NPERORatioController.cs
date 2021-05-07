using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class NPERORatioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<NORModel> GetNORModels()
        {
            List<NORModel> nors = new List<NORModel>();

            //Job 1
            NORModel nor = new NORModel()
            {
                job_id = "J21-0001",
                normal = 200,
                overtime = 40,
            };
            nors.Add(nor);

            //Job 2
            nor = new NORModel()
            {
                job_id = "J21-0002",
                normal = 100,
                overtime = 20,
            };
            nors.Add(nor);

            //Job 3
            nor = new NORModel()
            {
                job_id = "J21-0003",
                normal = 150,
                overtime = 15,
            };
            nors.Add(nor);
            return nors;
        }

        [HttpGet]
        public JsonResult GetNPOR()
        {
            List<NORModel> nors = GetNORModels();
            return Json(nors);
        }
    }
}
