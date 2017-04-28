using CRM_System.BLL;
using CRM_System.Model;
using CRM_System.Tools;
using PagedList;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class MaterialController : BaseAdminController
    {
        //
        // GET: /Admin/Material/
        Mpr_MaterialService MaterialService = new Mpr_MaterialService();
        public ActionResult Index(int? page, string keyword = "")
        { 
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            List<Mpr_Material> Pglist = MaterialService.GetPage(s => s.Title.Contains(keyword), s => s.AddTime, pageIndex, pageSize, ref totalCount);
            var personsAsIPagedList = new StaticPagedList<Mpr_Material>(Pglist, pageIndex, pageSize, totalCount);
            ViewBag.Keyword = keyword;
            return View(personsAsIPagedList);
        }
        [HttpPost]
        public string DelRow(string RowID)
        {
            ReturnJson Result = new ReturnJson();
            int DelRow = MaterialService.DelBy(s => s.ID == RowID);
            DeleteDataIndex(RowID, "db_MaterialTure", "Mpr_MaterialTure");
            if (DelRow > 0)
            {
                Result.Code = "0";
                Result.Errmsg = "删除成功";
            }
            else
            {
                Result.Code = "1";
                Result.Errmsg = "删除失败";
            }
            return ToJson(Result);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Mpr_Material MaterialMod = MaterialService.GetModel(s => s.ID == id);
            if (MaterialMod == null)
            {
                MaterialMod = new Mpr_Material();
            }

            return View(MaterialMod);
        }

        public ActionResult Keyword(string id)
        {
            List<string> K = KeySpirt.SpirtKeyword(id);
            return View(K);
        }
        public ActionResult CreateIndex(string Name)
        {
         
            return View();
        }
    
        /// <summary>
        /// 搜索数据
        /// </summary>
        public ActionResult Search(string key,string TypeID, int page = 0)
        {
            DateTime DT1 = DateTime.Now;
            List<Mpr_Material> List = GetMaterial(key, TypeID, page);
            DateTime DT2 = DateTime.Now;
            double Db = (DT2 - DT1).TotalMilliseconds;
            ViewBag.DB = Db;
            return View(List);
        }
 

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Mpr_Material Mod)
        {
            ReturnJson Rejson = new ReturnJson();

            //修改
            //尝试通过ID获取数据库内已有信息
            Mpr_Material DBmod = MaterialService.GetModel(s => s.ID == Mod.ID);
            if (DBmod != null)
            {
                //EntityToEntity(Mod, ref DBmod);
                DBmod.TypeID = Mod.TypeID;
                Mpr_Material_Type TypeMod = new Mpr_Material_TypeService().GetModel(s => s.ID == DBmod.TypeID);
                if(TypeMod != null)
                {
                    DBmod.TypeName = TypeMod.TypeName;
                } 
                DBmod.Title = Mod.Title;  
                DBmod.Des = Mod.Des;
                DBmod.Context = Mod.Context;
                DBmod.UpdateTime = DateTime.Now;
                DBmod.UpdateUser = currentadminUser.ID;
                DBmod = MaterialService.Update(DBmod);
                CreateDataIndex<Mpr_Material>(DBmod, DBmod.ID, "db_MaterialTure", "Mpr_MaterialTure");
                if (DBmod != null)
                {
                    ReturnAlert("保存成功！", 1);
                }
                else
                {
                    ReturnAlert("保存失败！", 0);
                }
            }
            else
            {
                Mpr_Material DBmodArticle = new Mpr_Material();
                DBmodArticle.ID = Guid.NewGuid().ToString("N");
                DBmodArticle.TypeID = Mod.TypeID;
                Mpr_Material_Type TypeMod = new Mpr_Material_TypeService().GetModel(s => s.ID == DBmodArticle.TypeID);
                if (TypeMod != null)
                {
                    DBmodArticle.TypeName = TypeMod.TypeName;
                } 
                DBmodArticle.Title = Mod.Title;
                DBmodArticle.Des = Mod.Des;
                DBmodArticle.Context = Mod.Context;
                DBmodArticle.AddTime = DateTime.Now;
                DBmodArticle.AddUser = currentadminUser.ID;
                DBmodArticle = MaterialService.Insert(DBmodArticle);
                CreateDataIndex<Mpr_Material>(DBmodArticle, DBmodArticle.ID, "db_MaterialTure", "Mpr_MaterialTure");
                if (DBmodArticle != null)
                {
                    ReturnAlert("保存成功！", 1);
                }
                else
                {
                    ReturnAlert("保存失败！", 0);
                }
            }
            return View(Mod);
        }



    }
}
