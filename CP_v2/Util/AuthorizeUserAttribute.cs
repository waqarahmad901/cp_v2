using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
 
namespace CP_v2.Util
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            var person = new DataClass().GetUserByUserName(httpContext.User.Identity.Name); // Call another method to get rights of the user from DB

            if (person.ap_role.Title.Equals("SuperAdmin") || person.ap_role.Title.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}