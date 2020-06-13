using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Championships.Models;

namespace Championships.Controllers
{
    public class RegisterController : Controller
    {
        private DashboardDbContext db = new DashboardDbContext();

        //GET: Register
        public ActionResult Index()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Users.ToList());
        }

        //// GET: Register/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        // GET: Register/Create
        public ActionResult Register()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Register/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Mail,Password")] User user)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if (db.Users.Any(randomUser => randomUser.Mail == user.Mail))
                {
                    Response.Write("<script>alert('Account with this e-mail addres already exists');</script>");
                    return View();
                }

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsUserExists(string mail)
        {
            return Json(!db.Users.Any(user => user.Mail == mail), JsonRequestBehavior.AllowGet);
        }
    }
}
