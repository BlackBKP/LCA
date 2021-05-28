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
    public class ManHourController : Controller
    {
        IManpower MPH;

        public ManHourController()
        {
            this.MPH = new ManpowerService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            List<List<MPHModel>> lmphs = MPH.GetMPHModels();
            return Json(lmphs);
        }
    }
}
