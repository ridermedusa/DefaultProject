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
    public class MaterialTypeController : BaseAdminController
    {
        //
        // GET: /Admin/MaterialType/ 
        Mpr_Material_TypeService Material_TypeService = new Mpr_Material_TypeService(); 
        public ActionResult Index(int? page, string Keyword = "")
        {
            int currid = Convert.ToInt32(currentadminUser.ID);
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            List<Mpr_Material_Type> Pglist = Material_TypeService.GetPage(s => s.TypeName.Contains(Keyword), s => s.Sort, pageIndex, 10, ref totalCount);
            var ResultList = new StaticPagedList<Mpr_Material_Type>(Pglist, pageIndex, pageSize, totalCount);
            return View(ResultList);
        }

        public ActionResult Edit(string id)
        {
            Mpr_Material_Type Matermod = Material_TypeService.GetModel(s => s.ID == id);
            if (Matermod == null)
            {
                Matermod = new Mpr_Material_Type();
            }
            return View(Matermod);
        }

        [ValidateInput(false)]
        [HttpPost]
        public string Save(string ID, string TypeName, int Sort)
        {
            ReturnJson Result = new ReturnJson();
            try
            {
                Mpr_Material_Type TypeMod = Material_TypeService.GetModel(s => s.ID == ID);
                if (TypeMod == null)
                {
                    TypeMod = new Mpr_Material_Type();
                    TypeMod.ID = Guid.NewGuid().ToString("N");
                    TypeMod.TypeName = TypeName;
                    TypeMod.Sort = Sort;
                    TypeMod.AddTime = DateTime.Now;
                    TypeMod.AddUser = currentadminUser.ID;
                    Material_TypeService.Insert(TypeMod);
                }
                else
                {
                    TypeMod.TypeName = TypeName;
                    TypeMod.Sort = Sort;
                    TypeMod.UpdateTime = DateTime.Now;
                    TypeMod.UpdateUser = currentadminUser.ID;
                    Material_TypeService.Update(TypeMod);
                }
                Result.Code = "0";
                Result.Errmsg = "保存成功";
            }
            catch (Exception ex)
            {
                Result.Code = "1";
                Result.Errmsg = "保存异常";
            }
            return ToJson(Result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DelRow(string RowID)
        {
            ReturnJson Result = new ReturnJson();
            int DelRow = Material_TypeService.DelBy(s => s.ID == RowID);
            if (DelRow > 0)
            {
                Result.Code = "0";
                Result.Errmsg = "删除成功";
            }
            else {
                Result.Code = "1";
                Result.Errmsg = "删除失败";
            }
            return ToJson(Result);
        }
    }
}
