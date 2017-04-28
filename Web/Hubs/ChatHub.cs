using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRChat
{
    #region  定义上线用户模型
    public class UserInfo
    {
        /// <summary>
        /// 系统内对应用户ID
        /// </summary>
        public string ConnectionId { get; set; }
        public int AdminID { get; set; }  
    }
    #endregion
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly ChatTicker ticker; 
        public ChatHub()
        {
            ticker = ChatTicker.Instance;
        }
        public static List<UserInfo> OnlineUsers = new List<UserInfo>(); // 在线用户列表
        #region 用户上线
        /// <summary>
        /// 传入当前用户ADMIN表示用户上线
        /// </summary>
        /// <param name="AdminID"></param>
        public void UserLogin(int AdminID)
        {
            ///判断当前用户是否已经存在于登录列表内
            if (OnlineUsers.Where(s => s.AdminID == AdminID).Count() > 0)
            {
                //每次登陆id会发生变化,首先清除原有数据
                OnlineUsers.RemoveAll(x => x.AdminID == AdminID);
            }
            //直接进行添加
            UserInfo UInfo = new UserInfo();
            UInfo.ConnectionId = Context.ConnectionId;
            UInfo.AdminID = AdminID;
            OnlineUsers.Add(UInfo);
            //新用户上线，服务器广播该用户名
            Clients.All.loginUser(OnlineUsers);
        }
        #endregion
        #region 发送数据
        public void SendData(int AdminID, string value)
        {
            UserInfo Umod = OnlineUsers.Where(s => s.AdminID == AdminID).FirstOrDefault();
            if (Umod != null)
            {
                Clients.Client(Umod.ConnectionId).SendMessage(value);
            }
        }
        public  void SendData(string ConnectionId, string value)
        {
            Clients.Client(ConnectionId).SendMessage(value);
        }
        #endregion
        #region 注册实例
        public class ChatTicker
        {
            #region 实现一个单例  

            private static readonly ChatTicker _instance =
                new ChatTicker(GlobalHost.ConnectionManager.GetHubContext<ChatHub>());

            private readonly IHubContext m_context;

            private ChatTicker(IHubContext context)
            {
                m_context = context;
                //这里不能直接调用Sender，因为Sender是一个不退出的“死循环”，否则这个构造函数将不会退出。  
                //其他的流程也将不会再执行下去了。所以要采用异步的方式。  
                Task.Run(() => Sender());
            } 
            public IHubContext GlobalContext
            {
                get { return m_context; }
            }

            public static ChatTicker Instance
            {
                get { return _instance; }
            }

            #endregion

            public void Sender()
            {
                int count = 0;
                while (true)
                {
                    Thread.Sleep(500);
                    int tag = count % 2;
                    //动态绑定前端的js函数 broadcaseMessage  
                    m_context.Clients.Group(tag + "").broadcastMessage("group is:" + tag, "current count:" + count);
                    count++;
                }
            }
        }
        #endregion
    }
} 