using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Jurassic.So.Infrastructure.Util
{
    public static class WebRequestUtil
    {
        public static string GetResponseStr(string api)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(api);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;
            wrp = wrt.GetResponse();
            return new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
        public static string PostResponseStr(string api, string json)
        {
            string rs = null;
            if (!string.IsNullOrEmpty(json))
            {
                if (string.IsNullOrEmpty(api))
                {
                    throw new Exception("Web Api不能为空！");
                }
                ServicePointManager.DefaultConnectionLimit = 300;
                System.GC.Collect();
                CookieContainer cookieContainer = new CookieContainer();
                // 设置提交的相关参数 
                HttpWebRequest request = null;
                HttpWebResponse SendSMSResponse = null;
                Stream dataStream = null;
                StreamReader SendSMSResponseStream = null;
                try
                {
                    request = WebRequest.Create(api) as HttpWebRequest;
                    request.Method = "POST";
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLimit = 300;
                    request.AllowAutoRedirect = true;
                    request.Timeout = 10000;
                    request.ReadWriteTimeout = 10000;
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
                    string strContent = json;
                    byte[] bytes = Encoding.UTF8.GetBytes(strContent);
                    request.Proxy = null;
                    request.CookieContainer = cookieContainer;
                    using (dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(bytes, 0, bytes.Length);
                    }
                    SendSMSResponse = (HttpWebResponse)request.GetResponse();
                    if (SendSMSResponse.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        if (SendSMSResponse != null)
                        {
                            SendSMSResponse.Close();
                            SendSMSResponse = null;
                        }
                        if (request != null)
                        {
                            request.Abort();
                        }
                        return null;
                    }
                    SendSMSResponseStream = new StreamReader(SendSMSResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                    string strRespone = SendSMSResponseStream.ReadToEnd();

                    return strRespone;
                }
                finally
                {
                    if (dataStream != null)
                    {
                        dataStream.Close();
                        dataStream.Dispose();
                        dataStream = null;
                    }
                    if (SendSMSResponseStream != null)
                    {
                        SendSMSResponseStream.Close();
                        SendSMSResponseStream.Dispose();
                        SendSMSResponseStream = null;
                    }
                    if (SendSMSResponse != null)
                    {
                        SendSMSResponse.Close();
                        SendSMSResponse = null;
                    }
                    if (request != null)
                    {
                        request.Abort();
                    }
                }
            }
            return rs;
        }

        /// <summary>
        /// Get调用API服务的通用方法，参数会被转换成Json参数。
        /// </summary>
        /// <param name="api">API服务路径</param>
        /// <param name="prams">调用参数，Key，Value形式包裹，简单支持string，int，Guid</param>
        /// <param name="token">授权验证信息</param>
        /// <returns></returns>
        public static async Task<string> GetHttpClientStr(string api, Dictionary<string, string> prams, string token = null)
        {
            if (string.IsNullOrWhiteSpace(api))
                throw new ArgumentNullException("api");

            string jsonlist = string.Empty;
            using (var clientdb = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    clientdb.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", token);
                }

                //Walt add --20160427
                clientdb.Timeout = DateTime.Now - DateTime.Now.AddHours(-4);
                clientdb.MaxResponseContentBufferSize = int.MaxValue;

                if (prams != null)
                {
                    int i = prams.Count();
                    foreach (var item in prams)
                    {
                        if (i == prams.Count())
                            api += "?";
                        api += item.Key + "=" + item.Value + "&";

                        if (i == 1)
                            api = api.Substring(0, api.Count() - 1);
                        i--;
                    }
                }
                var resultDataGet = clientdb.GetAsync(api).Result;
                if (resultDataGet.StatusCode.ToString() == "OK")
                {
                    jsonlist = await resultDataGet.Content.ReadAsStringAsync();
                }
            }

            return jsonlist;
        }

        /// <summary>
        /// Post调用API服务的通用方法
        /// </summary>
        /// <param name="api">API服务路径</param>
        /// <param name="pramsKeyValue">调用参数，Key，Value形式包裹，复杂类型请使用Json格式的字符串</param>
        /// <param name="token">授权验证信息</param>
        /// <returns></returns>
        public static async Task<string> PostHttpClientStr(string api, Dictionary<string, string> pramsKeyValue, string token = null)
        {
            if (string.IsNullOrWhiteSpace(api))
                throw new ArgumentNullException("api");
            //if (prams == null || prams.Count == 0)
            //    throw new ArgumentNullException("prams");

            //string pramJson = pramsKeyValue.Where(item => !string.IsNullOrWhiteSpace(item.Key)).Aggregate("{", (current, item) => current + ((string.IsNullOrWhiteSpace(item.Key) ? "" : item.Key) + ":" + item.Value));

            string pramJson = "{";
            foreach (var item in pramsKeyValue)
            {
                pramJson += "'" + item.Key + "' " + ":" + " '" + item.Value + "'" + ",";
            }
            pramJson = pramJson.Substring(0, pramJson.Length - 1);

            pramJson += "}";
            HttpContent requestContent = new StringContent(pramJson);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string jsonlist = string.Empty;
            using (var clientdb = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    clientdb.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", token);
                }
                var resultDataPost = clientdb.PostAsync(api, requestContent).Result;
                if (resultDataPost.StatusCode.ToString() == "OK")
                {
                    jsonlist = await resultDataPost.Content.ReadAsStringAsync();
                }
            }
            return jsonlist;
        }
        /// <summary>
        /// Post调用API服务的通用方法，方法不会将参数转换成 Json，而直接使用。
        /// </summary>
        /// <param name="api">API服务路径</param>
        /// <param name="pramsKeyValue">调用参数，Key，Value形式包裹，简单支持string，int，Guid</param>
        /// <param name="token">授权验证信息</param>
        /// <returns></returns>
        public static async Task<string> PostHttpClientStrByHttpContent(string api, Dictionary<string, string> pramsKeyValue, string token = null)
        {
            if (string.IsNullOrWhiteSpace(api))
                throw new ArgumentNullException("api");
            //if (prams == null || prams.Count == 0)
            //    throw new ArgumentNullException("prams");

            //string pramJson = pramsKeyValue.Where(item => !string.IsNullOrWhiteSpace(item.Key)).Aggregate("{", (current, item) => current + ((string.IsNullOrWhiteSpace(item.Key) ? "" : item.Key) + ":" + item.Value));


            string jsonlist = string.Empty;
            using (var clientdb = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    clientdb.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", token);
                }
                var resultDataPost = clientdb.PostAsync(api, new FormUrlEncodedContent(pramsKeyValue)).Result;
                if (resultDataPost.StatusCode.ToString() == "OK")
                {
                    jsonlist = await resultDataPost.Content.ReadAsStringAsync();
                }
            }
            return jsonlist;
        }
        /// <summary>
        /// 用JSON参数Post调用API服务的通用方法
        /// </summary>
        /// <param name="url">API服务路径</param>
        /// <param name="json">Json格式的参数</param>
        /// <param name="token">授权验证信息</param>
        /// <returns>API应答信息</returns>
        public static async Task<string> PostHttpClientStr(string url, string json, string token = null)
        {
            return await PostJson(url, json, token).ConfigureAwait(false);
        }
        /// <summary>
        /// 用JSON参数Post调用API服务的通用方法
        /// </summary>
        /// <param name="url">API服务路径</param>
        /// <param name="json">Json格式的参数</param>
        /// <param name="token">授权验证信息</param>
        /// <returns>API应答信息</returns>
        public static async Task<string> PostJson(string url, string json, string token = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));
            var requestContent = new StringContent(json ?? "");
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string result = null;
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await httpClient.PostAsync(url, requestContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return result;
        }
        /// <summary>
        /// Post调用API服务的通用方法
        /// </summary>
        /// <param name="api">API服务路径</param>
        /// <param name="pramsHttpContent">调用参数，HttpClient的标准参数</param>
        /// <param name="token">授权验证信息</param>
        /// <returns></returns>
        public static async Task<string> PostHttpClientStr(string api, HttpContent pramsHttpContent, string token = null)
        {
            if (string.IsNullOrWhiteSpace(api))
                throw new ArgumentNullException("api");
            //if (prams == null || prams.Count == 0)
            //    throw new ArgumentNullException("prams");



            string jsonlist = string.Empty;
            using (var clientdb = new HttpClient())
            {
                //clientdb.BaseAddress = new Uri("http://localhost:1668/");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    clientdb.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", token);
                }
                var resultDataPost = clientdb.PostAsync(api, pramsHttpContent).Result;
                if (resultDataPost.StatusCode.ToString() == "OK")
                {
                    jsonlist = await resultDataPost.Content.ReadAsStringAsync();
                }
            }

            return jsonlist;
        }

        /// <summary>
        /// 通过文件头信息传递账号与验证信息
        /// </summary>
        /// <param name="baseUrl">基础路径，域名</param>
        /// <param name="name">用户</param>
        /// <param name="key">授权</param>
        /// <returns></returns>
        public static string GetToken(string baseUrl, string name, string key)
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(name + ":" + key)));

                var parameters = new Dictionary<string, string>();
                parameters.Add("grant_type", "client_credentials");

                //AccessTokenModel accessTokenModel = new AccessTokenModel();
                HttpResponseMessage res = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result;
                string accessTokenModel = null;
                if (res.StatusCode.ToString() == "OK")
                {
                    accessTokenModel = res.Content.ReadAsStringAsync().Result;
                }
                return accessTokenModel;
            }
        }

        public static string GetTokenAccessKey(string baseUrl, string name, string key)
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(name + ":" + key)));

                var parameters = new Dictionary<string, string>();
                parameters.Add("grant_type", "client_credentials");

                HttpResponseMessage res = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result;
                string accessTokenModel = null;
                if (res.StatusCode.ToString() == "OK")
                {
                    string tokenStr = res.Content.ReadAsStringAsync().Result;
                    var tokenMap = JObject.Parse(tokenStr);
                    accessTokenModel = tokenMap["access_token"].ToString();
                }
                return accessTokenModel;
            }
        }

        public static void Getasw()
        {

        }
    }
}
