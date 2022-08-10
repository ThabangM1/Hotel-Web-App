using Newtonsoft.Json;
using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Web.Hosting;
using System.IO;
using System.Diagnostics;

namespace skyhotelsweb.Controllers
{
    public class InvoiceController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        const string BASE_URL = "http://localhost:4040/Invoice/";
        Invoice _oInvoice = new Invoice();
        Charge _ocharge = new Charge();
        List<Charge> _oCharges = new List<Charge>();

        public InvoiceController()
        {
            //Establish Http Client Handler
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
        }

        //Single Invoice by id
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            //verifying session data and access permission 
            if ((TempData["name"] == null) || (TempData["accesslevel"] == null))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!(((TempData["accesslevel"]).ToString() == "clerk") || ((TempData["accesslevel"]).ToString() == "manager")))
            {
                return RedirectToAction("Index", "Home");
            }

            _oInvoice = new Invoice();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oInvoice = JsonConvert.DeserializeObject<List<Invoice>>(apiResponse)[0];
                }
            }
            ViewBag.access = TempData["accesslevel"];
            ViewBag.name = TempData["name"];
            TempData["invoice"] = _oInvoice;
            TempData.Keep();

            return View(_oInvoice);
        }
        //Invoice plain html view
        public async Task<ActionResult> invoiceHTML(int id, int invid)
        {
            _oCharges = new List<Charge>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "charge/" + invid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oCharges = JsonConvert.DeserializeObject<List<Charge>>(apiResponse);
                }

                using (var response = await httpclient.GetAsync(BASE_URL + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oInvoice = JsonConvert.DeserializeObject<List<Invoice>>(apiResponse)[0];
                }

            }

            TempData.Keep();
            _oInvoice.charges = _oCharges;
            return View(_oInvoice);
        }
        //Invoice document pdf creation
        [HttpGet]
        public JsonResult savePDF(int id, int invid)
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings();

            //Set WebKit path
            settings.WebKitPath = Path.Combine(Environment.CurrentDirectory, "QtBinaries/");

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("https://localhost:44395/Invoice/invoiceHTML/" + id + "/" + invid);

            //Save and close the PDF document 
            document.Save("Output.pdf");

            document.Close(true);
            TempData.Keep();
            return Json(_oInvoice, JsonRequestBehavior.AllowGet);
            //return View(_oInvoice);
            // return new FileContentResult(document,"application/pdf");
        }
        //Invoice charges by invoice id
        [HttpGet]
        public async Task<JsonResult> getCharges(int id)
        {
            _oCharges = new List<Charge>();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                using (var response = await httpclient.GetAsync(BASE_URL + "charge/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oCharges = JsonConvert.DeserializeObject<List<Charge>>(apiResponse);
                }
            }
            TempData.Keep();
            return Json(_oCharges, JsonRequestBehavior.AllowGet);
        }
        //Invoice Creation
        [HttpPost]
        public async Task<Invoice> createInvoice(int id, Invoice newInvoice)
        {
            _oInvoice = new Invoice();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newInvoice), Encoding.UTF8, "application/json");
                using (var response = await httpclient.PostAsync(BASE_URL + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oInvoice = JsonConvert.DeserializeObject<Invoice>(apiResponse);
                }
            }
            TempData.Keep();
            return _oInvoice;
        }
        //Adding fees to invoice
        [HttpPost]
        public async Task<Charge> addCharge(int id, Charge newcharge)
        {
            _ocharge = new Charge();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newcharge), Encoding.UTF8, "application/json");
                using (var response = await httpclient.PostAsync(BASE_URL + "charge/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _ocharge = JsonConvert.DeserializeObject<Charge>(apiResponse);
                }
            }
            TempData.Keep();
            return _ocharge;
        }
        //Update invoice details
        [HttpPost]
        public async Task<Invoice> updateInvoice(int id,Invoice newInvoice)
        {
            _oInvoice = new Invoice();

            using (var httpclient = new HttpClient(_clientHandler))
            {
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), BASE_URL + id);
                request.Content = new StringContent(JsonConvert.SerializeObject(newInvoice), Encoding.UTF8, "application/json");
                using (var response = await httpclient.SendAsync(request))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oInvoice = JsonConvert.DeserializeObject<Invoice>(apiResponse);
                }
            }
            TempData.Keep();
            return _oInvoice;
        }
    }
}