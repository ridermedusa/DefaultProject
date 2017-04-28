﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Scripts.jsUpload
{
    /// <summary>
    /// UploadifyByHead 的摘要说明
    /// </summary>
    public class UploadifyByHead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpPostedFile file = context.Request.Files["FileData"];
            //context.Request["folder"]
            string path = "/UploadFile/Photo";

            string uploadpath = HttpContext.Current.Server.MapPath(path) + "\\";
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }

                string filetype = file.FileName.Substring(file.FileName.LastIndexOf("."));
                string FN = file.FileName.Substring(0, file.FileName.LastIndexOf("."));
                string ffilename = DateTime.Now.ToString("yyyyMMddhhmmssffff") + filetype;
                file.SaveAs(uploadpath + ffilename);
                string Jsoncode = "{\"FileName\":\"" + FN + "\",\"FileUrl\":\"" + path + "/" + ffilename + "\",\"FileType\":\"" + filetype.TrimStart('.') + "\"}";
                context.Response.Write(Jsoncode);

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