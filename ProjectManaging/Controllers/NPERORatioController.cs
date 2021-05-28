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
    public class NPERORatioController : Controller
    {
        INormalOvertime NPO;

        public NPERORatioController()
        {
            this.NPO = new NormalOvertimeService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            return Json(NPO.NormalPerOvertime());
        }
    }
}
