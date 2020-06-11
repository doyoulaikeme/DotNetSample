using easyMVCFramework.Model;
using easyMVCFramework.网络请求处理类;
using easyMVCFramework.自定义控制器接口;
using System;
using System.Collections.Generic;
using System.Linq;
namespace easyMVCFramework.Controller
{
    public class HomeController : IController
    {
        private System.Web.HttpContext currentContext;

        public void Exec(HttpContextWrapper context)
        {
            currentContext = context.Context;
            // 获取Action名称
            string actionName = "index";
            if (context.Routes.ContainsKey("{action}"))
            {
                actionName = context.Routes["{action}"];
            }

            switch (actionName.ToLower())
            {
                case "index":
                    this.Index();
                    break;
                case "test":
                    this.Test();
                    break;
                default:
                    this.Index();
                    break;
            }
        }

        public void Index()
        {
            currentContext.Response.Write("访问/Home/Index成功!");
        }

        public void Test()
        {
            var productList = GetProductList();
            foreach (var product in productList)
            {
                currentContext.Response.Write(string.Format("{0}-{1}-{2}<br/>", product.ProductId,
                    product.ProductName, product.ProductDescription));
            }

            currentContext.Response.Write("访问/Home/Test!");
        }

        public IList<Product> GetProductList()
        {
            IList<Product> productList = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                Product model = new Product();
                model.ProductId = "ProductId：" + Guid.NewGuid();
                model.ProductName = "ProductName：" + i.ToString();
                model.ProductDescription = "ProductDescription： " + model.ProductName;
                productList.Add(model);
            }

            return productList;
        }
    }
}