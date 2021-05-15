using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bishe1.Models
{
    public class Notice
    {
        public int id { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public Nullable<System.DateTime> subtime { get; set; }
        public Nullable<System.DateTime> edittime { get; set; }
    }
}