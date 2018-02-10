using System;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.So.Business;

namespace Jurassic.PKS.WebAPI.Submission
{
    /// <summary>API包装搜索服务</summary>
    public class ApiWrappedSearchService
    {
        /// <summary>构造函数</summary>
        static ApiWrappedSearchService()
        {
            HttpClient = new HttpClientWrapper();
            //配置
            HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60 * 3);
        }
        /// <summary>构造函数</summary>
        public ApiWrappedSearchService(string url)
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
        /// <summary>获得元数据标签定义</summary>
        public string Url_GetMetadataDefinition
        {
            get { return this.ServiceUrl + "/GetMetadataDefinition"; }
        }
        /// <summary>获得元数据标签定义</summary>
        public MetadataDefinitionCollection GetMetadataDefinition()
        {
            return GetMetadataDefinitionAsync().Result;
        }
        /// <summary>获得元数据标签定义</summary>
        public async Task<MetadataDefinitionCollection> GetMetadataDefinitionAsync()
        {
            var result = await HttpClient.PostAsync<MetadataDefinition[]>(this.Url_GetMetadataDefinition, null).ConfigureAwait(false);
            return new MetadataDefinitionCollection(result);
        }
    }
}
