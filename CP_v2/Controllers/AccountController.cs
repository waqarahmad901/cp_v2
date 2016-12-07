using System;
using System.Globalization;
using System.Linq;

using System.Web.Mvc;

using CP_v2.DB;
using System.Web.Security;
using System.Collections.Generic;

namespace CP_v2.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult GetAllUsers(bool addSelect = false)
        {
            try
            {

                DataClass da = new DataClass();
                var users = da.GetAllUsers().Select(x => new { name = x.username, id = x.id }).ToList();
                if (addSelect)
                {
                    users.Insert(0, new { name = "All", id = new Guid() });
                }
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;

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
                if (user.ap_role.Title.Equals("CheckOut"))
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