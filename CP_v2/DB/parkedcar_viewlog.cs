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
    
    public partial class parkedcar_viewlog
    {
        public System.Guid id { get; set; }
        public Nullable<System.Guid> parked_car_id { get; set; }
        public Nullable<System.DateTime> date_viewed { get; set; }
        public Nullable<System.Guid> viewed_by { get; set; }
    
        public virtual ap_user ap_user { get; set; }
        public virtual parked_car parked_car { get; set; }
    }
}
