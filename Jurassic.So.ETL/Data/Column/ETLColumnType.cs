using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>列类型</summary>
    [Description("列类型")]
    public enum ETLColumnType
    {
        /// <summary>字符串</summary>
        [Description("字符串")]
        String,
        /// <summary>全局唯一标识符</summary>
        [Description("GUID")]
        Guid,
        /// <summary>布尔值</summary>
        [Description("布尔值")]
        Boolean,
        /// <summary>日期和时间值</summary>
        [Description("日期时间值")]
        DateTime,
        /// <summary>显示时区的日期和时间值</summary>
        [Description("显示时区的日期时间值")]
        DateTimeOffset,
        /// <summary>有符号 8 位整数</summary>
        [Description("有符号字节")]
        SByte,
        /// <summary>8 位无符号整数</summary>
        [Description("字节")]
        Byte,
        /// <summary>二进制数据</summary>
        [Description("字节流")]
        Binary,
        /// <summary>有符号 16 位整数</summary>
        [Description("16位整数")]
        Int16,
        /// <summary>有符号 32 位整数</summary>
        [Description("32位整数")]
        Int32,
        /// <summary>有符号 64 位整数</summary>
        [Description("64位整数")]
        Int64,
        /// <summary>无符号 16 位整数</summary>
        [Description("16位无符号整数")]
        UInt16,
        /// <summary>无符号 32 位整数</summary>
        [Description("32位无符号整数")]
        UInt32,
        /// <summary>无符号 64 位整数</summary>
        [Description("64位无符号整数")]
        UInt64,
        /// <summary>浮点数</summary>
        [Description("浮点数")]
        Single,
        /// <summary>浮点数</summary>
        [Description("高精度浮点数")]
        Double,
        /// <summary>实数</summary>
        [Description("实数")]
        Decimal,
        /// <summary>表示任何没有由其他值显式表示的引用或值类型</summary>
        [Description("对象")]
        Object,
    }
}
