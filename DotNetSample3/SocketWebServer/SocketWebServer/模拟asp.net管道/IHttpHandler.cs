using SocketWebServer.网络交互请求封装类;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.模拟asp.net管道
{
    public interface IHttpHandler
    {
        void ProcessRequest(HttpContext context);
    }
}
