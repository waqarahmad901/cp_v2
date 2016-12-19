using System;
using System.Globalization;
using System.Linq;

using System.Web.Mvc;

using CP_v2.DB;
using System.Web.Security;
using System.Collections.Generic;
using CP_v2.Models;
using CP_v2.Util;

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

        public ActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection collection)
        {
          
            string newpass = collection["new"];
            DataClass da = new DataClass();
            var user= da.GetUserByUserName(User.Identity.Name);
            user.password = newpass;
            da.Update();
           
            return RedirectToAction("passwordchanged");

        }

        public ActionResult passwordchanged()
        {
            return View();

        }

        public ActionResult UserManagement(Guid? id)
        {
            ap_user user = null;
            DataClass da = new DataClass();
            var roles = da.GetRoles();
            ViewBag.Roles = new SelectList((from f in roles  select new { f.Id, f.Title }).ToList(), "Id", "Title");
            if (id == null)
                user = new ap_user();
            else
                user = da.GetUserById(id.Value);
            return View(user);

        }

        [HttpPost]
        public ActionResult UserManagement(ap_user user)
        {
            DataClass da = new DataClass();
            if (user.id == Guid.Empty)
            { 
                var roles = da.GetRoles();
                ViewBag.Roles = new SelectList((from f in roles select new { f.Id, f.Title }).ToList(), "Id", "Title"); 
                var userexist = da.GetUserByUserName(user.username);
                if (userexist != null)
                {
                    userexist.id = Guid.Empty;
                    ViewBag.isUserExist = true;
                    return View(userexist);
                }
                user.id = Guid.NewGuid(); 
                da.AddUser(user);
            }
            else
                da.UpdateUser(user);
            return RedirectToAction("ManageUsers");

        }
        [AuthorizeUser(AccessLevel = "SuperAdmin")]
        public ActionResult ManageUsers()
       {
            return View();
        }
      
        public ActionResult GetManageUsers()
        {
            DataClass da = new DataClass();
            List<UserModel> users = da.GetAllUsers().Select(x => new UserModel
            {
                Id = x.id,
                cnic = x.cnic,
                first_name = x.first_name,
                last_name = x.last_name,
                phone_no = x.phone_no,
                role = x.ap_role == null ? "" : x.ap_role.Title,
                UserName = x.username,
                blocked = x.is_block
            }).ToList();
            return Json(users,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUser(Guid id)
        {
            DataClass da = new DataClass();
            var user = da.GetUserById(id);
            da.DeleteUser(user);
            return Content("deleted");
        }

    }
}