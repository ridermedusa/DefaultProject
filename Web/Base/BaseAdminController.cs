using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_System.BLL;
using System.Data;
using CRM_System.Model;
using System.ComponentModel;
using Aspose.Cells;
using CRM_System.Tools;
using Newtonsoft.Json;
using PagedList;
using PlainElastic.Net.Queries;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;

namespace Web.Base
{
    public class BaseAdminController : Controller
    {
        #region 定义公司ID数据 
        int ComID = 1;
        #endregion
        #region 自定义变量   
        Jpush JpushService = new Jpush();

        #endregion
        #region 所有action 调用 身份验证
        /// <summary>
        /// 不需要需要登录的action等验证
        /// </summary>
        public Dictionary<string, List<string>> nologinaction
        {
            get
            {
                if (Session["_admin_login_action"] == null)
                {
                    Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
                    temp["login"] = new List<string>();
                    temp["common"] = new List<string>();
                    Session["_admin_login_action"] = temp;
                }
                return Session["_admin_login_action"] as Dictionary<string, List<string>>;
            }
        }
        //所有action 调用 身份验证
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string Area = RouteData.DataTokens["area"].ToString();
            string controller = RouteData.Values["controller"].ToString().ToLower();
            string action = RouteData.Values["action"].ToString().ToLower();
            bool blnNoRight = false;
            string agent = filterContext.HttpContext.Request.UserAgent.ToLower();
            int isWeixin = agent.IndexOf("micromessenger");
            //如果当前没在微信内，又要进来，那就先登录
            //if (isWeixin == -1 && Area == "WeChat")
            //{
            //    filterContext.Result = new RedirectResult("/Admin");
            //    return;
            //}
            if (!nologinaction.Keys.Contains(controller))
            {
                if (Area == "Admin")
                {
                    if (currentadminUser == null)
                    {
                        blnNoRight = true;
                    }
                }
                if (controller == "login")
                    blnNoRight = false;
            }
            if (controller == "app")
            {
                blnNoRight = false;
            }
            ViewBag.currentadminUser = currentadminUser;
            //无权限跳转
            if (blnNoRight && Area == "Admin")
            {
                //Response.Write(string.Format("<script>window.top.location.href='/Login?returnurl={0}';</script>", Server.UrlEncode(Request.Url.ToString())));
                //Response.Redirect(string.Format("/Login?returnurl={0}", Server.UrlEncode(Request.Url.ToString())));
                //Response.End();
                filterContext.Result = new RedirectResult("/Admin/Login");
                return;
            }

        }
        #endregion
        #region 用户信息操作 
        //注销用户信息
        public void SafeExit()
        {
            //清除cookies(用户信息)
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["lc_cookie"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(-1);
                Response.Cookies.Set(cookie);
            }
            //释放 session
            Session.Abandon();
        }
        public  string Getscroll_id
        {
            get
            {
                //string res = FormsAuthentication.FormsCookieName;
                if (Session["scroll_id"] == null)
                {
                    return "";
                }
                return Session["scroll_id"].ToString();
            }
            set
            { 
                Session["scroll_id"] = value;
            }
        }
        public string LastSerchKey
        {
            get
            {
                //string res = FormsAuthentication.FormsCookieName;
                if (Session["LastSerchKey"] == null)
                {
                    return "";
                }
                return Session["LastSerchKey"].ToString();
            }
            set
            {
                Session["LastSerchKey"] = value;
            }
        }


        /// <summary>
        /// 用于前后台用户登录的会话
        /// </summary>
        public Sys_Admin currentadminUser
        {
            get
            {
                //string res = FormsAuthentication.FormsCookieName;
                if (Session["currentadminUser"] == null)
                {
                    string id = CRM_System.Common.Cookie.GetCookie("admin_id");
                    if (string.IsNullOrEmpty(id))
                    {
                        return null;
                    }
                    int adminid = Convert.ToInt32(id);
                    Session["currentadminUser"] = new Sys_AdminService().Find(adminid);//.GetModel(x => x.ID == Convert.ToInt32(id));
                    if (Session["currentadminUser"] == null)
                    {
                        return null;
                    }
                }
                Sys_Admin tempadmin = Session["currentadminUser"] as Sys_Admin;
                return (Sys_Admin)Session["currentadminUser"];
            }
            set
            {
                CRM_System.Common.Cookie.SaveCookie("admin_id", value.ID.ToString());
                Session["currentadminUser"] = value;
            }
        }
         
