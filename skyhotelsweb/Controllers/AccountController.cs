using Newtonsoft.Json;
using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace skyhotelsweb.Controllers
{
    public class AccountController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/staff/";
        staff _staff = new staff();
        List<staff> _staffresults = new List<staff>();


        public AccountController()
        {
            TempData = new TempDataDictionary();
            TempData["name"] = "";
            TempData["accesslevel"] = "";
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<staff> UpdateAccount(int id, staff updatedstaff)
        {
            _staff = new staff();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(updatedstaff), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL + id);
                request.Content = new StringContent(JsonConvert.SerializeObject(updatedstaff), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _staff = JsonConvert.DeserializeObject<staff>(apiResponse);
                }
            }
            return _staff;
        }


        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "owner") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }
            _staff = new staff();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _staff = JsonConvert.DeserializeObject<List<staff>>(apiResponse)[0];
                }
                ViewBag.access = TempData["accesslevel"];
                ViewBag.name = TempData["name"];
                TempData.Keep();
                return View(_staff);
            }
        }

        public ActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<staff> Register(staff newstaff)
        {
            _staff = new staff();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newstaff), Encoding.UTF8, "application/json");
                using (var response = await httpclient.PostAsync(BASE_URL, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return _staff;
        }

        [HttpGet]
        public async Task<JsonResult> Verify(staff staff)
        {
            _staffresults = new List<staff>();
            _staff = new staff();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "login/" + staff.email))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _staffresults = JsonConvert.DeserializeObject<List<staff>>(apiResponse);
                }
                if (_staffresults.Count == 0)
                {
                    return Json(_staffresults, JsonRequestBehavior.AllowGet);
                }
                _staff = _staffresults[0];
            }
            if (_staff.password == staff.password)
            {
                Debug.WriteLine("**********************USER LOGGED IN****************************");
                TempData["name"] = (_staff.fname).ToString();
                TempData["accesslevel"] = (_staff.access).ToString(); ;
                TempData.Keep();
            }
            else
            {
                Debug.WriteLine("**********************PASSWORD MISMATCH****************************");
            }
            return Json(_staffresults, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "owner") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            TempData.Keep();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Management()    
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "owner") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }

            _staffresults = new List<staff>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _staffresults = JsonConvert.DeserializeObject<List<staff>>(apiResponse);
                }
            }

            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            TempData.Keep();
            return View(_staffresults);
        }
    }
}