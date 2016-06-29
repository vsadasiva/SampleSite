using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Areas.Agent.Controllers
{
    public class ChangePasswordController : Controller
    {
        //
        // GET: /Agent/ChangePassword/

        SampleEntities db = new SampleEntities();
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(int Id, string OldPassword, string NewPassword, string ConfirmPassword)
        {
            Id = 1;
            var pwd = db.Users.SingleOrDefault(x => x.RegId == Id);
            if (pwd.Password == OldPassword && pwd.ConfirmPassword == ConfirmPassword)
            {
                pwd.Password = NewPassword;
                pwd.ConfirmPassword = ConfirmPassword;
                db.SaveChanges();
            }
            else if (pwd.ConfirmPassword == ConfirmPassword)
            {
                ViewBag.InValidOldPassword = "InValid Old Password";
                return View("ChangePassword");
            }
            else
            {
                ViewBag.InValidOldPassword = "New password and Confirm Pawssword should match";
                return View("ChangePassword");
            }
            return View();
        }

    }
}
