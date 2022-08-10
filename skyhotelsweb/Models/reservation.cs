using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class reservation
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string room_num { get; set; }
        public int guest_id { get; set; }
        public int room_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string status { get; set; }
        public string code { get; set; }

    }
}