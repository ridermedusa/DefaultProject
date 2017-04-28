using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using Web.Base;

namespace Web.Scripts.Jcrop
{
    /// <summary>
    /// MyJcrop 的摘要说明
    /// </summary>
    public class MyJcrop : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int x = 1, y = 1, w = 1, h = 1;
            string f = string.Empty;
            if (context.Request["x"] == null || context.Request["y"] == null || context.Request["w"] == null || context.Request["h"] == null)
            {
                context.Response.Write("{\"Code\":\"1\",\"Errmsg\":\"没有发现裁剪信息\"}");
                context.Response.End();
            }
            if (context.Request["url"] == null)
            {
                context.Response.Write("{\"Code\":\"1\",\"Errmsg\":\"图片文件不存在\"}");
                context.Response.End();
            }
            else
            {
                f = context.Request["url"].ToString().Replace("/", "\\");
            }
            try
            {
                x = int.Parse(context.Request["x"].ToString());
                y = int.Parse(context.Request["y"].ToString());
                w = int.Parse(context.Request["w"].ToString());
                h = int.Parse(context.Request["h"].ToString());
            }
            catch(Exception ex)
            {
                context.Response.Write("{\"Code\":\"1\",\"Errmsg\":\"参数解析错误\"}");
                context.Response.End();
            }
            if (!File.Exists(context.Server.MapPath("~\\" + f)))
            {
                context.Response.Write("{\"Code\":\"1\",\"Errmsg\":\"图片文件不存在\"}");
                context.Response.End();
            } 
            Bitmap b;
            Graphics g;
            using (Image img = System.Drawing.Image.FromFile(context.Server.MapPath("~\\" + f)))
            {
                b = new Bitmap(w, h, img.PixelFormat);
                b.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(img, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
                img.Dispose();
            }
            string uploadpath = HttpContext.Current.Server.MapPath("/UploadFile") + "\\";
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
            string uploadpath2 = HttpContext.Current.Server.MapPath("/UploadFile/Photo") + "\\";
            if (!Directory.Exists(uploadpath2))
            {
                Directory.CreateDirectory(uploadpath2);
            }
            string ff = "/UploadFile/Photo/" + context.Request["PicName"].ToString() + ".jpeg";
            b.Save(context.Server.MapPath(ff));
            b.Dispose();
            g.Dispose(); 
            context.Response.Write("{\"Code\":\"0\",\"Errmsg\":\"Success\",\"url\":\""+ ff + "\"}"); 
            context.Response.End();
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