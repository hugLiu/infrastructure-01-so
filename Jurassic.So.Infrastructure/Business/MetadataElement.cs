using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Business
{
    /// <summary>元数据名称元素</summary>
    [Serializable]
    [DataContract]
    public class MetadataNameElement
    {
        /// <summary>类型</summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
        /// <summary>名称</summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    /// <summary>元数据文本元素</summary>
    [Serializable]
    [DataContract]
    public class MetadataTextElement
    {
        /// <summary>类型</summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
        /// <summary>文本</summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    /// <summary>元数据值元素</summary>
    [Serializable]
    [DataContract]
    public class MetadataValueElement<T>
    {
        /// <summary>类型</summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
        /// <summary>值</summary>
        [DataMember(Name = "value")]
        public T Value { get; set; }
    }
}