using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class SubscriptionTableModel
    {
        
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<SubscriptionModel> Subscriptions { get; set; }
    }
}