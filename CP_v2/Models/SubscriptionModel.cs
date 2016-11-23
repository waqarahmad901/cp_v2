using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class SubscriptionModel
    {
        public string carno { get; set; }
        public string mobileno { get; set; }

        public string cninc { get; set; }

        public double? amount { get; set; }
        public string month { get; set; }

        public string ownername { get; set; }


    }
}