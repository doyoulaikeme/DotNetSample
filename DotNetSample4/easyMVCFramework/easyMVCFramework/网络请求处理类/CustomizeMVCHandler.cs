using easyMVCFramework.自定义控制器接口;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
namespace easyMVCFramework.网络请求处理类
{
    public class CustomizeMVCHandler : IHttpHandler
    {
        /// <summary>
        /// 自定义路由表
        /// </summary>
        private IDictionary<string, string> Routes { get; set; }
        /// <summary>
        /// 所有控制器的类型集合
        /// </summary>
        private static IList<Type> allControllType;
        public bool IsReusable { get; }


        /// <summary>
        /// 第一次加载时调用静态获取所有Controller
        /// </summary>
        static CustomizeMVCHandler()
        {
            allControllType = new List<Type>();
            // 获得当前所有引用的程序集
            var assemblies = BuildManager.GetReferencedAssemblies();
            // 遍历所有的程序集
            foreach (Assembly assembly in assemblies)
            {
                // 获取当前程序集中所有的类型
                var allTypes = assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsPublic && typeof(IController).IsAssignableFrom(type)).ToList();

                if (allTypes.Count > 0)
                {
                    for (int i = 0; i < allTypes.Count; i++)
                    {
                        // 将所有Controller加入集合
                        allControllType.Add(allTypes[i]);
                    }

                }
            }
        }


        /// <summary>
        /// 构造函数获取自定义路由规则
        /// </summary>
        /// <param name="routes"></param>
        public CustomizeMVCHandler(IDictionary<string, string> routes)
        {
            this.Routes = routes;
        }
        /// <summary>
        /// http请求事件
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //获取控制器名称
            var controllerName = Routes["{controller}"];

            //默认控制器
            if (string.IsNullOrEmpty(controllerName))
            {
                // 指定默认控制器
                controllerName = "home";
            }


            IController controller = null;
            //通过反射创建具体实例
            var requestControllName = allControllType.FirstOrDefault(p => p.Name.Equals(string.Format("{0}Controller", controllerName), StringComparison.InvariantCultureIgnoreCase));
            if (requestControllName != null)
            {
                controller = Activator.CreateInstance(requestControllName) as IController;
            }
            //传输当前上下文及路由规则表
            var requestContext = new HttpContextWrapper()
            {
                Context = context,
                Routes = Routes
            };

            //通过反射获取相应控制器并统一调用接口执行方法
            controller.Exec(requestContext);

        }
    }
}