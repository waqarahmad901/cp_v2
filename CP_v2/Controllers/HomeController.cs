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

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            DataClass da = new DataClass();
            long ticketNumber = da.GetLastReciptNumber() + 1;

            parked_car pc = new parked_car();
            pc.id = Guid.NewGuid();
            pc.ap_user = da.GetUserByUserName(User.Identity.Name);
            pc.car_no = collection["name"];
            pc.date_created = DateTime.Now;
            pc.parkin_time = DateTime.Now;
            pc.veh_type = Guid.Parse(collection["veh_type"]);
            pc.recript_no = ticketNumber;
            da.AddNewParkedCar(pc);

            PrintTicket pt = new PrintTicket();
            pt.CarNumber = collection["name"];
            pt.Price = "0.00";
            pt.TicketNumber = ticketNumber.ToString();
            pt.ImageTitle = Server.MapPath("~/images/CBT_Title.png");
            pt.ImageBarCode = Server.MapPath("~/images/code128bar.jpg");
            pt.DrawTicket();

            var durationAmount = da.GetDurationAmount();
            HomeModel model = new HomeModel();
            SetCurrentDurantionRate(durationAmount, model);
            model.CurrentTokenNumber = ticketNumber;

           // model.parkedCars = da.GetParkedCars();

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