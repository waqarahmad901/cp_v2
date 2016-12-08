using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CP_v2.DB;
using CP_v2.Models;
using CP_v2.Util;
using Newtonsoft.Json;

namespace CP_v2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AuthorizeUser(AccessLevel = "CheckIn")]
        public ActionResult Index()
        {

            DataClass da = new DataClass();
            var durationAmount = da.GetDurationAmount();
            HomeModel model = new HomeModel();
            SetCurrentDurantionRate(durationAmount, model);

            // model.parkedCars = da.GetParkedCars();
            model.CurrentTokenNumber = da.GetLastReciptNumber();
            model.ParkedInCars = da.TotalParkedInCars();

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
        [AuthorizeUser(AccessLevel = "CheckOut")]
        public ActionResult Checkout()
        {

            DataClass da = new DataClass();
            var durationAmount = da.GetDurationAmount();
            HomeModel model = new HomeModel();
            SetCurrentDurantionRate(durationAmount, model);

            // model.parkedCars = da.GetParkedCars();
            model.CurrentTokenNumber = da.GetLastReciptNumber();

            model.ParkedInCars = da.TotalParkedInCars();
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

        public ActionResult PrintTokenForCar(string veh_no, string veh_type, bool night, string vehimage)
        {
            DataClass da = new DataClass();
            long ticketNumber = da.GetLastReciptNumber() + 1;

            parked_car pc = new parked_car();
            pc.id = Guid.NewGuid();
            pc.ap_user = da.GetUserByUserName(User.Identity.Name);
            pc.car_no = veh_no.ToUpper();
            pc.date_created = DateTime.Now;
            pc.parkin_time = DateTime.Now;
            pc.veh_type = Guid.Parse(veh_type);
            pc.recript_no = ticketNumber;
            pc.is_nightly = night;
            pc.is_monthly = da.CheckCarRegisterInCurrentMonth(veh_no.ToUpper());
            pc.checkinimage = vehimage;
            da.AddNewParkedCar(pc);

            return Json(new { token = pc.recript_no, parkin_time = pc.parkin_time.Value.ToShortTimeString(), parkin_date = pc.parkin_time.Value.ToShortDateString(), car_no = pc.car_no,
                challaned = ((pc.is_challaned == null || !pc.is_challaned.Value) ? "No" : "Yes") ,
                nightly = ((pc.is_nightly== null || !pc.is_nightly.Value) ? "No" : "Yes")
            }, JsonRequestBehavior.AllowGet);

            //PrintTicket pt = new PrintTicket();
            //pt.CarNumber = veh_no;
            //pt.Price = "0.00";
            //pt.TicketNumber = ticketNumber.ToString();
            //pt.ImageTitle = Server.MapPath("~/images/CBT_Title.png");
            //pt.ImageBarCode = Server.MapPath("~/images/code128bar.jpg");
            ////pt.DrawTicket();
            //Session["val"] = "";

        }
        [HttpGet]
        public ActionResult RePrintTokenForCar(Guid id)
        {
            DataClass da = new DataClass();
            parked_car pc = da.GetParkedCarById(id);
            PrintTicket pt = new PrintTicket();
            pt.CarNumber = pc.car_no;
            pt.Price = "0.00";
            pt.TicketNumber = pc.recript_no.ToString();
            pt.ImageTitle = Server.MapPath("~/images/CBT_Title.png");
            pt.ImageBarCode = Server.MapPath("~/images/code128bar.jpg");
            // pt.DrawTicket();
            return Json(new { token = pc.recript_no, parkin_time = pc.parkin_time.Value.ToShortTimeString(), parkin_date = pc.parkin_time.Value.ToShortDateString(), car_no = pc.car_no, nightly = pc.is_nightly != null && pc.is_nightly.Value == true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetParkedCars(int currentPage, string veh_no, string token_no,int recordPerPage = 10,string parked = "all",string from="",string to= "",string userid = "")
        {
            DataClass da = new DataClass();
            var cars = da.GetParkedCars(currentPage, veh_no, token_no,recordPerPage,parked,from,to,userid);
            cars.TotalParkedCars = da.TotalParkedInCars();
            return Json(cars, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckoutCars(string selectedIds)
        {

            DataClass da = new DataClass();
            Guid userId = da.GetUserByUserName(User.Identity.Name).id;
            var cars = da.CheckoutCars(selectedIds, userId);
            return Json(cars, JsonRequestBehavior.AllowGet);

        }

        private static void SetCurrentDurantionRate(List<dur_amount> durationAmount, HomeModel model)
        {
            model.day = durationAmount.Where(x => x.duration_time.Equals("Day")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.Night = durationAmount.Where(x => x.duration_time.Equals("Night")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.hour = durationAmount.Where(x => x.duration_time.Equals("Hour")).Select(x => x.amount_to_charge).FirstOrDefault().Value;

            model.week = model.day * 7;
            model.Month = durationAmount.Where(x => x.duration_time.Equals("Month")).Select(x => x.amount_to_charge).FirstOrDefault().Value;
            model.Yearly = model.day * 12;
        }
        [HttpGet]
        public ActionResult GetCheckoutTokenInfo(string token_no)
        {
            DataClass da = new DataClass();
            parked_car car = da.GetParkedCarByTokenNo(token_no);
            if (car == null || string.IsNullOrEmpty(token_no))
            {
                    return Content("null");
            }
            
            DateTime duration = new DateTime((DateTime.Now - car.parkin_time.Value).Ticks);
            CheckoutModel cm = new CheckoutModel();
            cm.checkinTime = car.parkin_time.Value.ToShortDateString() + " " + car.parkin_time.Value.ToShortTimeString();
            cm.time = duration.Day - 1+ " Days " +duration.ToString("HH") + " Hours " + duration.ToString("mm") + " Minutes ";
            cm.isMonthly = car.is_monthly == null ? false : car.is_monthly.Value;
            cm.isPaid = car.parkout_time != null;
            cm.isNight = car.is_nightly == null ? false : car.is_nightly.Value;
            cm.tokenNo = car.recript_no;
            cm.carNo = car.car_no;

            if(!cm.isMonthly)
                cm.totalAmount = CalculateAmount(da, car, duration, car.parkin_time.Value);
            else
                cm.totalAmount = 0;
            cm.Id = car.id;
            return Json(cm, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetTotalCarCount()
        {
            DataClass da = new DataClass();
            return Json(da.TotalParkedInCars(), JsonRequestBehavior.AllowGet);
        }
        private static double CalculateAmount(DataClass da, parked_car car, DateTime duration,DateTime parkin_time)
        {
            string[] rates = ConfigurationManager.AppSettings["MinutesRate"].Split(',');
            double totlalAmount;
            DateTime at8am = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 08, 00, 00);
            DateTime at20pm = at8am.AddHours(12);
            DateTime atnext8am = at20pm.AddHours(12);
            bool isNightCalculateAmount = false;
            if (car.is_nightly != null && car.is_nightly.Value)
            {
                if (at8am > parkin_time && DateTime.Now > at8am)
                {
                    isNightCalculateAmount = true;
                    duration = new DateTime((DateTime.Now - at8am).Ticks);
                }
                if (at20pm.AddDays(-1) > parkin_time)
                {
                    isNightCalculateAmount = true;
                    TimeSpan span = at20pm.AddDays(-1) - parkin_time;
                    duration = duration.Add(span);
                }

            }
            var rateList = da.GetRateListByType(car.veh_type.Value);
            var DayRate = rateList.Where(x => x.duration_time.Equals("Day")).Select(x => x.amount_to_charge).FirstOrDefault();
            var hoursRate = rateList.Where(x => x.duration_time.Equals("Hour")).Select(x => x.amount_to_charge).FirstOrDefault();
            var nightRate = rateList.Where(x => x.duration_time.Equals("Night")).Select(x => x.amount_to_charge).FirstOrDefault();
            int hours = duration.Hour;
            int minutesRate = 0;
            for (int i = 0; i < rates.Length; i++)
            {
                string[] rateArray = rates[i].Split(':');
                if (duration.Minute >= int.Parse(rateArray[0].Split('-')[0]) && duration.Minute <= int.Parse(rateArray[0].Split('-')[1]))
                    minutesRate += int.Parse(rateArray[1]);
            }
            totlalAmount = Math.Ceiling((hours * hoursRate.Value)) + minutesRate;
            totlalAmount +=((duration.Day -1) * DayRate)?? 0 ;
            if (car.is_nightly != null && car.is_nightly.Value)
            {
                if (isNightCalculateAmount)
                    totlalAmount = totlalAmount + nightRate.Value;
                else 
                    totlalAmount = nightRate.Value;

            }
            else
            {
                if (hours == 0)
                    totlalAmount =  hoursRate.Value;
            }
            return totlalAmount;
        }

        [HttpGet]
        public ActionResult CheckOutCar(Guid id, string vehimage)
        {
            DataClass da = new DataClass();
            parked_car car = da.GetParkedCarById(id);

            car.parkout_time = DateTime.Now;
            DateTime durationTime = new DateTime((DateTime.Now - car.parkin_time.Value).Ticks);
            CheckoutModel cm = new CheckoutModel();
            car.parked_duration = (durationTime.Day -1) + " Days " + durationTime.ToString("HH") + " Hours " + durationTime.ToString("mm") + " Minutes "; 
            car.out_by = da.GetUserByUserName(User.Identity.Name).id;
            car.checkoutimage = vehimage;

            // var hoursRate = da.GetRateListByType(car.veh_type.Value).Where(x => x.duration_time.Equals("Hour")).Select(x => x.amount_to_charge).FirstOrDefault();
            if (!car.is_monthly.Value)
            {
                car.charged_amount = CalculateAmount(da, car, durationTime, car.parkin_time.Value);
                car.paid_amount = car.charged_amount;
            }
            else
            {
                car.charged_amount = 0.00;
                car.paid_amount = 0.00;
            }
            da.Update();
            Session["val"] = "";
            cm.time = "00 Hours 00 Minutes";
            cm.totalAmount = 0.00;
            cm.parkinCars = da.TotalParkedInCars();
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