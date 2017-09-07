using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LeX.Models;
using Microsoft.AspNetCore.Authorization;

namespace LeX.Controllers
{
    public class HomeController : Controller
    {
        Data.Common.DB.IDBHelper dbh = null;
        public HomeController(Data.Common.DB.IDBHelper dbh)
        {
            this.dbh = dbh;
        }

        [HttpGet("index2")]
        public IActionResult Index()
        {
            string name = dbh.ExecuteScalar<string>("select name from users where id=1");

            return new ContentResult { Content = name };
            //return View();
        }

        [Authorize(Roles = "admin,system")]
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
