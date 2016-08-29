using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Home()
        {
            SampleEntities db = new SampleEntities();
            var content = db.HallImages;
            return View(content);
        }
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(int Id)
        {
            SampleEntities db = new SampleEntities();
            var q = from temp in db.HallImages where temp.HallId == Id select temp.ImageFile;
            byte[] cover = q.FirstOrDefault();
            return cover;
        }
    }
}
