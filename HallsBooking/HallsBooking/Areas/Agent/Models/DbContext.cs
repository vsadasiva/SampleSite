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
        
    }
}