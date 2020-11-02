using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JWT.Builder;
using mvcQuoteJWT.Common;

namespace mvcQuoteJWT.MyAuthorize
{
    public class JWTAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //jwt的token是加载header，所以在此要验证一下
            var authHeader = httpContext.Request.Headers["auth"];
            if (authHeader != null)
            {
                var infuser = JwtHelper.DecodeJWT(authHeader);
                if (infuser.Name == "admin" && infuser.PassWord == "123")
                {
                    return true;
                }
            }
            httpContext.Response.StatusCode = 403;
            return false;
        }


        /// <summary>
        /// 验证错误处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.Result = new RedirectResult("/Error");
                filterContext.HttpContext.Response.Redirect("/Home/Error");
            }
        }
    }
}