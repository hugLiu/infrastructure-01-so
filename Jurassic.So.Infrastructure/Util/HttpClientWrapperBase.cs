using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.So.Business;

namespace Jurassic.So.Infrastructure
{
    /// <summary>Http客户端包装器基类</summary>
    /// <remarks>
    /// 如果不指定应答结果，
    ///     结果有三种，一种是流，一种是字符串，最后一种是HttpResponseMessage(无法解析ContentType)
    /// 如果指定应答结果，将尝试转换为应答结果，如果无法转换将生成<see cref="System.InvalidOperationException"/>异常
    /// </remarks>
    public class HttpClientWrapperBase : DisposableObject
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public HttpClientWrapperBase()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseProxy = false;
            this.Instance = new HttpClient(handler);
            this.Instance.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            this.Instance.DefaultRequestHeaders.Connection.Add("keep-alive");
            this.Instance.Timeout = TimeSpan.FromSeconds(180);
            this.ReadContentHandlers = new Dictionary<Type, Func<HttpResponseMessage, Task<object>>>();
            InitReadHandlers(this.ReadContentHandlers);
        }
        /// <summary>Http客户端</summary>
        public HttpClient Instance { get; private set; }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                var instance = this.Instance;
                if (instance != null) instance.Dispose();
            }
            this.Instance = null;
        }
        #endregion

        #region 读应答内容方法
        /// <summary>读内容处理器字典</summary>
        private Dictionary<Type, Func<HttpResponseMessage, Task<object>>> ReadContentHandlers { get; set; }
        /// <summary>初始化读内容处理器字典</summary>
        protected virtual void InitReadHandlers(Dictionary<Type, Func<HttpResponseMessage, Task<object>>> handlers)
        {
            handlers[typeof(object)] = ReadAsSelfAsync;
            handlers[typeof(HttpResponseMessage)] = ReadAsSelfAsync;
            handlers[typeof(Stream)] = ReadAsStreamAsync;
            handlers[typeof(byte[])] = ReadAsByteArrayAsync;
            handlers[typeof(string)] = ReadAsStringAsync;
        }
        /// <summary>读内容处理器_返回应答对象</summary>
        private async Task<object> ReadAsSelfAsync(HttpResponseMessage response)
        {
            return await Task.FromResult(response);
        }
        /// <summary>读内容处理器_返回流对象</summary>
        protected async Task<object> ReadAsStreamAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }
        /// <summary>读内容处理器_返回字节数组</summary>
        private async Task<object> ReadAsByteArrayAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }
        /// <summary>读内容处理器_返回字符串</summary>
        protected async Task<object> ReadAsStringAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        /// <summary>读内容处理器</summary>
        protected virtual async Task<object> ReadAsObjectAsync<T>(HttpResponseMessage response)
        {
            return await ReadAsJsonAsync<T>(response);
        }
        /// <summary>读内容处理器_返回JSON强类型对象</summary>
        protected async Task<object> ReadAsJsonAsync<T>(HttpResponseMessage response)
        {
            var mediaType = response.Content.Headers.ContentType.MediaType;
            if (mediaType != MimeTypeConst.JSON)
            {
                throw new HttpRequestException($"MIME[{mediaType}]无法序列化为JSON对象！");
            }
            var content = await ReadAsStringAsync(response);
            return content.ToString().JsonTo<T>();
        }
        #endregion

        #region 检查应答方法
        /// <summary>检查是否成功</summary>
        protected virtual void EnsureSuccess(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                ex.Throw(response);
            }
        }
        /// <summary>是否流应答</summary>
        protected virtual bool IsStreamResponse(HttpResponseMessage response)
        {
            var mediaType = response.Content.Headers.ContentType.MediaType;
            return mediaType.IsStreamOutput();
        }
        /// <summary>读取应答数据</summary>
        private async Task<object> ReadContentAsync(HttpResponseMessage response)
        {
            EnsureSuccess(response);
            var httpContent = response.Content;
            if (IsStreamResponse(response))
            {
                return await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
            }
            return await httpContent.ReadAsStringAsync().ConfigureAwait(false);
        }
        /// <summary>读取应答数据</summary>
        private async Task<T> ReadContentAsync<T>(HttpResponseMessage response)
        {
            EnsureSuccess(response);
            var handler = this.ReadContentHandlers.GetValueBy(typeof(T));
            if (handler == null) handler = ReadAsObjectAsync<T>;
            var result = await handler(response);
            return (T)result;
        }
        #endregion

        #region 授权方法
        /// <summary>设置授权</summary>
        public void SetAuthorization(string token)
        {
            SetAuthorization(token, true);
        }
        /// <summary>设置授权</summary>
        public void SetAuthorization(string token, bool allowClear)
        {
            if (token.IsNullOrEmpty() && allowClear)
            {
                this.Instance.DefaultRequestHeaders.Authorization = null;
            }
            else
            {
                this.Instance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        #endregion

        #region GET方法
        /// <summary>Get请求</summary>
        public object Get(string url, Dictionary<string, object> queryParams = null, string token = null)
        {
            return GetAsync(url, queryParams, token).Result;
        }
        /// <summary>Get请求</summary>
        public async Task<object> GetAsync(string url, Dictionary<string, object> queryParams = null, string token = null)
        {
            SetAuthorization(token, false);
            var url2 = url.GetQueryUrl(queryParams);
            var response = await this.Instance.GetAsync(url2).ConfigureAwait(false);
            return await ReadContentAsync(response);
        }
        /// <summary>Get请求</summary>
        public T Get<T>(string url, Dictionary<string, object> queryParams = null, string token = null)
        {
            return GetAsync<T>(url, queryParams, token).Result;
        }
        /// <summary>Get请求</summary>
        public async Task<T> GetAsync<T>(string url, Dictionary<string, object> queryParams = null, string token = null)
        {
            SetAuthorization(token, false);
            var url2 = url.GetQueryUrl(queryParams);
            var response = await this.Instance.GetAsync(url2).ConfigureAwait(false);
            return await ReadContentAsync<T>(response);
        }
        #endregion

        #region POST方法
        /// <summary>生成请求内容</summary>
        protected virtual HttpContent BuildRequestContent(string content)
        {
            if (content == null) content = string.Empty;
            var httpContent = new StringContent(content);
            var mediaTypeHeaderValue = new MediaTypeHeaderValue(MimeTypeConst.JSON);
            mediaTypeHeaderValue.CharSet = Encoding.UTF8.WebName;
            httpContent.Headers.ContentType = mediaTypeHeaderValue;
            return httpContent;
        }
        /// <summary>序列化内容</summary>
        protected virtual string SerializableContent(object instance)
        {
            if (instance == null) return null;
            return instance.ToJson();
        }
        /// <summary>Post请求</summary>
        public object Post(string url, string content = null, string token = null)
        {
            return PostAsync(url, content, token).Result;
        }
        /// <summary>Post请求</summary>
        public object PostObject(string url, object instance = null, string token = null)
        {
            var content = SerializableContent(instance);
            return PostAsync(url, content, token).Result;
        }
        /// <summary>Post请求</summary>
        public async Task<object> PostAsync(string url, string content = null, string token = null)
        {
            SetAuthorization(token, false);
            var httpContent = BuildRequestContent(content);
            var response = await this.Instance.PostAsync(url, httpContent).ConfigureAwait(false);
            return await ReadContentAsync(response);
        }
        /// <summary>Post请求</summary>
        public async Task<object> PostObjectAsync(string url, object instance = null, string token = null)
        {
            var content = SerializableContent(instance);
            return await PostAsync(url, content, token);
        }
        /// <summary>Post请求</summary>
        public T Post<T>(string url, string content = null, string token = null)
        {
            return PostAsync<T>(url, content, token).Result;
        }
        /// <summary>Post请求</summary>
        public T PostObject<T>(string url, object instance = null, string token = null)
        {
            var content = SerializableContent(instance);
            return PostAsync<T>(url, content, token).Result;
        }
        /// <summary>Post请求</summary>
        public async Task<T> PostAsync<T>(string url, string content = null, string token = null)
        {
            SetAuthorization(token, false);
            var httpContent = BuildRequestContent(content);
            var response = await this.Instance.PostAsync(url, httpContent).ConfigureAwait(false);
            return await ReadContentAsync<T>(response);
        }
        /// <summary>Post请求</summary>
        public async Task<T> PostObjectAsync<T>(string url, object instance = null, string token = null)
        {
            var content = SerializableContent(instance);
            return await PostAsync<T>(url, content, token);
        }
        #endregion

        #region POST上传方法
        /// <summary>生成多部分内容</summary>
        private HttpContent BuildMultipartFormDataContent(string file)
        {
            var httpContent = new MultipartFormDataContent();
            var streamContent = BuildFileContent(file);
            httpContent.Add(streamContent);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            return httpContent;
        }
        /// <summary>生成文件内容</summary>
        private StreamContent BuildFileContent(string file)
        {
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var streamContent = new StreamContent(stream);
            var fileName = Path.GetFileName(file);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                Name = @"""files""",
                FileName = $@"""{fileName}"""
            };
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeConst.Stream);
            return streamContent;
        }
        /// <summary>上传文件</summary>
        public object Upload(string url, string file)
        {
            return UploadAsync(url, file).Result;
        }
        /// <summary>上传文件</summary>
        public async Task<object> UploadAsync(string url, string file)
        {
            var httpContent = BuildMultipartFormDataContent(file);
            var response = await this.Instance.PostAsync(url, httpContent).ConfigureAwait(false);
            return await ReadContentAsync(response);
        }
        /// <summary>上传文件</summary>
        public T Upload<T>(string url, string file)
        {
            return UploadAsync<T>(url, file).Result;
        }
        /// <summary>上传文件</summary>
        public async Task<T> UploadAsync<T>(string url, string file)
        {
            var httpContent = BuildMultipartFormDataContent(file);
            var response = await this.Instance.PostAsync(url, httpContent).ConfigureAwait(false);
            return await ReadContentAsync<T>(response);
        }
        #endregion
    }
}
