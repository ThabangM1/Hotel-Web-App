using Newtonsoft.Json;
using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace skyhotelsweb.Controllers
{
    public class RoomController : Controller
     {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/rooms/";
        Room _oRoom = new Room();
        List<Room> _oRooms = new List<Room>();
        imageUpload _oUpload = new imageUpload();
        List<Amenity> _oAmenities = new List<Amenity>();
        List<media> _oList = new List<media>();

        public RoomController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender,cert,chain,SslPolicyErrors) => { return true; };
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (TempData["accesslevel"].ToString() != "owner")
            {
                return RedirectToAction("Index", "Home");
            }
            _oRooms = new List<Room>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL+"/filter/a"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oRooms = JsonConvert.DeserializeObject<List<Room>>(apiResponse);
                }
            }
            TempData.Keep();
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];

            return View(_oRooms);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (TempData["accesslevel"].ToString() != "owner")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            TempData.Keep();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> deleted(string id)
        {
            List<Room> _oList = new List<Room>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL+"/filter/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oList = JsonConvert.DeserializeObject<List<Room>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> Media(int id)
        {
            _oList = new List<media>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "media/"+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oList = JsonConvert.DeserializeObject<List<media>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oList,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if ((TempData["accesslevel"]).ToString() != "owner")
            {
                return RedirectToAction("Index", "Home");
            }
            _oRoom = new Room();
            var list = new List<media>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oRoom = JsonConvert.DeserializeObject<List<Room>>(apiResponse)[0];
                }
            }

            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];

            return View(_oRoom);
        }

        [HttpGet]
        public async Task<JsonResult> getAmenities(int id)
        {
            _oAmenities = new List<Amenity>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "amenities/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oAmenities = JsonConvert.DeserializeObject<List<Amenity>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oAmenities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(Room newRoom)
        {
            _oRoom = new Room();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newRoom), Encoding.UTF8,"application/json");
                using (var response = await httpclient.PostAsync(BASE_URL, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oRoom = JsonConvert.DeserializeObject<Room>(apiResponse);
                }
            }
            TempData.Keep();
            return new RedirectResult("/Room/Index");
        }

        [HttpPost]
        public async Task<JsonResult> uploadRoomImage(imageUpload newImage)
        {
            _oUpload = new imageUpload();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                BinaryReader b = new BinaryReader(newImage.roomImage.InputStream);
                byte[] image = b.ReadBytes(newImage.roomImage.ContentLength);

                MultipartFormDataContent content = new MultipartFormDataContent();
                var idcontent = new StringContent(newImage.r_id.ToString());
                idcontent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { 
                    Name = "r_id"
                };
                content.Add(idcontent);
                var imgContent = new StreamContent(new MemoryStream(image));
                imgContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "roomImage",
                    FileName = newImage.roomImage.FileName
                    
                };
                imgContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                content.Add(imgContent);

                using (var response = await httpclient.PostAsync(BASE_URL+"upload", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUpload = JsonConvert.DeserializeObject<imageUpload>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oUpload,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<Room> UpdateRoom(int id,Room Room)
        {
            _oRoom = new Room();
            
            using (var httpclient = new HttpClient(_clientHandler))
            {
                //StringContent content = new StringContent(JsonConvert.SerializeObject(Room), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL +id);
                request.Content = new StringContent(JsonConvert.SerializeObject(Room), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oRoom = JsonConvert.DeserializeObject<Room>(apiResponse);
                }
            }
            TempData.Keep();
            return _oRoom;
        }

        [HttpDelete]
        public async Task<string> DeleteRoom(int id)
        {
            string message = "";

            using (var httpclient = new HttpClient(_clientHandler) )
            {
                using (var response = await httpclient.DeleteAsync(BASE_URL+id))
                {
                    message = await response.Content.ReadAsStringAsync();
                }
            }
            TempData.Keep();
            return message;
        }
        
                
        [HttpPost]
        public async Task<JsonResult> addAmenities(int id, Amenity amenity)
        {
            _oAmenities = new List<Amenity>();
            Debug.WriteLine(amenity);
            using (var httpclient = new HttpClient(_clientHandler))
            {
                //StringContent content = new StringContent(JsonConvert.SerializeObject(amenities), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL +"amenities/add/"+ id);
                request.Content = new StringContent(JsonConvert.SerializeObject(amenity), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //_oAmenities = JsonConvert.DeserializeObject<List<Amenity>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> removeAmenities(int id, Amenity amenity)
        {
            _oAmenities = new List<Amenity>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                //StringContent content = new StringContent(JsonConvert.SerializeObject(amenities), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), BASE_URL + "amenities/remove/" + id);
                request.Content = new StringContent(JsonConvert.SerializeObject(amenity), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //_oAmenities = JsonConvert.DeserializeObject<List<Amenity>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json("success", JsonRequestBehavior.AllowGet);
        }

    }

    
}