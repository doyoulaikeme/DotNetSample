using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.网络交互请求封装类
{
    /// <summary>
    ///响应内容
    /// </summary>
    public class HttpResponse
    {
        // 响应状态码
        public string StateCode { get; set; }
        // 响应状态描述
        public string StateDescription { get; set; }
        // 响应内容类型
        public string ContentType { get; set; }
        //响应报文的正文内容
        public byte[] Body { get; set; }

        // 生成响应头信息
        public byte[] GetResponseHeader()
        {
            //为了生成响应头信息，需要格式化一个固定格式的信息，并且在最后保留两个换行符
            string strRequestHeader = string.Format(@"HTTP/1.1 {0} {1}
Content-Type: {2}
Accept-Ranges: bytes
Server: Microsoft-IIS/7.5
X-Powered-By: ASP.NET
Date: {3} 
Content-Length: {4}

", StateCode ?? "404", StateDescription ?? "服务器响应错误", ContentType ?? "text/html;charset=utf-8", string.Format("{0:R}", DateTime.Now), Body == null ? 0 : Body.Length);

            return Encoding.UTF8.GetBytes(strRequestHeader);
        }

    }
}
