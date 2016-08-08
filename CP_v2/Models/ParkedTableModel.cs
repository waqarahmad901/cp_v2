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

        public string token_no_current { get { return Cars.FirstOrDefault().tokenNo.ToString(); } }
    }
}