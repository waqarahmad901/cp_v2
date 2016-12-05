using System;
using System.Globalization;
using System.Linq;

using System.Web.Mvc;

using CP_v2.DB;
using System.Web.Security;

namespace CP_v2.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            DataClass da = new DataClass();
            var users = da.GetAllUsers().Select(x=> new {name = x.username }).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);

        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection collection)
        {
            string UserName = collection["username"];
            string Password = collection["password"];
            // validate user name and pass word
            DataClass da = new DataClass();
            ap_user user = da.VerifyUserLogin(UserName, Password);
            ViewBag.IsLogin = user != null;
            Session["User"] = user;
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(UserName, false);
                if(user.ap_role.Title.Equals("CheckOut"))
                    return RedirectToAction("CheckOut", "Home");
                return RedirectToAction("Index", "Home");
            }
            

            return View();

        }

        public ActionResult Register()
        {
            return View();
        }

        //
        // GET: /Account/VerifyCode

        //
        // POST: /Account/Register
      
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
       
    }
}