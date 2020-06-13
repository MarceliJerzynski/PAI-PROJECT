using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Championships.Models;
using MvcPaging;

namespace Championships.Controllers
{
    public class TournamentsController : Controller
    {
        private DashboardDbContext db = new DashboardDbContext();

        private int PageSize = 10;
        // GET: Tournaments
        public ActionResult Index(string searchText, int page = 0, string registered = "false")
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var tournaments = db.Tournaments.ToList();

            if (registered.Equals("true"))
            {
                string userMail = Session["UserMail"].ToString();

                var loggedUser = db.Users.Where(registeredUser => registeredUser.Mail.Equals(userMail)).FirstOrDefault();

                tournaments = tournaments.Where(tournament => tournament.RegisteredUsers.Contains(loggedUser)).ToList();
            }
            if (!String.IsNullOrEmpty(searchText))
            {
                tournaments = tournaments.Where(item => item.Name.Contains(searchText)).ToList();
            }

            var count = tournaments.Count();

            var data = tournaments.OrderBy(o => o.Name).Skip(page * PageSize).Take(PageSize).ToList();

            this.ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);

            this.ViewBag.Page = page;

            return this.View(data);
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(string id)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            string userMail = Session["UserMail"].ToString();
            var loggedUser = db.Users.Where(registeredUser => registeredUser.Mail.Equals(userMail)).FirstOrDefault();

            var model = new Tuple<Tournament, User>(
                tournament,
                loggedUser);

            return View(model);
        }

        public ActionResult TakePart(string id)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Id = id;

            string userMail = Session["UserMail"].ToString();
            var loggedUser = db.Users.Where(registeredUser => registeredUser.Mail.Equals(userMail)).FirstOrDefault();

            Tournament tournament = db.Tournaments.Find(id);


            tournament.RegisteredUsers.Add(loggedUser);

            db.Tournaments.Attach(tournament);
            //db.Entry(tournament).State = EntityState.Modified;
            db.SaveChanges();
            //loggedUser.registeredTournaments.Add(tournament);

            //db.Users.Attach(loggedUser);
            //db.SaveChanges();
            //db.Entry(loggedUser).State = EntityState.Modified;
            //db.SaveChanges();

            return View();
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Tournaments/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Date,MaxAmountOfUsers, Discipline, Sponsors, DateToRegister")] Tournament tournament)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if (db.Tournaments.Any(registeredTournament=> registeredTournament.Name == tournament.Name))
                {
                    Response.Write("<script>alert('Tournament with this name already exists');</script>");
                    return View();
                }

                tournament.Organizer = Session["UserMail"].ToString();

                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(string id)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Date,MaxAmountOfUsers, Discipline, Sponsors, DateToRegister")] Tournament tournament)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(string id)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Login/Create
        public ActionResult RegisterUserOnTournament()
        {
            if (@Session["UserMail"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
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
