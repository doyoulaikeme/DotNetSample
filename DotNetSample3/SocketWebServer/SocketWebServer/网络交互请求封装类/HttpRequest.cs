using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketWebServer.网络交互请求封装类
{
    /// <summary>
    /// 请求内容
    /// </summary>
    public class HttpRequest
    {
        // 请求方式：GET or POST?
        public string HttpMethod { get; set; }
        // 请求URL
        public string Url { get; set; }
        // Http协议版本
        public string HttpVersion { get; set; }
        // 请求头
        public Dictionary<string, string> HeaderDictionary { get; set; }
        // 请求体
        public Dictionary<string, string> BodyDictionary { get; set; }
        public HttpRequest(string requestText)
        {
            string[] lines = requestText.Replace("\r\n", "\r").Split('\r');
            string[] requestLines = lines[0].Split(' ');
            // 获取HTTP请求方式、请求的URL地址、HTTP协议版本

            if (requestLines.Length > 0)
            {
                HttpMethod = requestLines[0];
            }
            if (requestLines.Length > 1)
            {
                Url = requestLines[1];
            }
            if (requestLines.Length > 2)
            {
                HttpVersion = requestLines[2];
            }

        }

    }
}
