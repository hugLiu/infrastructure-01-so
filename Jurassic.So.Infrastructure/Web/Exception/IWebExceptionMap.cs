using System;
using System.Collections.Generic;
using System.Net;

namespace Jurassic.So.Web
{
    /// <summary>WEB异常映射</summary>
    public interface IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        string ServiceName { get; }
        /// <summary>配置</summary>
        IDictionary<string, WebExceptionModel> Config();
    }
}
