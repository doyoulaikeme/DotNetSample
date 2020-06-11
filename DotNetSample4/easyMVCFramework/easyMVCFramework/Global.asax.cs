using easyMVCFramework.网络请求处理类;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace easyMVCFramework
{
    public class Global : System.Web.HttpApplication
    {

        // 自定义路由
        private static IList<string> Routes;

        /// <summary>
        /// 程序启动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            Routes = new List<string>();
            //根据MVC路由规则，只要前一项匹配，后面的规则不作数。
            //http://test.com/Home
            Routes.Add("{controller}");
            //http://test.com/Home/Index
            Routes.Add("{controller}/{action}");
        }

        /// <summary>
        /// 缓存开始事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 程序开始请求事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //模拟路由表实现映射服务
            //模拟路由字典
            IDictionary<string, string> routeData = new Dictionary<string, string>();
            //将URL与路由表中进行对比，并匹配对应关系。
            foreach (var route in Routes)
            {
                // 获得当前请求的参数数组。
                var execPath = Request.AppRelativeCurrentExecutionFilePath;
                //如果参数为空则执行默认配置规则。
                if (string.IsNullOrEmpty(execPath) || execPath.Equals("~/"))
                {
                    execPath = "/home/index";
                }
                //分割当前请求的路径
                var execPathArray = execPath.Replace("~/", "").Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                //分割路由规则
                var routeKey = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                //判断路由规则与当前请求是否一致
                if (execPathArray.Length == routeKey.Length)
                {
                    for (int i = 0; i < routeKey.Length; i++)
                    {
                        //添加到自定义路由字典中
                        routeData.Add(routeKey[i], execPathArray[i]);
                    }

                    //指定Handler进行后续处理
                    Context.RemapHandler(new CustomizeMVCHandler(routeData));
                    //匹配成功则结束此次请求
                    break;
                }
            }
        }
        /// <summary>
        /// 程序授权请求事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 程序错误处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 缓存结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 程序结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}