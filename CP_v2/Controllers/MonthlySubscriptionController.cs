using System;
using System.Web.Mvc;
using CP_v2.DB;
using CP_v2.Util;

namespace CP_v2.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin")]
    public class MonthlySubscriptionController : Controller
    {
        // GET: MonthlySubscription
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddSubscription(string id, string carbike, string cnic, string mobilenumber, double amount, string month, string ownername)
        {
            try
            {
                DataClass da = new DataClass();
                monthly_reg reg = null;
                if (id == "")
                {
                    reg = new monthly_reg();
                    reg.id = Guid.NewGuid();
                }
                else
                    reg = da.GetRegistrationbyId(id);
                reg.vehicle_no = carbike.ToUpper();
                reg.cnic = cnic;
                reg.contact_no = mobilenumber;
                reg.amount = amount;
                reg.month_name = month;
                reg.ownername = ownername;
                reg.date_registered = DateTime.Now;
                if (id == "")
                    da.AddRegistration(reg);
                else
                    da.Update();
                return Content("added");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;
        }

        public ActionResult GetAllSubscriptions(string carno, string name, string month, int page)
        {
            try
            {
                DataClass da = new DataClass();
                var subscription = da.GetAllSubscriptions(carno, name, month, page);

                return Json(subscription, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;
        }
        public ActionResult GetSubcriptionbyId(string id)
        {
            try
            {
                DataClass da = new DataClass();
                var subscription = da.GetSubcriptionbyId(id);

                return Json(subscription, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;
        }

        public ActionResult DeleteSubcriptionbyId(string id)
        {
            try
            {
                DataClass da = new DataClass();
                var subscription = da.DeleteSubcriptionbyId(id);

                return Json(subscription, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;
        }



    }
}