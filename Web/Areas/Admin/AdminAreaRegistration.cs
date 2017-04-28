using System.Web.Mvc;

namespace Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            context.MapRoute(
           "Admin_default",
           "Admin/{controller}/{action}/{id}/{params1}",
           new { controller = "Index", action = "Index", id = UrlParameter.Optional, params1 = UrlParameter.Optional },
           new string[] { "Web.Areas.Admin.Controllers" }
            );
        }
    }
}
