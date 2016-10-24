using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HallsBooking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
           (s, cert, chain, sslPolicyErrors) => true;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
