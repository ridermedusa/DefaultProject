using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class BasicsConfigController : BaseAdminController
    {
        //
        // GET: /Admin/BasicsConfig/

        public ActionResult Index()
        {
            return View();
        }

    }
}
