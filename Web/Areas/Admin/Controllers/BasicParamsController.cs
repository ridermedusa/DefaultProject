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
    public class BasicParamsController : BaseAdminController
    {

        // GET: /Admin/BasicParams/
        Mpr_Basic_ParamsService  BasicParamsService = new Mpr_Basic_ParamsService();
        public ActionResult Index(int? page, string Keyword = "",int BasicType = -1)
        {
            int currid = Convert.ToInt32(currentadminUser.ID);
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            if (BasicType == -1)
            {
                List<Mpr_Basic_Params> Pglist = BasicParamsService.GetPage(s => s.ParamsName.Contains(Keyword), s => s.Addtime, pageIndex, 10,ref totalCount);
                var ResultList = new StaticPagedList<Mpr_Basic_Params>(Pglist, pageIndex, pageSize, totalCount);
                return View(ResultList);
            }
            else
            {
                List<Mpr_Basic_Params> Pglist = BasicParamsService.GetPage(s => s.ParamsName.Contains(Keyword) && s.BasicType == BasicType, s => s.Addtime, pageIndex, 10, ref totalCount);
                var ResultList = new StaticPagedList<Mpr_Basic_Params>(Pglist,pageIndex , pageSize, totalCount);
                return View(ResultList);
            }
        }

        public ActionResult Edit(string id)
        {
            Mpr_Basic_Params ParamsMod = BasicParamsService.GetModel(s => s.ID == id);
            if (ParamsMod == null)
            {
                ParamsMod = new Mpr_Basic_Params();
            }
            return View(ParamsMod);
        }


        [ValidateInput(false)]
        [HttpPost]
        public string Save(string ID, int BasicType, string ParamsName)
        {
            ReturnJson Result = new ReturnJson();
            try
            {
                Mpr_Basic_Params TypeMod = BasicParamsService.GetModel(s => s.ID == ID);
                if (TypeMod == null)
                {
                    TypeMod = new Mpr_Basic_Params();
                    TypeMod.ID = Guid.NewGuid().ToString("N");
                    TypeMod.BasicType = BasicType;
                    TypeMod.ParamsName = ParamsName;
                    TypeMod.Addtime = DateTime.Now;
                    TypeMod.Adduser = currentadminUser.ID;
                    BasicParamsService.Insert(TypeMod);
                }
                else
                {
                    TypeMod.BasicType = BasicType;
                    TypeMod.ParamsName = ParamsName;
                    BasicParamsService.Update(TypeMod);
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
            int DelRow = BasicParamsService.DelBy(s => s.ID == RowID);
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
    }
}
