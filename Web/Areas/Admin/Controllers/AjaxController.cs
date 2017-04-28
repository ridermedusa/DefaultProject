using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class AjaxController : BaseAdminController
    {
        //
        // GET: /Admin/Ajax/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public string GetLeftMenu()
        {
            return GetLeftMenuInfo();
        }
        [HttpPost]
        public string GetButtonRole(int pageid)
        {
            return JsonConvert.SerializeObject(ButtonRoleList(pageid));
        }
    }
}
