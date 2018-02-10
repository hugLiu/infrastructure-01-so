using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.So.Business
{
    /// <summary>元数据工具</summary>
    public static class MetadataUtil
    {
        /// <summary>计算元数据的IIId</summary>
        public static string ComputeIIId(this IMetadata metadata)
        {
            return metadata.Url.ToMD5();
        }
        /// <summary>转换字典到元数据</summary>
        public static Metadata ToMetadata(this IDictionary<string, object> values)
        {
            return values.As<Metadata>() ?? new Metadata(values);
        }
        /// <summary>转换字典到元数据</summary>
        public static Metadata JsonToMetadata(this JObject value)
        {
            return value.JsonToDictionary(new Metadata());
        }
        /// <summary>转换字典集合到元数据集合</summary>
        public static MetadataCollection ToMetadataList(this IEnumerable<IDictionary<string, object>> values)
        {
            return new MetadataCollection(values.Select(e => e.ToMetadata()));
        }
        /// <summary>转换JObject集合到元数据集合</summary>
        public static MetadataCollection JsonToMetadataList(this IEnumerable<JObject> values)
        {
            return new MetadataCollection(values.Select(e => JsonToMetadata(e)));
        }

        /// <summary>
        /// 转换JObject集合到元数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static MetadataCollection KMDJsonToMetadataList(this IEnumerable<JObject> values)
        {
            return new MetadataCollection(values.Select(s => new KMD(s.ToJson())).ToList());
        }
        /// <summary>
        /// KMD list 转化为元数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static MetadataCollection ToMetadataList(this List<KMD> values)
        {
            return new MetadataCollection(values);
        }
    }
}
