using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class PaymentModel
    {
        public string username { get; set; }
        public DateTime received_date { get; set; }
        public decimal amount{ get; set; }
        public string payment_type { get; set; }

        public string received_date_string { get { return received_date.ToString(); } }
    }
}