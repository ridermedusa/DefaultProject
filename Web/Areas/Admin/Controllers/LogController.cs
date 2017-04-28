using CRM_System.BLL;
using CRM_System.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class LogController : BaseAdminController
    {
        //
        // GET: /Admin/Log/
        Sys_Error_logService ErrorLogService = new Sys_Error_logService();
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            List<Sys_Error_log> Pglist = ErrorLogService.GetPage(s => true, s => s.Date, pageIndex, pageSize, ref totalCount);
            var ResultList = new StaticPagedList<Sys_Error_log>(Pglist, pageIndex, pageSize, totalCount);
            return View(ResultList);
        }
    }
}
