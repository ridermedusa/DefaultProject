using CRM_System.BLL;
using CRM_System.Model;
using CRM_System.Tools;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Base;

namespace Web.Areas.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {
        Mpr_CustomerService CustomerService = new Mpr_CustomerService();
        Mpr_OrganizationService OrganizationService = new Mpr_OrganizationService();
        // GET: /Admin/Customer/

        public ActionResult Index(int? page, string keyword = "")
        {
            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;
            List<Mpr_Customer> Pglist = CustomerService.GetPage(s => s.Name.Contains(keyword), s => s.Addtime, pageIndex, pageSize, ref totalCount);
            var personsAsIPagedList = new StaticPagedList<Mpr_Customer>(Pglist, pageIndex, pageSize, totalCount);
            ViewBag.Keyword = keyword;
            return View(personsAsIPagedList); 
        }

        public ActionResult Edit(string id = "")
        {
            ViewBag.ID = id;
            Mpr_Customer CustomerMod = CustomerService.GetModel(s => s.ID == id);
            if (CustomerMod == null) 
            {
                CustomerMod = new Mpr_Customer();
            }
            return View(CustomerMod);
        }

        public string getInv(string q ,int limit)
        {
            List<Mpr_Organization> OrgList = OrganizationService.GetPage(s => s.Name.Contains(q) || s.WorkNo.Contains(q), s => s.WorkNo, 1, limit);
            return ToJson(OrgList);
        } 
        [HttpPost]
        public string Save(string data)
        {
            ReturnJson Result = new ReturnJson();
            try
            {
    
                Mpr_Customer CustomerMod = Newtonsoft.Json.JsonConvert.DeserializeObject<Mpr_Customer>(data);
                #region  开始逐条验证
                if (string.IsNullOrEmpty(CustomerMod.ID_Card) || (CustomerMod.ID_Card.Length != 15 && CustomerMod.ID_Card.Length != 18))
                {
                    Result.Code = "1";
                    Result.Errmsg = "身份证号输入不正确";
                    return ToJson(Result);
                }
                if (string.IsNullOrEmpty(CustomerMod.Name))
                {
                    Result.Code = "1";
                    Result.Errmsg = "姓名不能为空";
                    return ToJson(Result);
                }
                if (string.IsNullOrEmpty(CustomerMod.Phone) || !Tools.IsMobilePhone(CustomerMod.Phone))
                {
                    Result.Code = "1";
                    Result.Errmsg = "电话号码输入不正确";
                    return ToJson(Result);
                }
                if (CustomerMod.Province == "省份")
                {
                    Result.Code = "1";
                    Result.Errmsg = "省份不能为空";
                    return ToJson(Result);
                }
                if (CustomerMod.Province == "地级市")
                {
                    Result.Code = "1";
                    Result.Errmsg = "地级市不能为空";
                    return ToJson(Result);
                }
                if (CustomerMod.Province == "市、县级市")
                {
                    Result.Code = "1";
                    Result.Errmsg = "市、县级市不能为空";
                    return ToJson(Result);
                }
                if (string.IsNullOrEmpty(CustomerMod.Address))
                {
                    Result.Code = "1";
                    Result.Errmsg = "详细地址不能为空";
                    return ToJson(Result);
                }
                if (string.IsNullOrEmpty(CustomerMod.Sale))
                {
                    Result.Code = "1";
                    Result.Errmsg = "销售人员不能为空";
                    return ToJson(Result);
                }
                //检测销售人员是否存在
                Mpr_Organization OrgModel = OrganizationService.GetModel(s => s.ID == CustomerMod.SaleID);
                if (OrgModel == null)
                {
                    Result.Code = "1";
                    Result.Errmsg = "销售人员不存在";
                    return ToJson(Result);
                }
                #endregion
                #region 验证通过开始处理
                int Sex = -1;
                string birthday = "";
                Tools.GetBirthdayAndSex(CustomerMod.ID_Card, ref Sex,ref birthday);
                CustomerMod.Sex = Sex;
                CustomerMod.Birthday = birthday;
                //赋值部门以及上级信息. 
                CustomerMod.SaleDep = OrgModel.Dep;
                List<Mpr_Organization> Parlist = GetParentsList(OrgModel);
                Mpr_Organization ModDir = Parlist.Where(s => s.Position == "主管").FirstOrDefault();
                if (ModDir != null)
                {
                    CustomerMod.SaleDirectorID = ModDir.ID;
                    CustomerMod.SaleDirector = ModDir.Name;
                }
                Mpr_Organization ModManager = Parlist.Where(s => s.Position == "经理").FirstOrDefault();
                if (ModManager != null)
                {
                    CustomerMod.SaleManagerID = ModManager.ID;
                    CustomerMod.SaleManager = ModManager.Name;
                }
                Mpr_Organization ModChief = Parlist.Where(s => s.Position == "总监").FirstOrDefault();
                if (ModChief != null)
                {
                    CustomerMod.SaleChiefID = ModChief.ID;
                    CustomerMod.SaleChief = ModChief.Name;
                }
                #endregion
                Mpr_Customer OldMod =  CustomerService.GetModel(s => s.ID == CustomerMod.ID);
                if (OldMod == null)
                {
                    //当前没有搜到对应数据，直接开始添加 
                    CustomerMod.ID = Guid.NewGuid().ToString("N"); 
                    CustomerMod.AddUser = currentadminUser.ID;
                    CustomerMod.Addtime = DateTime.Now;
                    //开始添加数据
                    CustomerMod = CustomerService.Insert(CustomerMod);
                }
                else {
                    //当前存在对应数据
                    EntityToEntity(CustomerMod, ref OldMod, new string[] { "AddUser", "Addtime" });
                    OldMod.ChangeUser = currentadminUser.ID;
                    OldMod.Changetime = DateTime.Now;
                    OldMod = CustomerService.Update(OldMod);
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
        string[] Position = new string[] { "主管", "经理", "总监" };
        /// <summary>
        ///  获取所有上级列表
        /// </summary>
        /// <returns></returns>
        public List<Mpr_Organization> GetParentsList(Mpr_Organization KidMod)
        {
            if (KidMod.ParentID != null && KidMod.ParentID != 0)
            {
                //向上递归获取所有上级线
                List<Mpr_Organization> ParentList = new List<Mpr_Organization>();
                ParentList.Add(KidMod);
                while (KidMod.ParentID != 0)
                {
                    KidMod = OrganizationService.GetModel(s => s.ID == KidMod.ParentID);
                    if (KidMod != null)
                    {
                        if (Position.Contains(KidMod.Position))
                        {
                            //判断是否已经存在对应职位数据 如果存在则不再添加
                            if (ParentList.Where(s => s.Position == KidMod.Position).Count() == 0)
                            {
                                ParentList.Add(KidMod);
                            }
                        }
                        else {
                            ParentList.Add(KidMod);
                        } 
                    }
                    else
                    {
                        //出现上级为空，直接终止循环
                        break;
                    }
                } 
                return ParentList;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DelRow(string RowID)
        {
            ReturnJson Result = new ReturnJson();
            int DelRow = CustomerService.DelBy(s => s.ID == RowID);
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