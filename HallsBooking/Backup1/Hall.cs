//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HallsBooking
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hall
    {
        public int HallId { get; set; }
        public string HallName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string LandMark { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserId { get; set; }
        public string PinCode { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public Nullable<decimal> CostWithDecaration { get; set; }
        public Nullable<decimal> CostWithOutDecaration { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}