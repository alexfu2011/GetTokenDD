using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GetTokenDD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static string apikey = "api key";
        private static string secretkey = "secret key";
        private static string method = "POST";
        private static string baseUrl = "https://openplatform.dg-work.cn";
        private static string token_uri = "/gettoken.json";

        private IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var token = _memoryCache.GetOrCreate("token", (entry) =>
            {
                NameValueCollection formData = new NameValueCollection();
                var headers = Headers();
                string res = Utils.PostData(formData, method, baseUrl + token_uri, headers);
                Token token = JsonConvert.DeserializeObject<Token>(res);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.content.data.expiresIn);
                return token.content.data.accessToken;
            });
            return Ok(new TokenResult { Token = token });
        }

        private static Dictionary<string, string> Headers()
        {
            string timestamp = Utils.TimeStamp();
            string nonce = Utils.GetTime13() + Utils.RandomInt4();
            var headers = new Dictionary<string, string>();
            headers["X-Hmac-Auth-Timestamp"] = timestamp;
            headers["X-Hmac-Auth-Version"] = "1.0";
            headers["X-Hmac-Auth-Nonce"] = nonce;
            headers["apiKey"] = apikey;
            headers["X-Hmac-Auth-Signature"] = Utils.Signature(secretkey, method, timestamp, nonce, token_uri);
            headers["X-Hmac-Auth-IP"] = "127.0.0.1";
            headers["X-Hmac-Auth-MAC"] = Utils.GetMacByNetworkInterface();
            return headers;
        }
    }
}
