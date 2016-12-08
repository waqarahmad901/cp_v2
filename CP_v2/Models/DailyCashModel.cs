using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class DailyCashModel
    {
        public DailyCashModel() {
            shiftsTable = new List<DailyCahTable>();
        }
        public double? CashForShift { get {return shiftsTable.Select(x => x.totalAmount).Sum(); } }
        public int TotalVehicleIn { get { return shiftsTable.Select(x => x.totalParkedIn).Sum(); } }

        public int TotalVehicleOut { get { return shiftsTable.Select(x => x.totalParkedOut).Sum(); } }

        public int TotalVehicleInMonthly { get { return shiftsTable.Select(x => x.totalParkedInMonthly).Sum(); } }

        public int TotalVehicleOutMonthly { get { return shiftsTable.Select(x => x.totalParkedOutMonthly).Sum(); } }


        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public List<DailyCahTable> shiftsTable { get; set; } 

    }
}