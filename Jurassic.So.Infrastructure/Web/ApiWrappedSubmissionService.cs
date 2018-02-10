using System;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.PKS.WebAPI.Submission
{
    /// <summary>API包装提交服务</summary>
    public class ApiWrappedSubmissionService
    {
        /// <summary>构造函数</summary>
        static ApiWrappedSubmissionService()
        {
            HttpClient = new HttpClientWrapper();
            //配置
            HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60 * 3);
        }
        /// <summary>构造函数</summary>
        public ApiWrappedSubmissionService(string url)
        {
            this.ServiceUrl = url.Trim().TrimEnd('/');
        }
        /// <summary>Http客户端包装器</summary>
        public static HttpClientWrapper HttpClient { get; private set; }
        /// <summary>服务URL</summary>
        private string ServiceUrl { get; set; }
        /// <summary>获得服务能力信息URL</summary>
        public string Url_GetCapabilities
        {
            get { return this.ServiceUrl + "/GetCapabilities"; }
        }
        /// <summary>获得服务能力信息</summary>
        public ServiceCapabilities GetCapabilities()
        {
            return GetCapabilitiesAsync().Result;
        }
        /// <summary>获得服务能力信息</summary>
        public async Task<ServiceCapabilities> GetCapabilitiesAsync()
        {
            return await HttpClient.GetAsync<ServiceCapabilities>(this.Url_GetCapabilities).ConfigureAwait(false);
        }
        /// <summary>上传一个成果文件URL</summary>
        public string Url_Upload
        {
            get { return this.ServiceUrl + "/Upload"; }
        }
        /// <summary>上传一个成果文件</summary>
        public string Upload(string file)
        {
            return UploadAsync(file).Result;
        }
        /// <summary>上传一个成果文件</summary>
        public async Task<string> UploadAsync(string file)
        {
            var result = await HttpClient.UploadAsync<string>(this.Url_Upload, file).ConfigureAwait(false);
            return JObject.Parse(result).First.As<JProperty>().Value.As<JValue>().Value.ToString();
        }
        /// <summary>提交成果URL</summary>
        public string Url_Submit
        {
            get { return this.ServiceUrl + "/Submit"; }
        }
        /// <summary>提交成果</summary>
        public string Submit(SubmissionInfoRequest request)
        {
            return SubmitAsync(request).Result;
        }
        /// <summary>提交成果</summary>
        public async Task<string> SubmitAsync(SubmissionInfoRequest request)
        {
            var result = await HttpClient.PostAsync<string>(this.Url_Submit, request.ToJson()).ConfigureAwait(false);
            return JObject.Parse(result).First.As<JProperty>().Value.As<JValue>().Value.ToString();
        }
    }
}
