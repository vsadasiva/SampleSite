using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallsBooking.Filters
{
    public class CustomException:FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Session["Error"] = "There is Some Error. While Precessing your Request..";
           // var model = new HandleErrorInfo(filterContext.Exception, "Error", "Error");
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                MasterName = "~/Views/Shared/_Layout.cshtml",
            };
        }
    }
}