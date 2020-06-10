using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.网络交互请求封装类
{
    /// <summary>
    /// Http上下文
    /// </summary>
    public class HttpContext
    {
        /// <summary>
        /// 请求
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        /// 响应
        /// </summary>
        public HttpResponse Response { get; set; }

        /// <summary>
        /// 构造函数    
        /// </summary>
        /// <param name="requestText"></param>
        public HttpContext(string requestText)
        {
            Request = new HttpRequest(requestText);
            Response = new HttpResponse();
        }
    }
}
