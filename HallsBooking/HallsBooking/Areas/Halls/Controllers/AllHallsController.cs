using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Areas.Halls.Controllers
{
    public class AllHallsController : Controller
    {
        SampleEntities db = new SampleEntities();
        // GET: Halls/AllHalls
        public ActionResult Halls()
        {
            List<Country> countries = db.Countries.ToList();
            ViewBag.Countries = countries;
            return View();
        }

        public ActionResult GetAllHalls(int CityId)
        {
            string id = CityId.ToString();
            var halls = db.Halls.Where(x => x.City == id).ToList();
            //.Select(x => new { x.HallId, x.HallName, x.Address })
            //var hall = db.Halls.Select(x => new { x.HallId, x.HallName, x.Address,x.City }).Where(x=>x.City==id).ToList();
            //  return Json(halls, JsonRequestBehavior.AllowGet);
            return PartialView("HallsDetails", halls);
        }
    }
}