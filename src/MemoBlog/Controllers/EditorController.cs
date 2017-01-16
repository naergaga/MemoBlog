using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using MemoBlog.Models.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MemoBlog.Controllers
{
    public class EditorController : Controller
    {
        private readonly IHostingEnvironment _appEnv;

        public EditorController(IHostingEnvironment env)
        {
            _appEnv = env;
        }

        // GET: /<controller>/
        public IActionResult Upload(IFormFile upfile)
        {
            //上传配置
            string pathbase = "/upload/";    //保存路径
            int size = 10;                  //文件大小限制,单位mb                            
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };//文件允许格式

            UploadInfo info = new UploadInfo();
            info.Size = upfile.Length.ToString();
            //string editorId = context.Request["editorid"];

            // /upload/2016-12-18/
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            // full path dir
            var uploadpath = _appEnv.WebRootPath + pathbase;//获取文件上传路径
            string originalName = upfile.FileName;
            info.OriginalName = originalName;
            string ext = Path.GetExtension(originalName);
            info.Type = ext;
            string URL;
            List<string> error = new List<string>();

            //目录创建
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }

            //格式验证
            if (!filetype.Contains(ext))
            {
                error.Add("格式错误");
            }

            //大小验证
            if (upfile.Length > 3000000)
            {
                error.Add("太大了");
            }
            //保存图片
            if (error.Count == 0)
            {
                Guid guid = Guid.NewGuid();
                string filename = guid.ToString("N") + ext;
                info.Name = filename;
                var stream = new FileStream(uploadpath + filename, FileMode.Create);
                upfile.CopyTo(stream);
                stream.Flush();
                stream.Dispose();
                URL = pathbase + filename;
                info.Url = URL;
                info.State = "SUCCESS";
            }
            else
            {
                error.ForEach(item =>
                {
                    info.State += item + "  ";
                });
            }
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(info, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }),
                ContentType = "text/html"
            };
        }

        public IActionResult Upload2(IFormFile[] upfiles)
        {
            //上传配置
            string pathbase = "/upload/";    //保存路径
            int size = 10000000;                  //文件大小限制                           
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };//文件允许格式

            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            var uploadpath = _appEnv.WebRootPath + pathbase;//获取文件上传路径
            List<string> URLs = new List<string>();

            //目录创建
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }

            foreach (var upfile in upfiles)
            {
                string originalName = upfile.FileName;
                string ext = Path.GetExtension(originalName);
                //格式验证
                if (!filetype.Contains(ext))
                {
                    continue;
                }

                //大小验证
                if (upfile.Length > size)
                {
                    continue;
                }
                //保存图片
                Guid guid = Guid.NewGuid();
                string filename = guid.ToString("N") + ext;
                var stream = new FileStream(uploadpath + filename, FileMode.Create);
                upfile.CopyTo(stream);
                stream.Flush();
                stream.Dispose();
                URLs.Add(pathbase+filename);
            }


            return Json(new
            {
                Urls = URLs
            });
        }
    }
}
