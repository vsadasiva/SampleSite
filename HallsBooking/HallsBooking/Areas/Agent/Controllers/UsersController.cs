using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HallsBooking;
using System.Net.Mail;

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
                return RedirectToAction("Index", "Users");
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
        public ActionResult Login(string Email, string Password)
        {
            bool isValid = db.Users.Any(x => x.Email == Email && x.Password == Password);
            if (isValid)
            {
                Session["User"] = Email;
                return Json(new { RedirectUrl = Url.Action("Index", "Users"), SuccessMessage = "valid" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.InValidUser = "Invalid UserId or Password";
                return Json(new { RedirectUrl = Url.Action("RegisterUser", "Users"), SuccessMessage = "invalid" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ForgottenPassword()
        {
            if (Session["InvalidEmail"] != null)
            {
                ViewBag.Result = Session["InvalidEmail"];
                Session["InvalidEmail"] = null;
            }
            else if(Session["Message"]!=null)
            {
                ViewBag.Message = Session["Message"];
                Session["Message"] = null;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ForgottenPassword(string Email)
        {
            bool result = db.Users.Any(x => x.Email == Email);
            if (!result)
            {
                Session["InvalidEmail"] = "Email does not exist";
                return RedirectToAction("ForgottenPassword");
            }
            else
            {
                PasswordLink(Email);
                return RedirectToAction("ForgottenPassword");
            }
        }
        [NonAction]
        public void PasswordLink(string Email)
        {
            string dateTime = DateTime.Now.ToString().Replace(' ', '-');
            var message = new MailMessage();
            message.To.Add(new MailAddress(Email));
            message.From = new MailAddress("sitecoreteam7.5@gmail.com");
            message.Subject = "Password Reset";
            message.Body = "http://localhost:51946/Agent/Users/ResetPassword?Id=" + Email + "&Time=" + dateTime;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "sitecoreteam7.5@gmail.com",  // replace with valid value
                    Password = "sitecore@75"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            Session["Message"] = "Reset Password Link Is Sent to Your Email...";
        }
        public ActionResult ResetPassword()
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Time"] != null)
            {
                Session["Email"] = Request.QueryString["Id"].ToString();
                DateTime time = Convert.ToDateTime((Request.QueryString["Time"].ToString()).Replace('-', ' '));
                DateTime currentTime = DateTime.Now;
                int hours = Convert.ToInt32(currentTime.Subtract(time).TotalHours);
                if (hours > 24)
                {
                    ViewBag.ResetPasswordLinkExpires = "Reset Password Link Expired";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(String Password,string ConfirmPassword)
        {
            string Email = Session["Email"].ToString();
            var result = db.Users.SingleOrDefault(x => x.Email == Email);
            //int result = GetUser.RsetPassword(obj, Email.ToString());
            if (result !=null)
            {
                result.Password = Password;
                result.ConfirmPassword = ConfirmPassword;
                db.SaveChanges();
                return RedirectToAction("RegisterUser");
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
