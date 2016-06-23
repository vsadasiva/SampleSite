using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallsBooking;

namespace HallsBooking.Controllers
{
    public class ChangePasswordController : Controller
    {
         //
        // GET: /ChangePassword/

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword()
        {

        }

    }
}
