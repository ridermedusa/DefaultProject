using CRM_System.BLL;
using CRM_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;
using CRM_System.Tools;

namespace Web.Areas.Admin.Controllers
{
    public class LoginController : BaseAdminController
    {
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Exit()
        {
            SafeExit();
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
        [HttpPost]
        public string LoginSubmit(string uid, string password)
        {
            ReturnJson Result = new ReturnJson();
            Sys_AdminService AdminService = new Sys_AdminService();
            string md5pwd = Tools.ToMD5(password);
            Sys_Admin Mod = AdminService.GetModel(s => s.UName == uid.Trim() && s.LoginPwd == md5pwd);
            if (Mod == null)
            {
                Result.Code = "1";
                Result.Errmsg = "用户名或密码错误";
            }
            else
            {
                if (Mod.IsLock == 1)
                {
                    Result.Code = "1";
                    Result.Errmsg = "当前账户已锁定";
                }
                else
                {
                    currentadminUser = Mod;
                    Result.Code = "0";
                    Result.Errmsg = "";
                }
            }
            return ToJson(Result);
        }

    }
}
