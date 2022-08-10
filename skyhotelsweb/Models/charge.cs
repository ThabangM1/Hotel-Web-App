using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skyhotelsweb.Models
{
    public class Charge
    {
        public string item { get; set; }
        public int quantity { get; set; }
        public Decimal charge { get; set; }
    }
}