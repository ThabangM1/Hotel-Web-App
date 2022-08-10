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
    public class ReservationController : Controller
    {

        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/bookings/";
        reservation _oBooking = new reservation();
        List<reservation> _oBookings = new List<reservation>();
        List<dates> _oDates = new List<dates>();

        public ReservationController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "clerk") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }

            _oBookings = new List<reservation>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBookings = JsonConvert.DeserializeObject<List<reservation>>(apiResponse);
                }
            }

            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];

            return View(_oBookings);
        }

        [HttpGet]
        public async Task<JsonResult> fetchDates(int id)
        {
            TempData.Keep();
            _oDates = new List<dates>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "room/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiResponse);
                    _oDates = JsonConvert.DeserializeObject<List<dates>>(apiResponse);
                }
            }
            foreach(var d in _oDates)
            {
                Debug.WriteLine(d.start_date +"  "+ d.end_date);
            }
            return Json(_oDates,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> filterReservations(string id)
        {
            _oBookings = new List<reservation>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "filter/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBookings = JsonConvert.DeserializeObject<List<reservation>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oBookings, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(int id)
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "clerk") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (reservation res in _oBookings)
            {
                if (res.id == id)
                {
                    return View(res);
                }
            }
            reservation booking = new reservation();
            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booking = JsonConvert.DeserializeObject<List<reservation>>(apiResponse)[0];
                }
            }

            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];

            return View(booking);
        }

        [HttpPost]
        public async Task<reservation> UpdateReservation(int id, reservation reservation)
        {
            _oBooking = new reservation();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL+id);
                request.Content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oBooking = JsonConvert.DeserializeObject<reservation>(apiResponse);
                }
            }
            return _oBooking;
        }

        [HttpDelete]
        public async Task<string> DeleteRoom(int id)
        {
            string message = "";

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.DeleteAsync(BASE_URL + id))
                {
                    message = await response.Content.ReadAsStringAsync();
                }
            }
            return message;
        }

    }
}