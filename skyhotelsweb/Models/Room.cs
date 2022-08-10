using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class Room
    {
        public int id { get; set; }
        public string filename { get; set; }
        public string room_num { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string description { get; set; }
        public decimal rate { get; set; }
        public string status { get; set; }
        public List<media> media { get; set; }
    }
}