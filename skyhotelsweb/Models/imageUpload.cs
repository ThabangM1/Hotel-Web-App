using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class imageUpload
    {
        public int r_id { get; set; }
        public HttpPostedFileWrapper roomImage { get; set; }
    }
}