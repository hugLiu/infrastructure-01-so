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
    /// <summary>数据任务输出类型</summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ETLDataOutputType
    {
        /// <summary>新数据，默认值</summary>
        [EnumMember(Value = "New")]
        New,
        /// <summary>添加</summary>
        [EnumMember(Value = "Append")]
        Append,
        /// <summary>忽略</summary>
        [EnumMember(Value = "Ignore")]
        Ignore,
    }
}
