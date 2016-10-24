using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HallsBooking.Filters
{
    public class CustomAuthorize:AuthorizeAttribute
    {
        SampleEntities db = new SampleEntities();
        private readonly string[] allowUsers;
        public CustomAuthorize(params string[] users)
        {
            this.allowUsers = users;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
           // string url = httpContext.Request.RawUrl;
            bool authorize = false;
            if(httpContext.Session["User"]!=null)
            {
                foreach (var roles in allowUsers)
                {
                    string userId = Areas.Halls.Constants.Constant.Decrypt(httpContext.Session["User"].ToString());
                    int id = Convert.ToInt32(userId);
                    var role = db.Users.Where(x => x.RegId == id).FirstOrDefault().Role;
                    if (role == roles)
                    {
                        authorize = true;
                    }
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string returnUrl = filterContext.HttpContext.Request.RawUrl;
            if (filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area="Agent",controller = "Users", action = "RegisterUser", ReturnUrl= returnUrl }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Users", action = "UnAuthorize" }));
            }
        }
    }
}