using CRM_System.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class IndexController : BaseAdminController
    {
        //
        // GET: /Admin/Index/

        public ActionResult Index()
        { 
            if (currentadminUser == null)
            {
                currentadminUser = new CRM_System.Model.Sys_Admin();
            }
            return View(currentadminUser);
        }

    }
}
