using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.Model
{
    /// <summary>
    /// 构造APP基础类型数据
    /// </summary>
    public class Project_AppConfig
    {
        public Project_AppConfig(int MaxPhone, int MaxUser, string ServerIP)
        {
            this.MaxPhone = MaxPhone;
            this.MaxUser = MaxUser;
            this.ServerIP = ServerIP;
        }
        /// <summary>
        /// 最高可用电话
        /// </summary>
        public int MaxPhone { get; set; }
        /// <summary>
        /// 最高可定义业务员
        /// </summary>
        public int MaxUser { get; set; }
        /// <summary>
        /// 链接IP地址c
        /// </summary>
        public string ServerIP { get; set; } 
    }
}
