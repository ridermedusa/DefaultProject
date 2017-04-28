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
    public class MenuController : BaseAdminController
    {
        //
        // GET: /Admin/Menu/
        Mpr_Admin_MenuService AdminMenuService = new Mpr_Admin_MenuService();
        public ActionResult Index()
        {
            List<Mpr_Admin_Menu> Plist = AdminMenuService.FindAll();
            if (Plist.Count == 0)
            {
                ViewBag.pagecount = "1";
                ViewBag.ALLitemcount = "1";
            }
            else
            {
                ViewBag.pagecount = AdminMenuService.GetPageCount(Plist, 10).ToString();
                ViewBag.ALLitemcount = Plist.Count().ToString();
            }
            return View();
        }
        public ActionResult MenuIndex(int id = 0)
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
            string SafeListinfo = JsonConvert.SerializeObject(new Mpr_Admin_MenuRepository().GetPage(keyword, Pageindex, 10));
            return SafeListinfo;
        }

        [HttpPost]
        public string DelRow(string RowID)
        {
            int Rid = Convert.ToInt32(RowID);
            ReturnJson Rejson = new ReturnJson();
            int reint = new Mpr_Admin_MenuService().DelBy(s => s.ID == Rid);
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
           string Result= "";
           switch (Type)
           {
               case "GetModel":
                   Result = Getmode(Request["ID"].ToString());
                   break;
               case "save":
                   Mpr_Admin_Menu Mod = JsonConvert.DeserializeObject<Mpr_Admin_Menu>(Request["data"]);
                   List<Mpr_Admin_ButtonRole> RoleList = JsonConvert.DeserializeObject<List<Mpr_Admin_ButtonRole>>(Request["roledata"]);
                   Result = Save(Mod, RoleList);
                   break;
           }
            return Result;
        }


        Mpr_Admin_MenuService MenuService = new Mpr_Admin_MenuService();
        /// <summary>
        /// 根据ID查询返回单条数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        #region
        public string Getmode(string  ID)
        {
            ReturnObject Rejson = new ReturnObject();
            try
            {
                int Rid = Convert.ToInt32(ID);
                Rejson.Code = "0";
                Rejson.Errmsg = "Success";
                Rejson.GetModel = MenuService.GetModel(s => s.ID == Rid);
                Rejson.GetInList = BtnRoleService.FindByParam(s => s.PageID == Rid);
            }
            catch (Exception ex)
            {
                Rejson.Code = "1";
                Rejson.Errmsg = ex.Message;
            }
            return ToJson(Rejson);
        }
        #endregion

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>

        #region
        public string Save(Mpr_Admin_Menu mod, List<Mpr_Admin_ButtonRole> List)
        {
            ReturnJson Rejson = new ReturnJson();
            if (mod.ID == 0)
            {
                //添加
                mod.Sort = 1;
                mod.AddTime = DateTime.Parse(DateTime.Now.ToString());
                mod = MenuService.Insert(mod);
                if (mod != null)
                {
                    foreach (var item in List)
                    {
                        item.PageID = mod.ID;
                    }
                    SaveBtnRole(List);
                    Rejson.Code = "0";
                    Rejson.Errmsg = "Add success";
                }
                else
                {
                    Rejson.Code = "1";
                    Rejson.Errmsg = "Add failed";
                }
            }
            else
            {
                //修改
                //尝试通过ID获取数据库内已有信息
                Mpr_Admin_Menu DBmod = MenuService.GetModel(s => s.ID == mod.ID);
                if (DBmod != null)
                {
                    EntityToEntity(mod, ref DBmod);
                    DBmod = MenuService.Update(DBmod);
                    if (DBmod != null)
                    {
                        SaveBtnRole(List);
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


        Mpr_Admin_ButtonRoleService BtnRoleService = new Mpr_Admin_ButtonRoleService();
        public void SaveBtnRole(List<Mpr_Admin_ButtonRole> List)
        {
            if (List != null)
            {
                if (List.Count > 0)
                {
                    int pageid = List[0].PageID.Value;
                    List<Mpr_Admin_ButtonRole> Mpr_AdminRole = BtnRoleService.FindByParam(s => s.PageID == pageid);
                    for (int i = 0; i < List.Count; i++)
                    {
                        //判断数据库中是否存在  
                        Mpr_Admin_ButtonRole VaMod = Mpr_AdminRole.Where(s => s.ID == List[i].ID).FirstOrDefault();
                        if (VaMod == null)
                        {
                            //添加
                            List[i].Addtime = DateTime.Now;
                            List[i] = BtnRoleService.Insert(List[i]);
                        }
                        else
                        {
                            //修改
                            EntityToEntity(List[i], ref VaMod);
                            List[i] = BtnRoleService.Update(VaMod);
                        }
                    }
                }
            }
        }
        #endregion


    }
}
