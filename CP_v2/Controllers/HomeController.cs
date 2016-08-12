using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CP_v2.DB;
using CP_v2.Models;

namespace CP_v2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            DataClass da = new DataClass();
            var durationAmount = da.GetDurationAmount();
            HomeModel model = new HomeModel();
            SetCurrentDurantionRate(durationAmount, model);

            // model.parkedCars = da.GetParkedCars();
            model.CurrentTokenNumber = da.GetLastReciptNumber();
            if (Convert.ToString(Session["val"]) != string.Empty)
            {
                ViewBag.pic = "~/images/" + Session["val"].ToString();
            }
            else
            {
                ViewBag.pic = "~/images/car-img.jpg";
            }
            return View(model);
        }
        public ActionResult Checkout()
        {

            DataClass da = new DataClass();
            var durationAmount = da.GetDurationAmount();
            HomeModel model = new HomeModel();
            SetCurrentDurantionRate(durationAmount, model);

            // model.parkedCars = da.GetParkedCars();
            model.CurrentTokenNumber = da.GetLastReciptNumber();
            if (Convert.ToString(Session["val"]) != string.Empty)
            {
                ViewBag.pic = "~/images/" + Session["val"].ToString();
            }
            else
            {
                ViewBag.pic = "~/images/car-img.jpg";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
           
  

            return View();
        }

        public ActionResult PrintTokenForCar(string veh_no,string veh_type, bool night,string vehimage)
        {
            DataClass da = new DataClass();
            long ticketNumber = da.GetLastReciptNumber() + 1;

            parked_car pc = new parked_car();
            pc.id = Guid.NewGuid();
            pc.ap_user = da.GetUserByUserName(User.Identity.Name);
            pc.car_no = veh_no;
            pc.date_created = DateTime.Now;
            pc.parkin_time = DateTime.Now;
            pc.veh_type = Guid.Parse(veh_type);
            pc.recript_no = ticketNumber;
            pc.is_nightly = night;
            pc.is_monthly = da.CheckCarRegisterInCurrentMonth(veh_no);
            pc.checkinimage = vehimage;
            da.AddNewParkedCar(pc);

            PrintTicket pt = new PrintTicket();
            pt.CarNumber = veh_no;
            pt.Price = "0.00";
            pt.TicketNumber = ticketNumber.ToString();
            pt.ImageTitle = Server.MapPath("~/images/CBT_Title.png");
            pt.ImageBarCode = Server.MapPath("~/images/code128bar.jpg");
            //pt.DrawTicket();
            Session["val"] = "";
            return Json(ticketNumber,JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult RePrintTokenForCar(Guid id)
        {
            DataClass da = new DataClass();
            parked_car car = da.GetParkedCarById(id);
            PrintTicket pt = new PrintTicket();
            pt.CarNumber = car.car_no;
            pt.Price = "0.00";
            pt.TicketNumber = car.recript_no.ToString();
            pt.ImageTitle = Server.MapPath("~/images/CBT_Title.png");
            pt.ImageBarCode = Server.MapPath("~/images/code128bar.jpg");
            pt.DrawTicket();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetParkedCars(int currentPage,string veh_no, string token_no)
        {
            DataClass da = new DataClass();
            var cars = da.GetParkedCars(currentPage, veh_no, token_no);
            return Json(cars, JsonRequestBehavior.AllowGet);
        }

        private static void SetCurrentDurantionRate(List<dur_amount> durationAmount, HomeModel model)
        {
            model.day = durationAmount.Where(x => x.duration_time.Equals("Day")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.Night = durationAmount.Where(x => x.duration_time.Equals("Night")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.week = model.day * 7;
            model.Month = durationAmount.Where(x => x.duration_time.Equals("Month")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.Yearly = model.day * 12;
        }
        [HttpGet]
        public ActionResult GetCheckoutTokenInfo(string token_no)
        {
            DataClass da = new DataClass();
            parked_car car = da.GetParkedCarByTokenNo(token_no);
            if (car == null)
                return Content("null");
            DateTime duration = new DateTime((DateTime.Now - car.parkin_time.Value).Ticks);
            CheckoutModel cm = new CheckoutModel();
            cm.time =   duration.ToString("HH") + " Hours " + duration.ToString("mm") + " Minutes ";
            cm.isMonthly = car.is_monthly == null ? false : car.is_monthly.Value;
            cm.isPaid = car.parkout_time != null;
            cm.totalAmount = 0.00;
            cm.Id = car.id;
            return Json(cm,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckOutCar(Guid id,string vehimage)
        {
            DataClass da = new DataClass();
            parked_car car = da.GetParkedCarById(id);
            car.parkout_time = DateTime.Now;
            DateTime duration = new DateTime((DateTime.Now - car.parkin_time.Value).Ticks);
            CheckoutModel cm = new CheckoutModel();
            car.parked_duration = duration.ToString("HH") + " Hours " + duration.ToString("mm") + " Minutes ";
             car.out_by = da.GetUserByUserName(User.Identity.Name).id;
            car.checkoutimage = vehimage;
            da.Update();
            Session["val"] = "";
            return Json(cm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}