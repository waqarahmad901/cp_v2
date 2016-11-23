using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CP_v2.Controllers
{
    public class UserReportController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchReport(string from, string to)
        {
            DataClass da = new DataClass();
            var total = da.GetUserReport(from, to);
            return Json(total, JsonRequestBehavior.AllowGet);
        }


    }
}