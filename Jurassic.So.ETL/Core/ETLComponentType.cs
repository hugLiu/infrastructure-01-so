using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jurassic.So.ETL
{
    /// <summary>组件类型</summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ETLComponentType
    {
        /// <summary>连接</summary>
        [EnumMember(Value = "Connection")]
        Connection,
        /// <summary>任务</summary>
        [EnumMember(Value = "Task")]
        Task,
        /// <summary>转换器</summary>
        [EnumMember(Value = "Converter")]
        Converter,
        /// <summary>自定义</summary>
        [EnumMember(Value = "Custom")]
        Custom,
    }
}
