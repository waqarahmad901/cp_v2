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
    
    public partial class View_1
    {
        public string car_no { get; set; }
        public Nullable<System.Guid> veh_type { get; set; }
        public Nullable<System.DateTime> parkin_time { get; set; }
        public Nullable<System.DateTime> parkout_time { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public Nullable<double> charged_amount { get; set; }
        public Nullable<double> paid_amount { get; set; }
        public string parked_duration { get; set; }
        public int recript_no { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public Nullable<bool> is_challaned { get; set; }
        public Nullable<bool> is_monthly { get; set; }
        public Nullable<System.Guid> out_by { get; set; }
        public string assigned_no { get; set; }
    }
}
