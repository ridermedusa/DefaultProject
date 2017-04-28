using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.BLL
{
    public class EnumCollection
    {

    }
    public enum CusomerLogType
    {
        创建档案 = 0
    }
    public enum BasicType
    {
        部门 = 0,
        职位 = 1,
        行业 = 2,
        投资品种 = 3,
    }
    public enum PlanStatus
    {
        未执行 = 0,
        执行完成 = 1,
        已过期 = 2, 
    }
    public enum TaskStatus
    {
        任务请求中 = 0,
        开始任务 = 1,
        任务完成 = 2,
        任务失败 = 3,
        任务终止 = 4,
    }
    public enum TaskType
    {
        复位 = 0,
        添加联系人 = 1,
        群加好友 = 2,
        群发朋友圈 = 3,
        删除联系人 = 4,
        中断任务 = 5,
        中断并重启任务 = 6
    }

    public enum SignalRType
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        Log = 0,
        /// <summary>
        /// 百分比类型
        /// </summary>
        Percentage = 1
    }
    /// <summary>
    /// 号码
    /// </summary>
    public enum PhoneStatus
    {
        未使用 = 0,
        已分配未使用 = 1,
        已添加联系人 = 2,
        已申请未响应 = 3,
        申请成功 = 4,
        不存在微信 = 5,
    }

    public enum UserPhoneStatus
    {
        正常 = 1,
        任务中 = 2,
        任务异常 = 3,
        停用 = 4
    }
    //消息状态: 0-排队中, 1-发送中，2-发送完成，3-发送失败，4-消息被撤销，
    // 5-消息过期, 6-筛选结果为空，7-定时任务尚未开始处理
    public enum UmengSendStatus
    {
        排队中 = 0,
        发送中 = 1,
        发送完成 = 2,
        发送失败 = 3,
        消息被撤销 = 4,
        消息过期 = 5, 
        筛选结果为空 = 6,
        定时任务尚未开始处理 = 7
    }
    public enum PushReceiveStatus
    {
        未处理 = 0,
        已查看 = 1,
        已清除 = 2,
    }
    //搜索类型
    public enum SerchType
    {
        全部 = 0,
        人气高 = 1,
        风险低 = 2,
        周期短 = 3,
        运作中 = 4
    }
    public enum BannerType
    {
        首页Aanner = 0,
        首页AD = 1
    }
    public enum UserType
    {
        普通用户 = 0,
        投资顾问 = 1,
        达人 = 2,
        分析师 = 3
    }
    public enum CodeType
    {
        绑定手机 = 0,
        找回密码 = 1,
        重绑手机号 = 2
    }
    public enum CodeStatus
    {
        已发送 = 0,
        已使用 = 1,
        已过期 = 2
    }
    public enum LoginType
    {
        QQ = 0,
        WeChat = 1
    }
    public enum LoginTypeDes
    {
        QQ = 0,
        微信 = 1
    }
    public enum LogType
    {
        登录成功 = 1,
        登录失败 = 2,
        删除部门成功 = 3,
        删除部门失败 = 4,
        编辑部门信息 = 5,
        删除工厂 = 6,
        编辑工厂信息 = 7,
        删除管理员 = 8,
        编辑管理员 = 9,
        删除角色 = 10,
        编辑角色 = 11,
        编辑文件 = 12,
        删除文件成功 = 13,
        生成审核流程 = 14,
        上传文件成功 = 15,
        上传文件失败 = 16,
        生成文件 = 17,
        删除职位成功 = 18,
        删除职位失败 = 19,
        编辑职位信息 = 20,
        删除文件类型 = 21,
        添加文件类型 = 22,
        修改文件类型 = 23,
        添加网站维护 = 24,
        修改网站维护 = 25,
        删除网站维护 = 26,
        添加文件系统 = 27,
        修改文件系统 = 28,
        删除文件系统 = 29,
        修改试题维护 = 30,
        添加说明文档 = 31,
        修改说明文档 = 32

    }
}
