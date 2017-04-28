using CRM_System.BLL;
using CRM_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class OrganizationController : BaseAdminController
    {
        Mpr_OrganizationService OrganizetionService = new Mpr_OrganizationService();
        //
        // GET: /Admin/Organization/

        public ActionResult Index()
        { 
            return View();
        }

       [HttpGet]
        public string GetOrganization()
        { 
            //首先获取第一层关系.数据库内ParentID等于0的数据有且只有一个,表示公司最高位
            Mpr_Organization OneList = OrganizetionService.GetModel(s => s.ParentID == 0); 
            OrgData OneOrgList = new OrgData();
            if (OneList != null)
            {
                //加入第一层关系人
                OneOrgList.id = OneList.ID;
                OneOrgList.name = OneList.Name;
                OneOrgList.title = OneList.Title;
                OneOrgList.photo = OneList.Photo;
                OneOrgList.workcard = OneList.WorkNo;
                OneOrgList.children = GetOrgList(OneList);
            }
            return  Newtonsoft.Json.JsonConvert.SerializeObject(OneOrgList);
        }

        public List<OrgData> GetOrgList(Mpr_Organization ParentMod)
        {
            List<OrgData> Orglist = new List<OrgData>();
            //根据当前Mod获取当前最上级下所有夏季列表
            List<Mpr_Organization> dbList = OrganizetionService.FindByParam(s => s.ParentID == ParentMod.ID && s.Status == 1, s => s.Sort);
            if (dbList.Count > 0)
            {
                foreach (var item in dbList)
                {
                    OrgData OneOrgList = new OrgData();
                    //加入第一层关系人
                    OneOrgList.id = item.ID;
                    OneOrgList.name = item.Name;
                    OneOrgList.title = item.Title;
                    OneOrgList.photo = item.Photo;
                    OneOrgList.workcard = item.WorkNo;
                    OneOrgList.children = GetOrgList(item);
                    Orglist.Add(OneOrgList);
                }
            }
            return Orglist;
        }
        public class OrgData
        {
            public int id { get; set; }
            public string name { get; set; } = "";
            public string title { get; set; } = "";
            public string photo { get; set; } = "";

            public string workcard { get; set; } = "";
            public List<OrgData> children { get; set; }
        } 
        public ActionResult Edit(int  id= 0)
        {
            ViewBag.ID = id;
            Mpr_Organization Model = OrganizetionService.GetModel(s => s.ID == id);
            if (Model == null)
            {
                Model = new Mpr_Organization();
            }
            return View(Model);
        }
        [HttpPost]
        public string Save(string data)
        {
            Mpr_Organization Mod = Newtonsoft.Json.JsonConvert.DeserializeObject<Mpr_Organization>(data);
            ReturnJson ReturnJson = new ReturnJson();
            Mpr_Organization Model = OrganizetionService.GetModel(s => s.ID == Mod.ID);
            if (Model == null)
            {
                Mod.Title = Mod.Dep + "-" + Mod.Position;
                //直接添加  
                Mod.Adduser = currentadminUser.ID;
                Mod.Addtime = DateTime.Now;
                Mod.Status = 1;
                 Mod = OrganizetionService.Insert(Mod);
                if (Mod.ID != 0)
                {
                    ReturnJson.Code = "0";
                    ReturnJson.Errmsg = "保存成功";
                }
                else
                {
                    ReturnJson.Code = "1";
                    ReturnJson.Errmsg = "保存失败";
                }
            }
            else
            {
                Mod.Title = Mod.Dep + "-" + Mod.Position;
                EntityToEntity<Mpr_Organization>(Mod, ref Model);
                Model.Status = 1;
                Model = OrganizetionService.Update(Model);
                if (Model.ID != 0)
                {
                    ReturnJson.Code = "0";
                    ReturnJson.Errmsg = "保存成功";
                }
                else
                {
                    ReturnJson.Code = "1";
                    ReturnJson.Errmsg = "保存失败";
                }
            }
            return ToJson(ReturnJson);
        }
        [HttpPost]
        public string DelRow(int RowID)
        {
            ReturnJson Rejson = new ReturnJson();

            Mpr_Organization Model = OrganizetionService.GetModel(s => s.ID == RowID);
            Model.Status = 5;
            Model =  OrganizetionService.Update(Model); 
            if (Model.ID > 0)
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
    }
} 