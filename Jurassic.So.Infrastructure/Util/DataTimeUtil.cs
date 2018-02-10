using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace Jurassic.So.Infrastructure
{
    /// <summary>日期时间工具</summary>
    public static class DataTimeUtil
    {
        /// <summary>生成标准业务时间串</summary>
        public static string StandardFormat
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }
        /// <summary>生成标准业务时间串</summary>
        public static string ToStandardString(this DateTime value)
        {
            return value.ToString(StandardFormat);
        }
        /// <summary>生成标准时间</summary>
        public static DateTime? TryParseStandardString(this string value)
        {
            DateTime dtValue;
            if (DateTime.TryParseExact(value, StandardFormat, null, DateTimeStyles.None, out dtValue)) return dtValue;
            return null;
        }
    }
}
