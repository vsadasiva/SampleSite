using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using HallsBooking;

namespace HallsBooking.Areas.Agent.Models
{
    public enum FunctionType
    {
        Birthday,
        Marriage,
        Reception,
        GetToGather,
        NamingCeremony
    }
    public class BookHall
    {
        
        public Hall Hall {get;set;}

        public BookingDetail BookingDetail { get; set; }
    }
}