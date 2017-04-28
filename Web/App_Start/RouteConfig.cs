using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",
               "{controller}/{action}/{id}/{params1}",
                 new { controller = "Index", action = "Index", id = UrlParameter.Optional, params1 = UrlParameter.Optional },
               new string[] { "Web.Areas.Admin.Controllers" }
           ).DataTokens.Add("area", "Admin");    
        }
    }
}