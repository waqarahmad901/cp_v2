//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CP_v2.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class payment
    {
        public System.Guid id { get; set; }
        public System.Guid userid { get; set; }
        public decimal amount { get; set; }
        public System.DateTime received_date { get; set; }
        public string payment_type { get; set; }
    
        public virtual ap_user ap_user { get; set; }
    }
}
