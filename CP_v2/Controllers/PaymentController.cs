using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CP_v2.DB;
using CP_v2.Models;
using CP_v2.Util;

namespace CP_v2.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin")]
    public class PaymentController : Controller
    {
        // GET: Paymemt
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddPayment(string username, decimal amount, string paymenttype)
        {
            DataClass da = new DataClass();
            payment payment = new payment();
            payment.id = Guid.NewGuid();
            payment.amount = amount;
            payment.ap_user = da.GetUserByUserName(username);
            payment.payment_type = paymenttype;
            payment.received_date = DateTime.Now;
            da.AddPayment(payment); 
            return Content("added");
        }

        public ActionResult GetAllPayments(string userName)
        {
            DataClass da = new DataClass();
           var payments = da.GetAllPayemnts(userName,1);

            return Json(payments, JsonRequestBehavior.AllowGet);
        }
    }
}