using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Jurassic.So.Infrastructure
{
    /// <summary>WEB工具</summary>
    public static class WebUtil
    {
        /// <summary>获得URL中的域名URL(类似http://www.temp.com/)</summary>
        public static string GetDomainUrl(this Uri url)
        {
            var builder = new UriBuilder(url.Scheme, url.Host, url.Port);
            return builder.Uri.AbsoluteUri;
        }
        /// <summary>构造带查询串的URL(类似http://www.temp.com/Index?a=***&amp;b=***)</summary>
        public static string GetQueryUrl(this string url, Dictionary<string, object> queryParams)
        {
            if (queryParams.IsNullOrEmpty()) return url;
            var url2 = new StringBuilder(url);
            url2.Append("?");
            foreach (var pair in queryParams)
            {
                var value = HttpUtility.UrlEncode(pair.Value.ToString());
                url2.Append(pair.Key).Append("=").Append(value).Append("&");
            }
            url2.Length -= 1;
            return url2.ToString();
        }
    }
}
