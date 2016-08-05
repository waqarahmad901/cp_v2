using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CP_v2.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo
        public ActionResult Index()
        {
            Session["val"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Imagename)
        {
            string sss = Session["val"].ToString();

            ViewBag.pic = (Request.ApplicationPath + "/images/" + Session["val"].ToString()).Replace("//", "/");

            return View();
        }

        public JsonResult Rebind()
        {
            string path = (Request.ApplicationPath + "/images/" + Session["val"].ToString()).Replace("//", "/"); ;

            return Json(path, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();

                DateTime nm = DateTime.Now;

                string date = nm.ToString("yyyymmddMMss");

                var path = Server.MapPath("~/images/" + date + "test.jpg");

                System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

                ViewData["path"] = date + "test.jpg";

                Session["val"] = date + "test.jpg";
            }

            return View("Index");
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;

            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
    }
}