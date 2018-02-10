using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.So.Business
{
    /// <summary>数据格式工具</summary>
    public static class DataFormatUtil
    {
        /// <summary>获得数据格式的MIME类型</summary>
        static DataFormatUtil()
        {
            s_FormatMappings = new Dictionary<string, DataFormat>(StringComparer.OrdinalIgnoreCase);
            s_MimeTypeMappings = new Dictionary<DataFormat, MimeTypeAttribute>();
            s_MimeTypeValueMappings = new Dictionary<string, MimeTypeAttribute>(StringComparer.OrdinalIgnoreCase);
            foreach (var fieldInfo in typeof(DataFormat).GetFields())
            {
                if (fieldInfo.IsSpecialName) continue;
                var fvalue = (DataFormat)fieldInfo.GetValue(null);
                s_FormatMappings.Add(fieldInfo.Name, fvalue);
                var fAttributes = fieldInfo.GetCustomAttributes(typeof(MimeTypeAttribute), false);
                if (fAttributes.IsNullOrEmpty()) continue;
                var fAttribute = fAttributes[0].As<MimeTypeAttribute>();
                if (fvalue == DataFormat.Unknown)
                {
                    s_UnknownMimeTypeAttribute = fAttribute;
                }
                s_MimeTypeMappings.Add(fvalue, fAttribute);
                s_MimeTypeValueMappings.Add(fAttribute.Value, fAttribute);
            }
            s_FormatMappings.Add("3GX", DataFormat._3GX);
            s_FormatMappings.Add("jpeg", DataFormat.JPG);
            s_FormatMappings.Add("tiff", DataFormat.TIF);
            s_FormatMappings.Add("icon", DataFormat.ICO);
            s_FormatMappings.Add("htm", DataFormat.HTML);
        }
        /// <summary>数据格式映射表</summary>
        private static Dictionary<string, DataFormat> s_FormatMappings { get; set; }
        /// <summary>MimeType映射表</summary>
        private static Dictionary<DataFormat, MimeTypeAttribute> s_MimeTypeMappings { get; set; }
        /// <summary>未知MimeType</summary>
        private static MimeTypeAttribute s_UnknownMimeTypeAttribute { get; set; }
        /// <summary>MimeType值映射表</summary>
        private static Dictionary<string, MimeTypeAttribute> s_MimeTypeValueMappings { get; set; }
        /// <summary>获得数据格式</summary>
        public static DataFormat ToDataFormat(this string format)
        {
            return s_FormatMappings.GetValueOrDefaultBy(format, DataFormat.Unknown);
        }
        /// <summary>根据MIME类型获得数据格式</summary>
        public static DataFormat ToDataFormatFromMime(this string mediaType)
        {
            var result = s_MimeTypeMappings.FirstOrDefault(e => e.Value.Value.Equals(mediaType, StringComparison.OrdinalIgnoreCase));
            return result.Key;
        }
        /// <summary>获得数据格式的MIME类型</summary>
        public static MimeTypeAttribute ToMimeType(this DataFormat format)
        {
            return s_MimeTypeMappings.GetValueOrDefaultBy(format, s_UnknownMimeTypeAttribute);
        }
        /// <summary>获得数据格式的MIME类型</summary>
        public static string ToMimeTypeValue(this DataFormat format)
        {
            var attribute = ToMimeType(format);
            return attribute.Value;
        }
        /// <summary>MIME类型是否流输出</summary>
        public static bool IsStreamOutput(this string mediaType)
        {
            var attribute = s_MimeTypeValueMappings.GetValueOrDefaultBy(mediaType, s_UnknownMimeTypeAttribute);
            return attribute.IsStreamOutput;
        }
    }
}
