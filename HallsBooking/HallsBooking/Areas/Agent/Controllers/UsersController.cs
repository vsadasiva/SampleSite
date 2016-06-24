using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HallsBooking;

namespace HallsBooking.Areas.Agent.Controllers
{
    public class UsersController : Controller
    {
        private SampleEntities db = new SampleEntities();

        // GET: Agent/Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Agent/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Agent/Users/Create
        public ActionResult RegisterUser()
        {
            List<Country> countries = db.Countries.ToList();
            ViewBag.Countries = countries;
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterUsers(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        public JsonResult BindStates(int CountryId)
        {
            List<State> states = db.States.Where(s => s.CountryId == CountryId).ToList();
            return Json(new SelectList(states.ToArray(), "StateId", "StateName"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindCites(int StateId)
        {
            List<City> cites = db.Cities.Where(s => s.StateId == StateId).ToList();
            return Json(new SelectList(cites.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
        }
        // GET: Agent/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Agent/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegId,FirstName,LastName,Password,ConfirmPassword,Email,Mobile,Country,State,City,Pincode,AgentorMember,IsActive")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Agent/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Agent/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Login(string Email,string Password)
        {
            bool isValid = db.Users.Any(x => x.Email == Email && x.Password == Password);
            if (isValid)
            {
                Session["User"] = Email;
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
