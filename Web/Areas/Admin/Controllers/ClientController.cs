using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ClientController : Controller
    {
        //
        // GET: /Admin/Client/

        public ActionResult Index(string Message = "")
        {
           
            return View();
        }
    }
}
