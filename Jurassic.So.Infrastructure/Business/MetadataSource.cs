using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Business
{
    /// <summary>元数据源信息</summary>
    [Serializable]
    [DataContract]
    public class MetadataSource
    {
        /// <summary>适配器地址</summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>数据源名称</summary>
        [DataMember(Name = "name")]
        public string DataSourceName { get; set; }
        /// <summary>数据源类型</summary>
        [DataMember(Name = "type")]
        public string DataSourceType { get; set; }
        /// <summary>格式</summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }
        /// <summary>存储</summary>
        [DataMember(Name = "media")]
        public string Media { get; set; }
    }
}