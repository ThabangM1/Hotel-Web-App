using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class stat
    {
        public int room_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
        public int month { get; set; }
        public int cancellations { get; set; }
        public int completed { get; set; }
        public int cancelled { get; set; }
        public int reservations { get; set; }
        public int total_reservations { get; set; }
    }
}