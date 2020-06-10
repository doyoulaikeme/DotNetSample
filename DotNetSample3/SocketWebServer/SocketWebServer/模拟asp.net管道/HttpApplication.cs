using SocketWebServer.网络交互请求封装类;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.模拟asp.net管道
{
    public class HttpApplication : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // 1.获取网站根路径
            string bastPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = Path.Combine(bastPath + "\\PageFile", context.Request.Url.TrimStart('/'));
            string fileExtension = Path.GetExtension(context.Request.Url);
            // 2.处理动态文件请求
            if (fileExtension.Equals(".aspx") || fileExtension.Equals(".ashx"))
            {
                string className = Path.GetFileNameWithoutExtension(context.Request.Url);

                //通过获取程序集公共类型,防止大小写影响页面
                var type = Assembly.Load("SocketWebServer");
                var ExportedTypes = type.GetExportedTypes();
                if (ExportedTypes.Length > 0)
                {
                    var pageName = ExportedTypes.FirstOrDefault(p => p.Name.ToUpper() == className.ToUpper());
                    if (pageName != null)
                    {
                        IHttpHandler handler = Assembly.Load("SocketWebServer").CreateInstance("SocketWebServer.模拟页面." + pageName.Name) as IHttpHandler;
                        handler.ProcessRequest(context);
                    }
                }
                return;
            }
            // 3.处理静态文件请求
            if (!File.Exists(fileName))
            {
                context.Response.StateCode = "404";
                context.Response.StateDescription = "Not Found";
                context.Response.ContentType = "text/html";
                string notExistHtml = Path.Combine(bastPath, @"MyWebSite\notfound.html");
                context.Response.Body = File.ReadAllBytes(notExistHtml);
            }
            else
            {
                context.Response.StateCode = "200";
                context.Response.StateDescription = "OK";
                context.Response.ContentType = GetContenType(Path.GetExtension(context.Request.Url));
                context.Response.Body = File.ReadAllBytes(fileName);
            }
        }

        // 根据文件扩展名获取内容类型
        public string GetContenType(string fileExtension)
        {
            string type = "text/html; charset=UTF-8";
            switch (fileExtension)
            {
                case ".aspx":
                case ".html":
                case ".htm":
                    type = "text/html; charset=UTF-8";
                    break;
                case ".png":
                    type = "image/png";
                    break;
                case ".gif":
                    type = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    type = "image/jpeg";
                    break;
                case ".css":
                    type = "text/css";
                    break;
                case ".js":
                    type = "application/x-javascript";
                    break;
                default:
                    type = "text/plain; charset=gbk";
                    break;
            }
            return type;
        }
    }
}
