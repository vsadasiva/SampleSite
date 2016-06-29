using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Areas.Agent.Controllers
{
    public class HallsRegController : Controller
    {
        private SampleEntities db = new SampleEntities();

        //
        // GET: /Agent/HallsReg/

        public ActionResult Index()
        {
            List<Hall> lstHalls = db.Halls.ToList();

            return View(db.Halls.ToList());
        }

        //
        // GET: /Agent/HallsReg/Details/5

        public ActionResult HallsDetails(int id = 0)
        {
            Hall hall = db.Halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
        }

        //
        // GET: /Agent/HallsReg/Create

        public ActionResult HallRegistration()
        {
            List<Country> countries = db.Countries.ToList();
            ViewBag.Countries = HallsBooking.Areas.Agent.Models.DbContext.GetCountries();
            return View();
        }

        //
        // POST: /Agent/HallsReg/Create
        [HttpPost]
        public ActionResult HallRegistration(Hall hall)
        {
            if (ModelState.IsValid)
            {
                hall.CreatedDate = DateTime.Now;
                hall.CreatedBy = "admin";
                db.Halls.Add(hall);
                db.SaveChanges();
                HallsBooking.Areas.Agent.Models.DbContext.AddImages(hall);
                return RedirectToAction("Index");
            }
            return View(hall);
        }

        //
        // GET: /Agent/HallsReg/Edit/5

        public ActionResult HallDetailsEdit(int id = 0)
        {
            //Hall hall = db.Halls.Find(id);
            //IQueryable<HallImage> hallImages = db.HallImages.Where(x => x.HallId == id);
            //hall.HallImages = hallImages;
            //if (hall == null)
            //{
            //    return HttpNotFound();
            //}
            //bindDropdowns(Convert.ToInt32(hall.Country), Convert.ToInt32(hall.State));
            //return View(hall);
            return View();
        }

        //
        // POST: /Agent/HallsReg/Edit/5
        [HttpPost]
        public ActionResult HallDetailsEdit(Hall hall)
        {
            bindDropdowns(Convert.ToInt32(hall.Country), Convert.ToInt32(hall.State));
            if (ModelState.IsValid)
            {
                hall.CreatedDate = DateTime.Now;
                db.Entry(hall).State = EntityState.Modified;
                db.SaveChanges();
                //
                HallsBooking.Areas.Agent.Models.DbContext.AddImages(hall);
                return RedirectToAction("Index");
            }
            return View(hall);
        }
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
        //
        // GET: /Agent/HallsReg/Delete/5

        public ActionResult DeleteHall(int id = 0)
        {
            Hall hall = db.Halls.Find(id);
            if (hall == null)
            {
                return HttpNotFound();
            }
            return View(hall);
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

        //
        // POST: /Agent/HallsReg/Delete/5

        [HttpPost, ActionName("DeleteHall")]
        public ActionResult DeleteConfirmed(int id)
        {
            Hall hall = db.Halls.Find(id);
            db.Halls.Remove(hall);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public FileContentResult FileDownload(int? id)
        //{
        //    byte[] fileData;
        //    string fileName;

        //    HallImage fileRecord = db.HallImages.Find(id);

        //    fileData = (byte[])fileRecord.ImageFile.ToArray();
        //    fileName = fileRecord.ImageName;

        //    return File(fileData, "text", fileName);
        //}
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}