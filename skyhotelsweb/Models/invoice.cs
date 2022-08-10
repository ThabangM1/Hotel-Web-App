using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class Invoice
    {
        public int id { get; set; }
        public int guest_id { get; set; }
        public int res_id { get; set; }
        public string date_created { get; set; }
        public string date_issued { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public List<Charge> charges { get; set; }
    }
}