using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.Model
{
    /// <summary>
    /// 导入联系人方法
    /// </summary>
    public class CreateLink
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNum { get; set; }
        //备注信息 
        public string Bz { get; set; }

    }

    /// <summary>
    /// 群发朋友圈类
    /// </summary>
    public class CircleOfFriendsModel
    {

        public string SendText { get; set; }

        public List<string> SendPic { get; set; }

    }
    /// <summary>
    /// 用于列表页显示  构造模型
    /// </summary>
    public class TaskByLog
    {
        public string UserPhoneID { get; set; }
        public string TaskID { get; set; }
        public string TaskType { get; set; }
        public string TaskLogDes { get; set; }
        public string TaskPhone { get; set; }
    }
}
