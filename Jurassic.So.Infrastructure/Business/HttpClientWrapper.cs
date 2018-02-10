using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Business
{
    /// <summary>Http客户端包装器</summary>
    /// <remarks>
    /// 如果不指定应答结果，
    ///     结果有三种，一种是流，一种是字符串，最后一种是HttpResponseMessage(无法解析ContentType)
    /// 如果指定应答结果，将尝试转换为应答结果，如果无法转换将生成<see cref="System.InvalidOperationException"/>异常
    /// </remarks>
    public class HttpClientWrapper : HttpClientWrapperBase
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public HttpClientWrapper() { }
        #endregion

        #region 检查应答方法
        /// <summary>检查是否成功</summary>
        protected override void EnsureSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = ReadAsStringAsync(response).Result;
                var mediaType = response.Content.Headers.ContentType.MediaType;
                if (mediaType.Equals(MimeTypeConst.Exception, StringComparison.OrdinalIgnoreCase))
                {
                    var model = content.ToString().JsonTo<ExceptionModel>();
                    if (model != null && !model.Code.IsNullOrEmpty())
                    {
                        throw new UserFriendlyException(model.Code, model.Details, model.Message);
                    }
                }
            }
            base.EnsureSuccess(response);
        }
        /// <summary>初始化读内容处理器字典</summary>
        protected override void InitReadHandlers(Dictionary<Type, Func<HttpResponseMessage, Task<object>>> handlers)
        {
            base.InitReadHandlers(handlers);
            handlers[typeof(DataResult)] = ReadAsDataResultAsync;
        }
        /// <summary>读内容处理器_返回DataResult</summary>
        private async Task<object> ReadAsDataResultAsync(HttpResponseMessage response)
        {
            var httpContent = response.Content;
            var dataResult = new DataResult();
            var mediaType = httpContent.Headers.ContentType.MediaType;
            dataResult.Format = mediaType.ToDataFormatFromMime();
            if (IsStreamResponse(response))
            {
                dataResult.Value = await ReadAsStreamAsync(response);
            }
            else
            {
                dataResult.Value = await ReadAsStringAsync(response);
            }
            return dataResult;
        }
        #endregion
    }
}
