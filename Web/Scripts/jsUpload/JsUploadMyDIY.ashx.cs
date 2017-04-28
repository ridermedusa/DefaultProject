using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Scripts.jsUpload
{
    /// <summary>
    /// JsUploadMyDIY 的摘要说明
    /// </summary>
    public class JsUploadMyDIY : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpPostedFile file = context.Request.Files["FileData"];
            //context.Request["folder"]

            string path = "/BPM_Excel";

            string uploadpath = HttpContext.Current.Server.MapPath(path) + "\\";
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                if (File.Exists(uploadpath + file.FileName))
                {
                    File.Delete(uploadpath + file.FileName);
                }
                file.SaveAs(uploadpath + file.FileName);

                context.Response.Write(path + "/" + file.FileName);

            }
            else
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}