using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Championships.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserMail"] == null)
            {
                Session["UserMail"] = "Guest";
            }
            return View();
        }

        public ActionResult Contact()
        {
            if(@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignOut()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Session["UserMail"] = "Guest";
            return View();
        }
    }
}