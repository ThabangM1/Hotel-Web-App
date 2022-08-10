using Newtonsoft.Json;
using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace skyhotelsweb.Controllers
{
    public class HomeController : Controller
    {
        staff _oStaff = new staff();

        public ActionResult Index()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            TempData.Keep();
            return View();
        }

    }
}