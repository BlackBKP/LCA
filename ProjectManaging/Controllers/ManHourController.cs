using Microsoft.AspNetCore.Mvc;
using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Controllers
{
    public class ManHourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            List<MPHModel> mphs = GetMPHModels();
            return Json(mphs);
        }

        public List<MPHModel> GetMPHModels()
        {
            List<MPHModel> mphs = new List<MPHModel>();
            string[] weeks = new string[] { "JAN 1", "JAN 2", "FEB 1", "FEB 2", "MAR 1", "MAR 2", "APR 1", "APR 2", "MAY 1", "MAY 2", "JUN 1", "JUN 2", 
                                            "JUL 1", "JUL 2", "AUG 1", "AUG 2", "SEP 1", "SEP 2", "OCT 1", "OCT 2", "NOV 1", "NOV 2", "DEC 1", "DEC 2" };

            //Job-01
            MPHModel mph = new MPHModel();
            mph.job_id = "J21-0001";
            mph.week = new string[] { "JAN 1", "JAN 2", "FEB 1", "FEB 2", "MAR 1", "MAR 2" };
            mph.normal = new int[] { 200, 200, 192, 200, 192, 166, 100};
            mph.overtime = new int[] { 50, 46, 44, 46, 20, 10};
            mphs.Add(mph);

            //Job-02
            mph = new MPHModel();
            mph.job_id = "J21-0002";
            mph.week = new string[] { "JAN 1", "JAN 2", "FEB 1" };
            mph.normal = new int[] { 196, 200, 192 };
            mph.overtime = new int[] { 20, 12, 16 };
            mphs.Add(mph);

            return mphs;
        }
    }
}
