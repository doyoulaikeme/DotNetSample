using System;
using System.Linq.Expressions;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Ajax.Utilities;
using mvcQuoteJWT.Models;
using Newtonsoft.Json;


namespace mvcQuoteJWT.Common
{
    public class JwtHelper
    {

        private static string m_Secret = "3DE6F947-34E5-4C0C-9378-C1BFD920BE97";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static string EncodeJwt(LoginInfo login)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlencoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlencoder);
            return encoder.Encode(login, m_Secret);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static LoginInfo DecodeJWT(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var model = decoder.DecodeToObject<LoginInfo>(token, m_Secret, true);
            return model;
        }
    }
}