using SocketWebServer.模拟asp.net管道;
using SocketWebServer.网络交互请求封装类;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.模拟页面
{
    public class Page : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<html>");
            sbText.Append("<head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/><title>DemoPage</title></head>");
            sbText.Append("<body style='margin:10px auto;text-align:center;'>");
            sbText.Append("<h1>测试</h1>");
            sbText.Append("<table align='center' cellpadding='1' cellspacing='1'><thead><tr><td>ID</td><td>用户名</td></tr></thead>");
            sbText.Append("<tbody>");
            sbText.Append("<tr><td>1</td><td>测试用户</td></tr>");
            sbText.Append("</tbody></table>");
            sbText.Append(string.Format("<h3>更新时间：{0}</h3>", DateTime.Now.ToString()));
            sbText.Append("</body>");
            sbText.Append("</html>");
            context.Response.Body = Encoding.UTF8.GetBytes(sbText.ToString());
            context.Response.StateCode = "200";
            context.Response.ContentType = "text/html";
            context.Response.StateDescription = "OK";
        }



    }
}
