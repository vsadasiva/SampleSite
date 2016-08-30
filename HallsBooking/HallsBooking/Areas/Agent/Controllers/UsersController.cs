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
using HallsBooking.Areas.Agent.Models;

namespace HallsBooking.Areas.Agent.Controllers
{
    [CustomException]
    public class UsersController : Controller
    {
        private SampleEntities db = new SampleEntities();

        /// <summary>
        /// Displaying the all Users Data...
        /// </summary>
        [CustomAuthorize("admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        /// <summary>
        /// Display the Single UserData
        /// </summary>
        [CustomAuthorize("user", "admin")]
        public ActionResult Details()
        {
            string id = Request["id"];
            if (id != null)
            {
                int userID = Convert.ToInt32(Constant.Decrypt(id));
                if (userID == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(userID);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return HttpNotFound();
            }
        }
        /// <summary>
        /// New User registration
        /// </summary>
        public ActionResult RegisterUser()
        {
            List<Country> countries = db.Countries.ToList();
            ViewBag.Countries = countries;
            ViewBag.ReturnUrl = Request["ReturnUrl"];
            if (Session["EmailExits"]!=null)
            {
                ViewBag.EmailExits = Session["EmailExits"];
                Session["EmailExits"] = null;
            }
            return View();
        }
        /// <summary>
        /// store the New User data into database.
        /// </summary>
        /// <param name="user">Getting the user Filled data using JQuery</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Email != null)
                {
                    if (!HallsBooking.Areas.Agent.Models.DbContext.IsMailIDExists(user.Email))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        //return Json(new { Message = "EmailIdExists" }, JsonRequestBehavior.AllowGet);
                        Session["EmailExits"] = "Email is already Exists";
                        return RedirectToAction("RegisterUser");
                    }
                }
            }

            return View(user);
        }
        /// <summary>
        /// Bind the States
        /// </summary>
        /// <param name="CountryId">Getting the Current Country ID</param>
        public JsonResult BindStates(int CountryId)
        {
            List<State> states = db.States.Where(s => s.CountryId == CountryId).ToList();
            return Json(new SelectList(states.ToArray(), "StateId", "StateName"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Bind the Cities
        /// </summary>
        /// <param name="StateId"> Getting the Current StateId</param>
        public JsonResult BindCites(int StateId)
        {
            List<City> cites = db.Cities.Where(s => s.StateId == StateId).ToList();
            return Json(new SelectList(cites.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Edit page for Current User.
        /// </summary>
        [CustomAuthorize("user", "admin")]
        public ActionResult Edit()
        {
            string id = Request["id"];
            if (id != null)
            {
                int userId = Convert.ToInt32(Constant.Decrypt(id));
                // int id = Convert.ToInt32(Request["id"]);
                if (userId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(userId);
                ViewBag.RegId = userId;
                //int countryId = db.Countries.SingleOrDefault(x => x.CountryName == user.Country).CountryId;
                //int stateId = db.States.SingleOrDefault(x => x.StateName == user.State).StateId;
                //int cityId = db.Cities.SingleOrDefault(x => x.CityName == user.City).CityId;

                bindDropdowns(Convert.ToInt32(user.Country), Convert.ToInt32(user.State));
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            return View("_Error");
        }
        /// <summary>
        /// Binding The DropDowns For User Selected Values.
        /// </summary>
        private void bindDropdowns(int countryId, int stateId)
        {
            if (ViewBag.Countries == null)
            {
                ViewBag.Countries = HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
            }
            if (ViewBag.States == null)
            {
                var states = db.States.Where(x => x.CountryId == countryId).ToList();
                ViewBag.States = states;
            }
            if (ViewBag.Cities == null)
            {
                var cities = db.Cities.Where(x => x.StateId == stateId).ToList();
                ViewBag.Cities = cities;
            }
        }
        //private void bindDropdowns(int countryId, int stateId, int cityId)
        //{
        //    if (ViewBag.Countries == null)
        //    {
        //        List<Country> country = HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
        //        ViewBag.Countries = new SelectList(country, "CountryId", "CountryName", countryId);
        //        //ViewBag.Countries = HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
        //    }
        //    if (ViewBag.States == null)
        //    {
        //        var states = db.States.Where(x => x.CountryId == countryId).ToList();
        //        ViewBag.States = new SelectList(states, "StateId", "StateName", stateId);
        //        //ViewBag.States = states;
        //    }
        //    if (ViewBag.Cities == null)
        //    {
        //        var cities = db.Cities.Where(x => x.StateId == stateId).ToList();
        //        ViewBag.Cities = new SelectList(cities, "CityId", "CityName", cityId);
        //        //ViewBag.Cities = cities;
        //    }
        //}
        /// <summary>
        /// Update the user Edited data to database.
        /// </summary>
       // [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(User obj)
        {
            if (ModelState.IsValid)
            {
                User objUser = new HallsBooking.User();
                var result = db.Users.SingleOrDefault(x => x.RegId == obj.RegId);
                if (result != null)
                {
                    result.FirstName = obj.FirstName;
                    result.LastName = obj.LastName;
                    result.Email = obj.Email;
                    result.Mobile = obj.Mobile;
                    result.Country = obj.Country;
                    result.State = obj.State;
                    result.City = obj.City;
                    result.Pincode = obj.Pincode;
                    result.AgentorMember = obj.AgentorMember;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    //return Json(new { RedirectUrl = Url.Action("Index", "Users"), SuccessMessage = "valid" }, JsonRequestBehavior.AllowGet);
                }
            }
            return View();
        }
        /// <summary>
        /// Method for Deletion of user data..
        /// </summary>
        [CustomAuthorize("user", "admin")]
        public ActionResult Delete(string id)
        {
            int userID = Convert.ToInt32(Constant.Decrypt(id));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(userID);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        /// <summary>
        /// Method for deletion Confirmation.
        /// </summary>
        public ActionResult DeleteConfirmed()
        {
            string userID = Request["id"].ToString();
            int id = Convert.ToInt32(Constant.Decrypt(userID));
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Method for Checking the userCredentials.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string Email, string Password, string ReturnUrl)
        {
            string redirectUrl = string.Empty;
            bool isValid = db.Users.Any(x => x.Email == Email && x.Password == Password);
            if (isValid)
            {
                int userid = db.Users.SingleOrDefault(x => x.Email == Email).RegId;
                if (userid != 0)
                {
                    string uid = userid.ToString();
                    string id = Constant.Encrypt(uid);
                    Session["User"] = id;
                    if (ReturnUrl != null)
                    {
                        if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                   && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                        {
                            redirectUrl = ReturnUrl;
                        }
                        else
                        {
                            redirectUrl = "/Home/Home";
                        }
                    }
                    //FormsAuthentication.SetAuthCookie(Email, true);
                }
                return Redirect(redirectUrl);
                //return Json(new { RedirectUrl = redirectUrl, SuccessMessage = "valid" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Url.Action("RegisterUser", "Users")
                redirectUrl = "/Users/RegisterUser";
                ViewBag.InValidUser = "Invalid UserId or Password";
                return Redirect(redirectUrl);
                //return Json(new { RedirectUrl = redirectUrl, SuccessMessage = "invalid" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Method for Logout The user..
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["User"] = null;
            //FormsAuthentication.SignOut();
            return RedirectToAction("RegisterUser");
        }
        /// <summary>
        /// Method for ForgottenPassword 
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgottenPassword()
        {
            return View();
        }
        /// <summary>
        /// checking the EmailID is Valid or Not
        /// </summary>
        /// <param name="Email">MailID</param>
        [HttpPost]
        public ActionResult ForgottenPassword(string Email)
        {
            bool result = db.Users.Any(x => x.Email == Email);
            if (!result)
            {
                ViewBag.Result = "Email does not exist";
                return View();
            }
            else
            {
                PasswordLink(Email);
                return View();
            }
        }
        /// <summary>
        /// Method for Sending the ResetPassword Link to User MailID.
        /// </summary>
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
            ModelState.Clear();
            ViewBag.Result = "Reset Password Link Is Sent to Your Email...";
        }
        /// <summary>
        /// Method for Reset password.
        /// </summary>
        public ActionResult ResetPassword()
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Time"] != null)
            {
                Session["Email"] = Constant.Decrypt(Request.QueryString["Id"].ToString());
                string Datetime = Constant.Decrypt(Request.QueryString["Time"].ToString());
                DateTime time = Convert.ToDateTime(Datetime.Replace('-', ' '));
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
        /// <summary>
        /// Method for Store the Passwords into database.
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="ConfirmPassword"></param>
        /// <returns></returns>
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
        [CustomAuthorize("user", "admin")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string Password, string ConfirmPassword)
        {
            int id = Convert.ToInt32(Constant.Decrypt(Session["User"].ToString()));
            var user = db.Users.Where(x => x.RegId == id).FirstOrDefault();
            if (user.Password == OldPassword)
            {
                user.Password = Password;
                user.ConfirmPassword = ConfirmPassword;
                db.SaveChanges();
                Session["User"] = null;
                return RedirectToAction("RegisterUser");
            }
            else
            {
                ViewBag.InValidOldPassword = "InValid Old Password";
                return View();
            }
        }
        public ActionResult UnAuthorize()
        {
            return View();
        }
    }
}