using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallsBooking.Areas.Agent.Models;

namespace HallsBooking.Areas.Agent.Controllers
{
    public class BookHallController : Controller
    {
        //
        // GET: /Agent/BookHall/

        public ActionResult BookHall()
        {
            List<FunctionType> EnumValues = new List<FunctionType>();
            EnumValues.Add(FunctionType.Birthday);
            EnumValues.Add(FunctionType.GetToGather);
            EnumValues.Add(FunctionType.Marriage);
            EnumValues.Add(FunctionType.NamingCeremony);
            EnumValues.Add(FunctionType.Reception);

            ViewBag.FunctionType = EnumValues;
            return View();
        }
    }
}
