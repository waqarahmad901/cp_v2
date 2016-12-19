using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP_v2.Models
{
    public class UserModel
    {
        public bool blocked { get; set; }

        public Guid Id { get; set; }
        public string UserName{get;set;}
        public string first_name{get;set;}
        public string last_name{get;set;}
        public string cnic{get;set; }
        public string role{get;set; }
        public string phone_no { get;set; }
    }
}