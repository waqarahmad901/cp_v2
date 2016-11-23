using System;
using System.Web.Mvc;
using CP_v2.DB;


namespace CP_v2.Controllers
{
    [Authorize]
    public class MonthlySubscriptionController : Controller
    {
        // GET: MonthlySubscription
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddSubscription(string carbike,string cnic,string mobilenumber,double amount,string month,string ownername)
        {
            DataClass da = new DataClass();
            monthly_reg reg = new  monthly_reg();
            reg.id = Guid.NewGuid();
            reg.vehicle_no = carbike;
            reg.cnic = cnic;
            reg.contact_no = mobilenumber;
            reg.amount = amount;
            reg.month_name = month;
            reg.ownername = ownername;
            reg.date_registered = DateTime.Now;
            da.AddRegistration(reg);
            return Content("added");
        }

        public ActionResult GetAllSubscriptions(string carno, string name)
        {
            DataClass da = new DataClass();
            var subscription = da.GetAllSubscriptions(carno,name, 1);

            return Json(subscription, JsonRequestBehavior.AllowGet);
        }



    }
}