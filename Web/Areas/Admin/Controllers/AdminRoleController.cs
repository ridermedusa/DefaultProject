using CRM_System.BLL;
using CRM_System.DAL;
using CRM_System.Model;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class AdminRoleController : BaseAdminController
    {
        //
        // GET: /Admin/AdminRole/
        Sys_RoleService RoleService = new Sys_RoleService();
        public ActionResult Index()
        {
            List<Sys_Role> Plist = RoleService.FindAll();
            if (Plist.Count == 0)
            {
                ViewBag.pagecount = "1";
                ViewBag.ALLitemcount = "1";
            }
            else
            {
                ViewBag.pagecount = RoleService.GetPageCount(Plist, 10).ToString();
                ViewBag.ALLitemcount = Plist.Count().ToString();
            }
            return View();
        }
        public ActionResult AdminRole(string id = "")
        {
            ViewBag.ID = id;
            return View();
        }
        [HttpPost]
        public string GetTableinfo(int? pageindex, string serchJson)
        {
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
            if (Pageindex < 1)
            {
                Pageindex = 1;
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
            string SafeListinfo = JsonConvert.SerializeObject(new Sys_RoleService().GetPage(s => s.ROLENAME.Contains(keyword), s => s.ID, Pageindex, 10));
            return SafeListinfo;
        }
        [HttpPost]
        public string DelRow(string RowID)
        { 
            ReturnJson Rejson = new ReturnJson();
            int reint = new Sys_RoleService().DelBy(s => s.ID == RowID);
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
        [HttpPost]
        public string Controller(string Type)
        {
            string Result = "";
            switch (Type)
            {
                case "GetModel":
                    Result = Getmode(Request["ID"].ToString());
                    break;
                case "save":
                    Sys_Role Mod = JsonConvert.DeserializeObject<Sys_Role>(Request["data"]);
                    Result = Save(Mod);
                    break;
                case "GetHtml":
                    Result = GetHtml();
                    break;
            }
            return Result;
        }
        Mpr_Admin_MenuService MenuService = new Mpr_Admin_MenuService();
        Mpr_Admin_ButtonRoleService ButtonRoleService = new Mpr_Admin_ButtonRoleService();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetHtml()
        {
            string html = "<ul style=\"list-style:none;\">";
            //获取全部最上级菜单
            List<Mpr_Admin_Menu> ListMenu = MenuService.FindByParam(s => s.RightParent == 0);
            foreach (var item in ListMenu)
            {
                html += " <li><input type=\"checkbox\" name=\"menuinfo\" value=\"" + item.ID + "\" />" + item.RightName;
                html += GetDGHtml(item.ID);
                html += "</li>";
            }
            html += "</ul>";
            return html;
        }
        /// <summary>
        /// 递归方法
        /// </summary>
        /// <param name="ParenId"></param>
        /// <returns></returns>
        public string GetDGHtml(int ParenId)
        {

            string html = "";
            //通过传入的上级ID获取下级列表数据,如果存在则开始递归，如果不存在则跳出递归
            List<Mpr_Admin_Menu> ListMenu = MenuService.FindByParam(s => s.RightParent == ParenId);
            //获取全部按钮信息列表
            List<Mpr_Admin_ButtonRole> ButtonList = ButtonRoleService.FindAll();
            if (ListMenu.Count > 0)
            {
                //开始返回数据
                html += "<ul style=\"list-style:none;    margin-left: 5%;\">";
                //获取下级数据信息
                foreach (var item in ListMenu)
                {
                    html += "<li><input type=\"checkbox\" name=\"menuinfo\" id=\"menuinfo_" + ParenId + "_" + item.ID + "\" value=\"" + item.ID + "\" />";
                    html += item.RightName;
                    Mpr_Admin_Menu Exmod = MenuService.GetModel(s => s.RightParent == item.ID);
                    if (Exmod != null)
                    {
                        //继续尝试递归
                        html += GetDGHtml(item.ID);
                    }
                    html += "</li>";
                    if (item.IsButton == 1)
                    {
                        //判断当前是否存在对应按钮数据信息
                        List<Mpr_Admin_ButtonRole> NowButtonList = ButtonList.Where(s => s.PageID == item.ID).ToList();
                        if (NowButtonList.Count > 0)
                        {
                            //标示当前页面存在可操作按钮，开始拼接一个LI内的UL
                            html += "<li>";
                            html += "<ul style=\"list-style:none;margin-left:5px;\">";
                            foreach (var Buttonitem in NowButtonList)
                            {
                                html += "<li><input type=\"checkbox\" name=\"IsButton\" id=\"IsButton_" + Buttonitem.PageID + "_" + Buttonitem.ID + "\" value=\"" + Buttonitem.ID + "\" />" + Buttonitem.Operation + "</li>";
                            }
                            html += "</ul>";
                            html += "</li>";
                        }
                    }
                }
                html += "</ul>";
            }
            return html;
        }
        /// <summary>
        /// 返回单条数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Getmode(string ID)
        {
            ReturnObject Rejson = new ReturnObject();
            try
            { 
                Rejson.Code = "0";
                Rejson.Errmsg = "Success";
                Rejson.GetModel = RoleService.GetModel(s => s.ID == ID);
            }
            catch (Exception ex)
            {
                Rejson.Code = "1";
                Rejson.Errmsg = ex.Message;
            }
            return ToJson(Rejson);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public string Save(Sys_Role mod)
        {
            ReturnJson Rejson = new ReturnJson();
            if (mod.ID == "")
            {
                mod.ID = Guid.NewGuid().ToString("N");
                //添加
                mod = RoleService.Insert(mod);
                if (mod != null)
                {
                    Rejson.Code = "0";
                    Rejson.Errmsg = "添加成功";
                }
                else
                {
                    Rejson.Code = "1";
                    Rejson.Errmsg = "添加失败";
                }
            }
            else
            {
                //修改
                //尝试通过ID获取数据库内已有信息
                Sys_Role DBmod = RoleService.GetModel(s => s.ID == mod.ID);
                if (DBmod != null)
                {
                    EntityToEntity(mod, ref DBmod);
                    DBmod = RoleService.Update(DBmod);
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
    }
}
