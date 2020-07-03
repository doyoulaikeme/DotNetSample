using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcQuoteJWT.Common;
using mvcQuoteJWT.Models;
using mvcQuoteJWT.MyAuthorize;

namespace mvcQuoteJWT.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        [JWTAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [JWTAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateToken(string username, string password)
        {
            DataResult result = new DataResult();
            //假设用户名为"admin"，密码为"123"  
            if (username == "admin" && password == "123")
            {

                var payload = new LoginInfo()
                {
                    Name = username,
                    PassWord = password

                };

                result.Token = JwtHelper.EncodeJwt(payload);
                result.Success = true;
                result.Message = "成功";
            }
            else
            {
                result.Token = "";
                result.Success = false;
                result.Message = "生成token失败";
            }

            return Json(result);
        }
    }

    public class DataResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}