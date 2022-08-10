using skyhotelsweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace skyhotelsweb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "UpdateReservationRoute",
                "Reservation/UpdateReservation/{id}/{data}",
                new { controller = "Reservation", action = "UpdateReservation" }
                );
            routes.MapRoute(
                "UpdateInvoice",
                "Invoice/updateInvoice/{id}/{data}",
                new { controller = "Invoice", action = "updateInvoice" }
                );
            routes.MapRoute(
                "CreateInvoiceRoute",
                "Invoice/CreateInvoice/{id}/{data}",
                new { controller = "Invoice", action = "CreateInvoice" }
                );
            routes.MapRoute(
                "UpdateRoomRoute",
                "Room/UpdateRoom/{id}/{data}",
                new {controller="Room",action = "UpdateRoom"}
                );
            routes.MapRoute(
                "UpdateAccountRoute",
                "Account/UpdateAccount/{id}/{data}",
                new { controller = "Account", action = "UpdateAccount" }
                );
            routes.MapRoute(
                "invoiceHTMLRoomRoute",
                "Invoice/invoiceHTML/{id}/{invid}",
                new { controller = "Invoice", action = "invoiceHTML" }
                );
            routes.MapRoute(
                "savePDFRoomRoute",
                "Invoice/savePDF/{id}/{invid}",
                new { controller = "Invoice", action = "savePDF" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
