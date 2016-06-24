using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HallsBooking.Areas.Agent.Models
{
    public class ImageModel
    {
        [Required]
        [DisplayName("Select File to Upload")]
        public IEnumerable<HttpPostedFileBase> File { get; set; }  //2nd change
    }
}