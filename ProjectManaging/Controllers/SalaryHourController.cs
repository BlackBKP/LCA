using Microsoft.AspNetCore.Mvc;
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
    }
}
