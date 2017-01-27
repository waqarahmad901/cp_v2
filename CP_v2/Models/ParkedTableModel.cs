using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class ParkedTableModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<ParkedCars> Cars { get; set; }

        public double? totalAmount { get; set; }
        public int TotalParkedCars { get; set; }

        public string token_no_current { get { return Cars.Count > 0 ? Cars.FirstOrDefault().tokenNo.ToString() : ""; } }


    }
}