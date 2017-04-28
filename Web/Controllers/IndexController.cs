using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using CRM_System.Model;
using CRM_System.BLL;

namespace Web.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/
        Mpr_Admin_MenuService MenuService = new Mpr_Admin_MenuService();
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            List<Mpr_Admin_Menu> Pglist = MenuService.GetPage(s => s.ID > 0, s => s.AddTime, pageIndex, pageSize, ref totalCount);
            var personsAsIPagedList = new StaticPagedList<Mpr_Admin_Menu>(Pglist, pageIndex, pageSize, totalCount);
            return View(personsAsIPagedList);  
        }
       
    }
}
