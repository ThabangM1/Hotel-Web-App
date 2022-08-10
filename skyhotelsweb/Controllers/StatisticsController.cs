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
    public class StatisticsController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/stats/";
        List<stat> _oStats = new List<stat>();
        List<season> _oSeasonal = new List<season>();

        public StatisticsController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
        }

        public ActionResult Summary()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!((TempData["accesslevel"].ToString() == "owner") || (TempData["accesslevel"].ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            return View();
        }

        public ActionResult Rooms()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!((TempData["accesslevel"].ToString() == "owner") || (TempData["accesslevel"].ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> statsOverview()
        {
            _oStats = new List<stat>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "overview/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oStats = JsonConvert.DeserializeObject<List<stat>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oStats, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> popularity()
        {
            _oSeasonal = new List<season>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "popularity/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oSeasonal = JsonConvert.DeserializeObject<List<season>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oSeasonal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> cancellations()
        {
            _oStats = new List<stat>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "cancelled/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oStats = JsonConvert.DeserializeObject<List<stat>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oStats, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> roomOverview()
        {
            _oStats = new List<stat>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "rooms/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oStats = JsonConvert.DeserializeObject<List<stat>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oStats, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> roomPerformance(int id)
        {
            _oStats = new List<stat>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oStats = JsonConvert.DeserializeObject<List<stat>>(apiResponse);
                }
            }
            TempData.Keep();
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            return Json(_oStats, JsonRequestBehavior.AllowGet);
        }
    }
}