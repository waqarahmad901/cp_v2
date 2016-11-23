using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class ParkedCars
    {
        public bool? monthly;
        public bool? night;

        public DateTime? checkinDate { get; set; }

        public string checkinDateString { get { return checkinDate.Value.ToShortDateString(); } }

        public Guid Id { get; set; }
        public long tokenNo { get; set; }
        public string vehicle_NO { get; set; }
        public string checkinTime { get {return checkinDate.Value.ToString("dd/MM/yyyy") + " " + checkinDate.Value.ToShortTimeString(); } }
        public DateTime? checkoutDate { get; set; }
        public double? Amount { get; set; }
        public string Duration { get; set; }
        public string checkouttimeString { get { return checkoutDate == null ? "" : checkoutDate.Value.ToString("dd/MM/yyyy") + " " + checkoutDate.Value.ToShortTimeString(); } }

        public string checkinby { get; set; }
    }
}