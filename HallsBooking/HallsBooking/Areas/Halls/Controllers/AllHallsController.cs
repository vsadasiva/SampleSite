using HallsBooking.Filters;
using System;
using System.Collections.Generic;
using System.IO;
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
           // int x = 0;int y = 1;int result = y / x;
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
        public ActionResult ShowAllHalls()
        {
            List<Hall> halls = db.Halls.OrderBy(h => h.HallId).Take(2).ToList();
            return View(halls);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ShowMoreHalls(int size)
        {
            //var model = from h in db.Halls
            //             join hi in db.HallImages on h.HallId equals hi.HallId
            //             select new Hall {HallId=h.HallId,HallName=h.HallName };
            var model = db.Halls.OrderBy(h => h.HallId).Skip(size).Take(2);
            int modelCount = db.Halls.Count();
            if(model.Any())
            {
                string modelString = RenderRazorViewToString("_LoadMoreHalls", model);
                return Json(new { ModelString = modelString, ModelCount = modelCount });
            }
            return Json(model);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext =
                     new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}