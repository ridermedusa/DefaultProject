using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_System.BLL;
using CRM_System.Model;
using CRM_System.Tools;
using Web.Base;
using Web.Areas.Admin.Models; 

namespace Web.Areas.Admin.Controllers
{
    public class Sys_AdminController : BaseAdminController
    {
        //
        // GET: /Admin/Sys_Admin/

        Sys_AdminService AdminService = new Sys_AdminService(); 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(int id = 0)
        {
         
            ViewBag.ID = id;
            if (id != 0)
            {
                Sys_Admin AMod = AdminService.GetModel(s => s.ID == id); 
                return View(AMod);
            }
            else
            {
                return View(new Sys_Admin());
            }
        }
        public ActionResult MyAdmin()
        {
            ViewBag.ID = currentadminUser.ID;
            if (currentadminUser.ID != 0)
            {
                return View(AdminService.GetModel(s => s.ID == currentadminUser.ID));
            }
            else
            {
                return View(new Sys_Admin());
            }
        }
        [HttpPost]
        public string Controller(string Type)
        {
            string Result = "";
            switch (Type)
            {
                case "save":
                    Sys_Admin AMod = JsonConvert.DeserializeObject<Sys_Admin>(Request["data"].ToString());
                    Result = Save(AMod);
                    break;
                case "Mysave":
                    Result = Mysave(JsonConvert.DeserializeObject<Sys_Admin>(Request["data"].ToString()));
                    break;
                case "Delete":
                    Result = DelRow(Convert.ToInt32(Request["ID"].ToString()));
                    break;
                case "GetPage":
                    Result = GetPage(Request["serchJson"], Request["pageindex"]);
                    break;

            }
            return Result;
        }
        private string DelRow(int RowID)
        {
            ReturnJson Rejson = new ReturnJson();
            int reint = AdminService.DelBy(s => s.ID == RowID);
            if (reint > 0)
            {
                Rejson.Code = "0";
                Rejson.Errmsg = "删除成功";

            }
            else
            {
                Rejson.Code = "1";
                Rejson.Errmsg = "删除失败";
            }
            return ToJson(Rejson);
        }
        private string Mysave(Sys_Admin Mod)
        {
            ReturnJson Rejson = new ReturnJson();
            //修改
            //尝试通过ID获取数据库内已有信息
            Sys_Admin DBmod = AdminService.GetModel(s => s.ID == Mod.ID);
            if (DBmod != null)
            {
                if (string.IsNullOrEmpty(Mod.LoginPwd.Trim()))
                {
                    Mod.LoginPwd = DBmod.LoginPwd;
                }
                else
                {
                    Mod.LoginPwd = Tools.ToMD5(Mod.LoginPwd);
                }
                Mod.UName = DBmod.UName;
                Mod.RoleID = DBmod.RoleID;
                Mod.RoleName = DBmod.RoleName;
                Mod.IsLock = DBmod.IsLock;
                Mod.LastLogin = DBmod.LastLogin;
                Mod.LastLoginIP = DBmod.LastLoginIP;
                EntityToEntity(Mod, ref DBmod);
                DBmod = AdminService.Update(DBmod);
                if (DBmod != null)
                {
                    Rejson.Code = "0";
                    Rejson.Errmsg = "保存成功";
                }
                else
                {
                    Rejson.Code = "1";
                    Rejson.Errmsg = "保存失败";
                }
            }
            else
            {
                Rejson.Code = "1";
                Rejson.Errmsg = "保存失败";
            }
            return ToJson(Rejson);
        }
        private string Save(Sys_Admin Mod)
        {
            ReturnJson Rejson = new ReturnJson();
            if (Mod.ID == 0)
            {
                //如果当前为添加用户，先获取当前中最多可创建业务员 
                int ALLCount = AdminService.FindByParam(s => true).Count();
         
                string Pwd = Tools.ToMD5(Mod.LoginPwd);
                Mod.LoginPwd = Pwd; 
                Mod.Addtime = DateTime.Now;  
                    Mod = AdminService.Insert(Mod);
                    if (Mod != null)
                    {
                        Rejson.Code = "0";
                        Rejson.Errmsg = "保存成功";

                    }
                    else
                    {
                        Rejson.Code = "1";
                        Rejson.Errmsg = "保存失败";
                    } 
            }
            else
            {
                //修改
                //尝试通过ID获取数据库内已有信息
                Sys_Admin DBmod = AdminService.GetModel(s => s.ID == Mod.ID);
                if (DBmod != null)
                {
                    if (!string.IsNullOrEmpty(Mod.LoginPwd.Trim()))
                    {
                        Mod.LoginPwd = Tools.ToMD5(Mod.LoginPwd);
                    }
                    else
                    {
                        Mod.LoginPwd = DBmod.LoginPwd;
                    }
                    Mod.LastLogin = DBmod.LastLogin;
                    Mod.LastLoginIP = DBmod.LastLoginIP;
                    EntityToEntity(Mod, ref DBmod);
                    DBmod = AdminService.Update(DBmod);
                    if (DBmod != null)
                    {
                        Rejson.Code = "0";
                        Rejson.Errmsg = "保存成功";
                    }
                    else
                    {
                        Rejson.Code = "1";
                        Rejson.Errmsg = "保存失败";
                    }
                }
                else
                {
                    Rejson.Code = "1";
                    Rejson.Errmsg = "保存失败";
                }
            }
            return ToJson(Rejson);
        }
        private string GetPage(string serchJson, string pageindex)
        {
            ReturnObject Result = new ReturnObject();
            string GetTableinfo = serchJson;
            //根据页数和条件获取数据源
            int Pageindex = 1;
            try
            {

                Pageindex = Convert.ToInt32(pageindex);
            }
            catch (Exception ex)
            {
                //跳出
            }
            string keyword = "";
            if (!string.IsNullOrEmpty(GetTableinfo))
            {
                JsonData SerJD = JsonMapper.ToObject(GetTableinfo);
                try
                {
                    keyword = SerJD["keyword"].ToString();
                }
                catch (Exception ex)
                {

                }
            }
            int ItemCount = 0;
            List<Sys_Admin> FileSystemList = AdminService.GetPage(s => s.UName.Contains(keyword) || s.TrueName.Contains(keyword), s => s.ID, Pageindex, 10, ref ItemCount);
            Result.Code = "0";
            Result.GetModel = FileSystemList;
            Result.ItemCount = ItemCount;
            Result.PageCount = GetPageCount(ItemCount, 10);
            return ToJson(Result);
        }


    }
}
