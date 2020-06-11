using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyMVCFramework.网络请求处理类
{
    /// <summary>
    /// 原httpContextBase封装类
    /// </summary>
    public class HttpContextWrapper
    {
        /// <summary>
        /// 网络请求上下文
        /// </summary>
        public HttpContext Context { get; set; }
        /// <summary>
        /// 自定义路由表
        /// </summary>
        public IDictionary<string, string> Routes { get; set; }
    }
}