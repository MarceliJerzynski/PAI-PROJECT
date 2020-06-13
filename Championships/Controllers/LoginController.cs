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
    public class LoginController : Controller
    {
        private DashboardDbContext db = new DashboardDbContext();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Login/Create
        public ActionResult Login()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Login/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Mail,Password")] User user)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var potentialUser = db.Users.Where(registeredUser => registeredUser.Mail.Equals(user.Mail) && 
                                                                     registeredUser.Password.Equals(user.Password)).FirstOrDefault();
                if (potentialUser != null) //good mail and password
                {
                    Session["UserMail"] = potentialUser.Mail;
                    return RedirectToAction("Index", "Tournaments");
                }
                else
                {
                    Response.Write("<script>alert('Incorrect e-mail address or password');</script>");
                    return View();
                }

            }

            return View(user);
        }

        public ActionResult ForgotPassword()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword([Bind(Include = "Id, Mail, ForgotPasswordKey, Password")] User user)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var forgetfulUser = db.Users.Where(registeredUser => registeredUser.Mail.Equals(user.Mail) && 
                                                                     registeredUser.ForgotPasswordKey.Equals(user.ForgotPasswordKey)).FirstOrDefault();

                if (forgetfulUser != null) //good mail and forgotPasswordKey
                {
                    forgetfulUser.Password = user.Password;
                    db.Entry(forgetfulUser).State = EntityState.Modified;
                    db.SaveChanges();
                    Response.Write("<script>alert('Your password have been changed');</script>");
                    return RedirectToAction("Login");
                }
                else
                {
                    Response.Write("<script>alert('Incorrect e-mail addres or forgotPassword key');</script>");
                    return View();
                }
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
    }
}
