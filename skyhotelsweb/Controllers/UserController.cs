using Newtonsoft.Json;
using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace skyhotelsweb.Controllers
{
    public class UserController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/user/";
        user _oUser = new user();

        // GET: User
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<user> UpdateReservation(int id, user reservation)
        {
            _oUser = new user();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL + id);
                request.Content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUser = JsonConvert.DeserializeObject<user>(apiResponse);
                }
            }
            return _oUser;
        }
    }
}