        public bool IsLoginIn(DateTime? LastEveryDayTime)
        {
            if (LastEveryDayTime == null)
            {
                //当前第一次登录，可以算作当日登陆
                return true;
            }
            else
            {
                DateTime Dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                if (Dt > LastEveryDayTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
        #region 自定义方法汇总
        public string ReturnMsg(string code, string errmsg)
        {
            return "{\"status\":\"" + code + "\",\"errmsg\":\"" + errmsg + "\"}";
        }
        public void ReturnR(string msg, string jumppage)
        {
            Response.Write("<script>alert('" + msg + "');location.href='" + jumppage + "';</script>");
        }
        /// <summary>
        ///  处理返回信息
        /// </summary>
        /// <param name="ToJsonData"></param>
        /// <returns></returns>
        public string ToJson(object ToJsonData)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ToJsonData);
        }
        #region 自定义返回类
        /// <summary>
        /// 基础类
        /// </summary>
        public class ReturnJson
        {
            /// <summary>
            /// 返回错误码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 返回错误信息
            /// </summary>
            public string Errmsg { get; set; }
        }
        public class ReturnObjJson : ReturnJson
        {
            public object Data { get; set; }
        }
        /// <summary>
        ///  返回HTML时
        /// </summary>
        public class ReturnHtml : ReturnJson
        {
            public string HtmlInfo { get; set; }
        }
        public class ReturnMod<T> : ReturnJson
        {
            public T Mod { get; set; }
        }
        /// <summary>
        /// 返回基础信息
        /// </summary>
        public class ReturnObject : ReturnJson
        {
            public int PageCount { get; set; }
            public int ItemCount { get; set; }
            public object GetModel { get; set; }
            public object GetInList { get; set; }
            public string Model { get; set; }
        }
        /// <summary>
        /// 带链接的基础信息
        /// </summary>
        public class ReturnObjectByLink : ReturnJson
        {
            public object GetModel { get; set; }

            public string GetLink { get; set; }
        }
        /// <summary>
        /// 自定义错误日志
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="value"></param>
        public void AddMyErrorLog(string Type, string value)
        {
            Sys_Error_log ErrorLog = new Sys_Error_log();
            ErrorLog.ErrType = Type;
            ErrorLog.ErrMsg = value;
            ErrorLog.Date = DateTime.Now;
            new Sys_Error_logService().Insert(ErrorLog);
        }
        /// <summary>
        ///  获取分页数量
        /// </summary>
        /// <param name="ItemCount"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public int GetPageCount(int ItemCount, int PageNum)
        {
            return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(ItemCount) / PageNum));
        }
        #endregion

        #endregion
        #region 获取左侧菜单信息

        Sys_RoleService RoleService = new Sys_RoleService();
        Mpr_Admin_MenuService MenuService = new Mpr_Admin_MenuService();
        ComSQLService SQLSerivice = new ComSQLService();
        public string GetLeftMenuInfo()
        {
            string html = "";
            //根据当前用户权限等级获取对应菜单
            if (currentadminUser != null)
            {
                //int RoleId = Convert.ToInt32(currentadminUser.SopRoleId);
                //Sys_Role RoleMod = RoleService.GetModel(s => s.ID == RoleId);
                GetRoleClass GRC = GetRoleValueAndButton(currentadminUser.RoleID.ToString());
                if (GRC != null)
                {
                    //当前用户权限真实存在
                    string RoleStr = GRC.ROLEVALUE;
                    //去掉字符串生成时可能出现的多余逗号
                    string Rval = "";
                    string[] RoleArr = RoleStr.Split(',');
                    foreach (string Str in RoleArr)
                    {
                        if (!string.IsNullOrEmpty(Str))
                        {
                            Rval += Str + ",";
                        }
                    }
                    //获取当前权限下最上级菜单
                    string SQL = "select * from Mpr_Admin_Menu where id in (" + Rval.TrimEnd(',') + ") order by Sort asc";
                    DataTable DT = SQLSerivice.Query(SQL).Tables[0];
                    foreach (DataRow Dr in DT.Select(" RightParent = 0"))
                    {
                        html += GetNextMenu(Dr, DT);
                    }
                }
            }
            return html;
        }

        /// <summary>
        /// 检测目标是否存在下级 如果存在 额继续执行该方法 如果不存在则返回当前项目
        /// </summary>
        /// <returns></returns>
        public string GetNextMenu(DataRow NowDr, DataTable Dt,int Isdg = 0)
        {
            string html = "";
            if (Isdg == 1)
            {
                //当前只可能有两级，所以直接返回 
                html += " <dd  name=\"leftdd_" + NowDr["ID"] + "\" style=\"cursor: pointer; \"><a href='" + NowDr["RightUrl"] + "' target='main' id='" + NowDr["RightName"] + "' title='" + NowDr["RightName"] + "' name=\"clickitem\" style=\"cursor: pointer; \"> " + NowDr["RightName"] + "</a></dd>";
            }
            else {
                if (Dt.Select(" RightParent = " + NowDr["ID"]).Count() > 0)
                {
                     html += "    <dl  clt=\"" + NowDr["ID"] + "\"><dt title='" + NowDr["RightName"] + "'> " + NowDr["RightName"] + " <img src=\"/Content/CRM_System/images/-.png\" /></ dt > ";
                     foreach (DataRow Dr in Dt.Select(" RightParent = " + NowDr["ID"]))
                    {
                        //表示当前入参存在上下级

                        //获取当前条件下的下级菜单
                        html += GetNextMenu(Dr, Dt, 1);
                    }
                    html += " </dl> "; 
                }
                else
                {
                    //表示当前已经不存在下级了,直接返回当前ID基础信息
                    //html += "<li name=\"leftdd_" + NowDr["ID"] + "\"><a href='" + NowDr["RightUrl"] + "' target='main' id='" + NowDr["Ename"] + "' title='" + NowDr["RightName"] + "'><i class=\"fa fa-angle-double-right\"></i> " + NowDr["Ename"] + "</a></li>";
                    html += "<dl name=\"leftdd_" + NowDr["ID"] + "\" style=\"cursor: pointer; \"><dt style=\"cursor: pointer; \"><a href='" + NowDr["RightUrl"] + "' target='main' id='" + NowDr["RightName"] + "' title='" + NowDr["RightName"] + "' name=\"clickitem\"> " + NowDr["RightName"] + "</a> <img src =\"/Content/CRM_System/images/-.png\" /></dt></dl> ";
                }
            } 
            return html;
        }
        #endregion
        #region 日志类信息操作
        /// <summary>
        ///填写一条报错数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Context"></param>
        public static void AddErrorlog(string Type, string Context)
        {
            Sys_Error_logService LogService = new Sys_Error_logService();
            Sys_Error_log ErrLog = new Sys_Error_log();
            ErrLog.ErrType = Type;
            ErrLog.ErrMsg = Context;
            ErrLog.Date = DateTime.Now;
            ErrLog.Ip = Tools.IPAddress;
            LogService.Insert(ErrLog);
        }
        /// <summary>
        /// 填写一条报错数据
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Context"></param>
        public static void AddInterfaceLog(string InterfaceName, string Params)
        {
            Mpr_InterfaceCall_LogService LogService = new Mpr_InterfaceCall_LogService();
            Mpr_InterfaceCall_Log ErrLog = new Mpr_InterfaceCall_Log();
            ErrLog.Ip = Tools.IPAddress;
            ErrLog.Addtime = DateTime.Now;
            ErrLog.InterfaceName = InterfaceName;
            ErrLog.Params = Params;
            LogService.Insert(ErrLog);
        }
        #endregion

        public static bool IsEquals<T>(T obj1, T obj2)
        {
            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();

            System.Reflection.PropertyInfo[] properties1 = type1.GetProperties();
            System.Reflection.PropertyInfo[] properties2 = type2.GetProperties();

            bool IsMatch = true;
            for (int i = 0; i < properties1.Length; i++)
            {
                string s = properties1[i].DeclaringType.Name;
                if (properties1[i].GetValue(obj1, null).ToString() != properties2[i].GetValue(obj2, null).ToString())
                {
                    IsMatch = false;
                    break;
                }
            }

            return IsMatch;
        }

        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntity<T>(T newmod, ref T oldmod, string[] ExcludeList)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (ExcludeList.Contains(mItem.Name))
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值
                        if (newitem != null)
                        {
                            //判断新得到的值是否为NULL
                            if (newitem != olditem)
                            {
                                //判断数值是否改变  如果改变则直接将新值赋到上面
                                mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntity<T>(T newmod, ref T oldmod)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (mItem.Name == "Addtime" || mItem.Name == "Adduser" || mItem.Name == "ID" || mItem.Name == "AdduserName")
                    {
                        continue;
                    }
                    else if (mItem.Name == "Restoretime" || mItem.Name == "Backtime")
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值
                        if (newitem != null)
                        {
                            //判断新得到的值是否为NULL
                            if (newitem != olditem)
                            {
                                //判断数值是否改变  如果改变则直接将新值赋到上面
                                mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTargetObjSrc"></param>
        /// <param name="pTargetObjDest"></param>
        public void EntityToEntityUpdateNull<T>(T newmod, ref T oldmod)
        {
            try
            {
                foreach (var mItem in typeof(T).GetProperties())
                {
                    if (mItem.Name == "Addtime" || mItem.Name == "Adduser" || mItem.Name == "ID" || mItem.Name == "FirstTime")
                    {
                        continue;
                    }
                    else if (mItem.Name == "Changetime" || mItem.Name == "Restoretime" || mItem.Name == "Backtime")
                    {
                        continue;
                    }
                    else
                    {
                        object olditem = mItem.GetValue(oldmod, new object[] { });
                        object newitem = mItem.GetValue(newmod, new object[] { });
                        //判断是否赋值

                        //判断新得到的值是否为NULL
                        if (newitem != olditem)
                        {
                            //判断数值是否改变  如果改变则直接将新值赋到上面
                            mItem.SetValue(oldmod, mItem.GetValue(newmod, new object[] { }), null);
                        }
                    }
                }
            }
            catch (NullReferenceException NullEx)
            {
                throw NullEx;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #region 可能的公共调用方法
        Sys_RoleService RoleBLL = new Sys_RoleService();
        public List<Sys_Role> GetRoleList()
        {
            List<Sys_Role> RoleList = RoleBLL.FindAll();
            return RoleList;
        }
        #endregion
        #region 入参页面ID 返回当前页面当前登录用户权限
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GetRoleClass GetRoleValueAndButton(string SopRoleId)
        {
            //获取权限列表,返回按钮权限以菜单权限
            if (!string.IsNullOrEmpty(SopRoleId))
            {
                GetRoleClass RoleValue = new GetRoleClass();
                try
                {
                    string[] Spr = SopRoleId.Split(',');
                    foreach (string Str in Spr)
                    {
                        //获取ID 
                        Sys_Role RoleMod = RoleServivce.GetModel(s => s.ID == Str);
                        if (RoleMod != null)
                        {
                            RoleValue.ROLEVALUE += RoleMod.ROLEVALUE + ",";
                            RoleValue.ButtonRole += RoleMod.ButtonRole + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(RoleValue.ROLEVALUE))
                    {
                        RoleValue.ROLEVALUE = RoleValue.ROLEVALUE.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(RoleValue.ButtonRole))
                    {
                        RoleValue.ButtonRole = RoleValue.ButtonRole.TrimEnd(',');
                    }
                    return RoleValue;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public class GetRoleClass
        {
            public string ROLEVALUE { get; set; }
            public string ButtonRole { get; set; }
        }
        Sys_RoleService RoleServivce = new Sys_RoleService();
        Mpr_Admin_ButtonRoleService ButtonRoleService = new Mpr_Admin_ButtonRoleService();
        /// <summary>
        /// 获取 当前用户对应权限数据
        /// </summary>
        /// <returns></returns>
        public List<Mpr_Admin_ButtonRole> ButtonRoleList(int pageID)
        {

            if (currentadminUser != null)
            {
                //首先通过入参的参数获取对应数据信息
                List<Mpr_Admin_ButtonRole> ButtonRoleList = ButtonRoleService.FindByParam(s => s.PageID == pageID);
                if (ButtonRoleList.Count > 0)
                {
                    //获取对应按钮权限字符串
                    //Sys_Role RoleMod = RoleServivce.GetModel(s => s.ID == currentadminUser.SopRoleId);
                    GetRoleClass GRC = GetRoleValueAndButton(currentadminUser.RoleID.ToString());
                    if (GRC != null)
                    {

                        string ButtonRole = GRC.ButtonRole;
                        if (!string.IsNullOrEmpty(ButtonRole))
                        {
                            List<Mpr_Admin_ButtonRole> ResultList = new List<Mpr_Admin_ButtonRole>();
                            //通过字符串获取对应数据列表
                            List<string> StrList = ButtonRole.Split(',').ToList();
                            foreach (string str in StrList)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    int Roleinfoid = Convert.ToInt32(str);
                                    //筛选出权限数据进行返回
                                    Mpr_Admin_ButtonRole ButtonRoleId = ButtonRoleList.Where(s => s.ID == Roleinfoid).FirstOrDefault();
                                    if (ButtonRoleId != null)
                                    {
                                        ResultList.Add(ButtonRoleId);
                                    }
                                }
                            }
                            return ResultList;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //还是没权限
                        return null;
                    }
                }
                else
                {
                    //当前连按钮都没做 有个P的用户权限
                    return null;
                }
            }
            else
            {
                //如果这里为空 表示表示什么权限都没有，此处未登录默认无任何页面按钮权限
                return null;
            }
        }
        #endregion
        #region 视图页方法
        public string GetRoleCollection(string RoleName)
        {
            string ReturnStr = "";
            if (!string.IsNullOrEmpty(RoleName))
            {
                string[] StrP = RoleName.Split(',');
                foreach (string str in StrP)
                {
                    
                    try
                    {
                        ReturnStr += new Sys_RoleService().GetModel(s => s.ID == str).ROLENAME + ",";
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return ReturnStr.TrimEnd(',');
            }
            else
            {
                return "未设置";
            }
        }
        #endregion
        #region 操作完成，返回结果页面提示，并关闭layer页面，刷新列表页

        /// <summary>
        /// 操作完成，返回结果页面提示，并关闭layer页面，刷新列表页
        /// </summary>
        /// <param name="returnValue">页面提示信息</param>
        /// <param name="type">是否关闭layer页（0:不关闭  1:关闭）</param>
        public void ReturnAlert(string returnValue, int type,string url ="")
        {

            if (type == 0)
            {
                Response.Write("<script>alert('" + returnValue + "');</script>");
            }
            else if (type == 1)
            {
                Response.Write("<script>alert('" + returnValue + "');var index = parent.layer.getFrameIndex(window.name);parent.main.location.href = parent.main.location.href;parent.layer.close(index);</script>");

            }
            else if (type == 2)
            {
                Response.Write("<script>alert('" + returnValue + "');location.href='"+ url + "'</script>");

            }


        }
        #endregion
        
        #region  SignalR Send
        public void SignalRSend(string ConnectionId, string value)
        {
            SignalRChat.ChatHub.ChatTicker.Instance.GlobalContext.Clients.Client(ConnectionId).SendMessage(value); //.SendData(ConnectionId, DateTime.Now.ToString());
        }
        #endregion
        #region Elasticsearch
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Model"></param>
        /// <param name="ID"></param>
        /// <param name="IndexName"></param>
        /// <param name="IndexType"></param>
        public void CreateDataIndex<T>(T Model,string ID,string IndexName,string IndexType)
        {
            var result = Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Index(IndexName, IndexType, ID, Model);
        }
        public void DeleteDataIndex(string ID, string IndexName, string IndexType)
        {
            Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Delete(IndexName, IndexType, ID);
        }
        public List<Mpr_Material> GetMaterial(string key, string TypeID, int page = 0)
        {
            List<string> A = Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.GetIKTokenFromStr(key);
            #region 定义查询内容
            var query = new QueryBuilder<Mpr_Material>()
                .Query(b =>
                            b.Bool(m =>
                                m.Must(t =>
                                          t.Term(r => r.Field("TypeID").Value(TypeID))
                                          .Bool(ms =>
                                                    ms.Should(ts =>
                                                             ts.Bool(m1 =>
                                                                         m1.Must(
                                                                                q=>q.QueryString(t1 => t1.Fields(obj => obj.Title, obj => obj.Context).Query(key))
                                                                         )
                                                                    )
                                                           ) 

                                                  )
                                         )
                             ) 
                        )
                //查询，多字段匹配（Title和Content匹配key中的任意一个单词）
                //.Query(x => 
                //x.MultiMatch(t => t.Fields(obj => obj.Title, obj => obj.Context).Analyzer(key))
                //      x.QueryString(t => t.Fields(obj => obj.Title, obj => obj.Context).Query(key))
                //)
                //过滤，数据为启用状态并且大小在40~150之间
                //.Filter(x =>
                //x.And(w =>
                // w.Term(t => t
                //     .Field(obj => obj.TypeID).Value(TypeID)
                // )
                //        .Range(t => t
                //            .Field(obj => obj.Size).From("40").To("150")
                //        )
                //)
                //) 
                //排序，按照时间倒序排列
                //.Sort(x => x.Field("AddTime", SortDirection.desc))
                //高亮，定义高亮样式及字段
                .Highlight(h => h
                    .PreTags("<b>")
                    .PostTags("</b>")
                    .Fields(
                        f => f.FieldName("Title").Order(HighlightOrder.score),
                        f => f.FieldName("Context").Order(HighlightOrder.score),
                        f => f.FieldName("_all")
                    )
                );
            #endregion
            //var result = Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Search<Mpr_Material>("db_Material", "Mpr_Material", query, page, 100);
            string scroll_id = "";
            //如果上次搜索结果和当次一样,并且当前页不是第一页，表示需要入参Scroll
            if (LastSerchKey == key && page == 0)
            {
                scroll_id = Getscroll_id;
            }
            else
            {
                //如果上次搜索结果和当次不一样，表示结果有变化，则置空
                LastSerchKey = key;
                scroll_id = "";
            }
            var result = Amy.Toolkit.PlainElastic.ElasticSearchHelper.Intance.Search<Mpr_Material>("db_MaterialTure", "Mpr_MaterialTure", query, ref scroll_id);
            Getscroll_id = scroll_id;
            #region 拼装查询内容
            var list = result.hits.hits.Select(c => new Mpr_Material()
            {
                ID = c._source.ID,

                Title = c.highlight == null ? c._source.Title : c.highlight.Keys.Contains("Title") ? string.Join("", c.highlight["Title"]) : c._source.Title, //高亮显示的内容，一条记录中出现了几次
                Context = c.highlight == null ? c._source.Context : c.highlight.Keys.Contains("Context") ? string.Join("", c.highlight["Context"]) : c._source.Context, //高亮显示的内容，一条记录中出现了几次
                AddTime = c._source.AddTime,
                AddUser = c._source.AddUser,
                UpdateTime = c._source.UpdateTime,
                UpdateUser = c._source.UpdateUser,
            }).ToList();
            int O = result.hits.total;
            #endregion
            return list;
        }

        #endregion
        #region  创建用户轨迹日志
        /// <summary>
        /// 
        /// </summary>
        public void CreateCustomer(string CustomerID, CusomerLogType Type,string Context)
        {
            Mpr_Customer_Log Log = new Mpr_Customer_Log();
            Log.ID = Guid.NewGuid().ToString("N");
            Log.CustomerID = CustomerID;
            Log.Type = (int)Type;
            Log.Context = Context;
            Log.Adduser = currentadminUser.ID;
            Log.Addtime = DateTime.Now;

        }
        #endregion
    }
}
 