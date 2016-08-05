using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CP_v2.DB;

namespace CP_v2.Models
{
    public class HomeModel
    {
        public double? day { get; set; }
        public double? Night { get; set; }
        public double? week { get; set; }
        public double? Month { get; set; }
        public double? Yearly { get; set; }

        public List<parked_car> parkedCars { get; set; }
    }
}