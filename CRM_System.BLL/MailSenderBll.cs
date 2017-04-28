using CRM_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net;
using CRM_System.BLL;

namespace CRM_System.BLL
{
    public class MailSenderBll
    {
        public static void Send(string server, string sender, string recipient, string subject,
    string body, bool isBodyHtml, Encoding encoding, bool isAuthentication, params string[] files)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(server);
                smtpClient.UseDefaultCredentials = true;
                MailMessage message = new MailMessage(sender, recipient);
                message.IsBodyHtml = isBodyHtml;
                message.SubjectEncoding = encoding;
                message.BodyEncoding = encoding;
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.Attachments.Clear();
                if (files != null && files.Length != 0)
                {
                    for (int i = 0; i < files.Length; ++i)
                    {
                        Attachment attach = new Attachment(files[i]);
                        message.Attachments.Add(attach);
                    }
                }

                if (isAuthentication == true)
                {
                    smtpClient.Credentials = new NetworkCredential(SmtpConfig.Create().SmtpSetting.User, SmtpConfig.Create().SmtpSetting.Password);
                }
                //smtpClient.EnableSsl = true;

                //smtpClient.Timeout = 5;
                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                Sys_Error_logService LogService = new Sys_Error_logService();
                Sys_Error_log Errmod = new Sys_Error_log();
                Errmod.Date = DateTime.Now;
                Errmod.ErrMsg = e.Message;
                Errmod.ErrType = "发送邮件";
                LogService.Insert(Errmod);
            }
        }
        /// <summary>
        /// recipient:发送人subject：标题body：正文
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void Send(string recipient, string subject, string body)
        {
            Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.Sender, recipient, subject, body, true, Encoding.Default, true, null);
        }

        public static void Send(string recipient, string subject, string body, string[] files)
        {
            //Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.Sender, recipient, subject, body, true, Encoding.Default, true, files);
        }

        public static void Send(string Recipient, string Sender, string Subject, string Body)
        {
            //Send(SmtpConfig.Create().SmtpSetting.Server, Sender, Recipient, Subject, Body, true, Encoding.UTF8, true, null);
        }
        static readonly string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
        static readonly string userName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
        static readonly string pwd = System.Configuration.ConfigurationManager.AppSettings["Pwd"];
        static readonly int smtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]);
        static readonly string authorName = System.Configuration.ConfigurationManager.AppSettings["AuthorName"];
        static readonly string to = System.Configuration.ConfigurationManager.AppSettings["To"];
    }

    public class SmtpSetting
    {
        private string _server;

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }
        private bool _authentication;

        public bool Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
        }
        private string _user;

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
        private string _sender;

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }

    public class SmtpConfig
    {
        private static SmtpConfig _smtpConfig;
        private string ConfigFile
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["SmtpConfigPath"];
                if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
                {
                    configPath = HttpContext.Current.Request.MapPath("/Config/SmtpSetting.config");
                }
                else
                {
                    if (!Path.IsPathRooted(configPath))
                        configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "SmtpSetting.config"));
                    else
                        configPath = Path.Combine(configPath, "SmtpSetting.config");
                }
                return configPath;
            }
        }
        public SmtpSetting SmtpSetting
        {
            get
            {
                //XmlDocument doc = new XmlDocument();
                //doc.Load(this.ConfigFile);
                //线程发送不能获取配置文件
                SmtpSetting smtpSetting = new SmtpSetting();
                smtpSetting.Server = "smtp.sina.com";// doc.DocumentElement.SelectSingleNode("Server").InnerText;
                smtpSetting.Authentication = false;// Convert.ToBoolean(doc.DocumentElement.SelectSingleNode("Authentication").InnerText);
                smtpSetting.User = "rider84391558@sina.com";// doc.DocumentElement.SelectSingleNode("User").InnerText;
                smtpSetting.Password = "24556178";// doc.DocumentElement.SelectSingleNode("Password").InnerText;
                smtpSetting.Sender = "rider84391558@sina.com";// doc.DocumentElement.SelectSingleNode("Sender").InnerText;

                return smtpSetting;
            }
        }
        private SmtpConfig()
        {

        }
        public static SmtpConfig Create()
        {
            if (_smtpConfig == null)
            {
                _smtpConfig = new SmtpConfig();
            }
            return _smtpConfig;
        }
    }
}
