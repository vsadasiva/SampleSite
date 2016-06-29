using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Areas.Agent.Models
{
    public static class DbContext
    {
        public static List<Country> GetCountries()
        {
            SampleEntities db = new SampleEntities();
            return db.Countries.ToList();
        }
        public static List<State> GetStates(int countryId)
        {
            SampleEntities db = new SampleEntities();
            var lstStates = (List<State>)db.States.Select(x => x.CountryId == countryId);
            return (List<State>)db.States.Where(x => x.CountryId == countryId).ToList();
        }
        public static List<City> GetCities(int stateId)
        {
            SampleEntities db = new SampleEntities();
            return (List<City>)db.Cities.Where(x => x.StateId == stateId);
        }
        public static void AddImages(Hall hall)
        {
            //SampleEntities db = new SampleEntities();
            //HallImage objhallImage = new HallImage();
            //foreach (var item in hall.HallImage.File)  //3rd change
            //{
            //    byte[] uploadFile = new byte[item.InputStream.Length];
            //    item.InputStream.Read(uploadFile, 0, uploadFile.Length);
            //    objhallImage.ImageName = item.FileName;
            //    objhallImage.ImageFile = uploadFile;
            //    objhallImage.HallId = hall.HallId;
            //    objhallImage.CreatedBy = "admin";
            //    objhallImage.CreatedOn = DateTime.Now;
            //    db.HallImages.Add(objhallImage);
            //    db.SaveChanges();
            //}           

        }
        
    }
}