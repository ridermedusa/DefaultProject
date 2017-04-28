using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Admin.Models
{
    public class Admin_User
    {
        public int ID { get; set; }
        public string UName { get; set; }
        public string TrueName { get; set; }
        public string LoginPwd { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string RoleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public Nullable<int> IsLock { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> Addtime { get; set; }
        public Nullable<int> AdminType { get; set; }
        public int TypeID { get; set; }
        public string NickName { get; set; }
        public string Linkman { get; set; }
        public string PeopleCard { get; set; }
        public int Sex { get; set; }
        public Nullable<int> Year { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Signature { get; set; }
        public string Customer { get; set; }
        public string Postal { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string RegIP { get; set; }
        public int Status { get; set; }
        public int ErrorCount { get; set; }
        public int IsPhoneProving { get; set; }
        public int IsEmailProving { get; set; }
        public int IsTrueNameProving { get; set; }
        public Nullable<int> LV { get; set; }
        public Nullable<int> LoginStatus { get; set; }
        public Nullable<System.DateTime> LastCheckTime { get; set; }
        public Nullable<int> InviterCount { get; set; }
        public Nullable<int> FirstRegType { get; set; }
        public string QQ_Token { get; set; }
        public Nullable<System.DateTime> QQ_Token_LastTime { get; set; }
        public string WeChat_Token { get; set; }
        public Nullable<System.DateTime> WeChat_Token_LastTime { get; set; }
        public int FunsNum { get; set; }
        public string CertificateNum { get; set; }
        public decimal SharesYear { get; set; }
        public int FollowNum { get; set; }
        public int IsHot { get; set; }
        public int SubscribeNum { get; set; }
        public int MyselfChoiceNum { get; set; }
        public string PersonalData { get; set; }


    }
}