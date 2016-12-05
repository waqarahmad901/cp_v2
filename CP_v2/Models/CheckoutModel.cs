using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class CheckoutModel
    {
        public string time { get; set; }
        public double totalAmount { get; set; }
        public bool isNight { get; set; }
        public Guid Id{ get; set; }
        public bool isMonthly { get; set; }
        public bool isPaid { get; set; }

        public String checkinTime { get; set; }

        public int parkinCars { get; set; }
        public string carNo { get; set; }
        public long tokenNo{ get; set; }


    }
}