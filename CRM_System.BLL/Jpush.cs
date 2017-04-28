using cn.jpush.api;
using cn.jpush.api.push;
using cn.jpush.api.push.mode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_System.BLL
{
    public class Jpush
    {
        private static string AppKey = "05af5d84cdea23a122e7ce10";
        private static string Master_Secret = "8d76e006354176b358750740";
        /// <summary>
        ///  向指定安卓手机推送一条
        /// </summary>
        /// <param name="registrationId">注册ID </param>
        /// <param name="Data">数据</param>
        /// <param name="TaskID">任务ID</param>
        /// <returns></returns>
        public static void PushObject_Android(string RegistrationId,string Data,string TaskID,TaskType TaskType,string Extra = "")
        {
            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_registrationId(RegistrationId);

            //入值发送内容,然后将任务ID作为附加ID传递过去
            Message Info = Message.content("{\"Data\":" + Data + "}").AddExtras("Operation", (int)TaskType).AddExtras("TaskID", TaskID);
            Info.title = Extra;
            pushPayload.message = Info;
            Options Option = new Options(); 
            Option.time_to_live = 0;//如果当前用户离线，则不接收任何消息
            Option.apns_production = true;//当前所有推送都是正式环境下进行
            pushPayload.options = Option;
            JPushClient client = new JPushClient(AppKey, Master_Secret);
            MessageResult Result = client.SendPush(pushPayload); 
        }
    }
}
