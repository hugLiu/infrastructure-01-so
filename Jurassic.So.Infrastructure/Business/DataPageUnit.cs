using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jurassic.PKS.Service
{
    /// <summary>数据分页单位</summary>
    [Serializable]
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataPageUnit
    {
        /// <summary>无</summary>
        [EnumMember(Value = "")]
        None,
        /// <summary>页数</summary>
        [EnumMember(Value = "page")]
        Page,
        /// <summary>行数</summary>
        [EnumMember(Value = "row")]
        Row,
    }
}
