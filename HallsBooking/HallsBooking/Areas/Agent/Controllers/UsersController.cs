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
using HallsBooking.Areas.Halls.Constants;
using System.Web.Security;
using HallsBooking.Filters;

namespace HallsBooking.Areas.Agent.Controllers
{
    //[CustomException]
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
            //int id = Convert.ToInt32(Constant.Decrypt(Request["id"]));
            if (id == 0)
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
            //int id = Convert.ToInt32(Constant.Decrypt(Request["id"]));
           // int id = Convert.ToInt32(Request["id"]);
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            ViewBag.RegId = id;
            int countryId = db.Countries.SingleOrDefault(x => x.CountryName == user.Country).CountryId;
            int stateId = db.States.SingleOrDefault(x => x.StateName == user.State).StateId;
            int cityId = db.Cities.SingleOrDefault(x => x.CityName == user.City).CityId;
            bindDropdowns(countryId,stateId,cityId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        private void bindDropdowns(int countryId, int stateId,int cityId)
        {
            if (ViewBag.Countries == null)
            {
                List<Country> country= HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
                ViewBag.Countries = new SelectList(country, "CountryId", "CountryName", countryId);
                //ViewBag.Countries = HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
            }
            if (ViewBag.States == null)
            {
                var states = db.States.Where(x => x.CountryId == countryId).ToList();
                ViewBag.States = new SelectList(states, "StateId", "StateName", stateId);
                //ViewBag.States = states;
            }
            if (ViewBag.Cities == null)
            {
                var cities = db.Cities.Where(x => x.StateId == stateId).ToList();
                ViewBag.Cities = new SelectList(cities, "CityId", "CityName", cityId);
                //ViewBag.Cities = cities;
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditUser(User obj)
        {
            if (ModelState.IsValid)
            {
                // var url = Url.RequestContext.RouteData.Values["id"];
                var result = db.Users.SingleOrDefault(x => x.RegId == obj.RegId);
                if(result!=null)
                {
                    db.Entry(result).CurrentValues.SetValues(obj);
                    return Json(new { RedirectUrl = Url.Action("Index", "Users"), SuccessMessage = "valid" }, JsonRequestBehavior.AllowGet);
                }
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            return View();
        }



        // POST: Agent/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RegId,FirstName,LastName,Password,ConfirmPassword,Email,Mobile,Country,State,City,Pincode,AgentorMember,IsActive")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}

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
                int userid = db.Users.SingleOrDefault(x => x.Email == Email).RegId;
                if (userid != 0)
                {
                    string uid = userid.ToString();
                    string id = Constant.Encrypt(uid);
                    Session["User"] = id;
                    FormsAuthentication.SetAuthCookie(Email, true);
                }
                return Json(new { RedirectUrl = Url.Action("Index", "Users"), SuccessMessage = "valid" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.InValidUser = "Invalid UserId or Password";
                return Json(new { RedirectUrl = Url.Action("RegisterUser", "Users"), SuccessMessage = "invalid" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("RegisterUser");
        }
        public ActionResult ForgottenPassword()
        {
            if (Session["InvalidEmail"] != null)
            {
                ViewBag.Result = Session["InvalidEmail"];
                Session["InvalidEmail"] = null;
            }
            else if (Session["Message"] != null)
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
            string EmailId = Constant.Encrypt(Email);
            string dateTime = Constant.Encrypt(DateTime.Now.ToString().Replace(' ', '-'));
            var message = new MailMessage();
            message.To.Add(new MailAddress(Email));
            message.From = new MailAddress("sitecoreteam7.5@gmail.com");
            message.Subject = "Password Reset";
            message.Body = "http://localhost:51946/Agent/Users/ResetPassword?Id=" + EmailId + "&Time=" + dateTime;
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
                Session["Email"] = Constant.Decrypt(Request.QueryString["Id"].ToString());
                string Datetime = Constant.Decrypt(Request.QueryString["Time"].ToString());
                DateTime time= Convert.ToDateTime(Datetime.Replace('-', ' '));
                Session["Datetime"] = time;
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
        public ActionResult ResetPassword(String Password, string ConfirmPassword)
        {
            string Email = Session["Email"].ToString();
            var result = db.Users.SingleOrDefault(x => x.Email == Email);
            //int result = GetUser.RsetPassword(obj, Email.ToString());
            if (result != null)
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