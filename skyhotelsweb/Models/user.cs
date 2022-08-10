using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class user
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int phone_num { get; set; }
        public string status { get; set; }
    }
}