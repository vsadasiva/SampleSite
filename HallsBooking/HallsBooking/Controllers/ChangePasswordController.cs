using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Controllers
{
    public class ChangePasswordController : Controller
    {
         //
        // GET: /ChangePassword/
        SampleEntities2 db = new SampleEntities2();
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            var pwd = db.Users.SingleOrDefault(x => x.RegId == 1);
            if (pwd.Password == OldPassword)
            {
                pwd.Password = NewPassword;
                pwd.ConfirmPassword = ConfirmPassword;
                db.SaveChanges();
            }
            else
            {
                ViewBag.InValidOldPassword = "InValid Old Password";
                return View("ChangePassword");
            }
            return View();
        }

    }
}
