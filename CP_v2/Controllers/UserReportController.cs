using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CP_v2.Util;

namespace CP_v2.Controllers
{
    public class UserReportController : Controller
    {
        [AuthorizeUser(AccessLevel = "SuperAdmin")]
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchReport(string from, string to)
        {
            DataClass da = new DataClass();
            var model = da.GetUserReport(from, to);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllCars()
        {
 
            return View();
        }


    }
}