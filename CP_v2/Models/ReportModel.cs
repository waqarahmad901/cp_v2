using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class ReportModel
    {
        public double? totalAmount { get; set; }
        public string user { get; set;}
        public int totalParkedIn { get; set;}
        public int totalParkedOut { get; set;}
        public DateTime parkedin { get; set;}
        public DateTime parkedOut { get; set; }
        public string parkedInTime { get { return parkedin.ToString("dd/MM/yyyy") + " " + parkedin.ToShortTimeString(); } }
        public string parkedOutTime { get { return parkedOut.ToString("dd/MM/yyyy") + " " + parkedOut.ToShortTimeString(); } }
    }
}