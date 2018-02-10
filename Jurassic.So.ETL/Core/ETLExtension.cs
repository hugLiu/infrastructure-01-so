using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;
using Jurassic.So.Infrastructure.Logging;

namespace Jurassic.So.ETL
{
    /// <summary>扩展方法</summary>
    public static class ETLExtension
    {
        /// <summary>静态构造函数</summary>
        static ETLExtension()
        {
            InitTypeConverterMappers();
        }

        #region 列类型转换
        /// <summary>转换为运行时类型</summary>
        public static Type ETLToRuntimeType(this ETLColumnType type)
        {
            switch (type)
            {
                case ETLColumnType.String: return typeof(String);
                case ETLColumnType.Guid: return typeof(Guid);
                case ETLColumnType.Boolean: return typeof(Boolean);
                case ETLColumnType.DateTime: return typeof(DateTime);
                case ETLColumnType.DateTimeOffset: return typeof(DateTimeOffset);
                case ETLColumnType.SByte: return typeof(SByte);
                case ETLColumnType.Byte: return typeof(Byte);
                case ETLColumnType.Binary: return typeof(byte[]);
                case ETLColumnType.Int16: return typeof(Int16);
                case ETLColumnType.Int32: return typeof(Int32);
                case ETLColumnType.Int64: return typeof(Int64);
                case ETLColumnType.UInt16: return typeof(UInt16);
                case ETLColumnType.UInt32: return typeof(UInt32);
                case ETLColumnType.UInt64: return typeof(UInt64);
                case ETLColumnType.Single: return typeof(Single);
                case ETLColumnType.Double: return typeof(Double);
                case ETLColumnType.Decimal: return typeof(Decimal);
                case ETLColumnType.Object: return typeof(Object);
                default:
                    ConfigExceptionCodes.InvalidColumnType.ThrowUserFriendly
                        ($"列类型[{type}]无效！", "无效的列类型！");
                    break;
            }
            return null;
        }
        /// <summary>转换为列类型</summary>
        public static ETLColumnType ETLToColumnType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String: return ETLColumnType.String;
                case TypeCode.Boolean: return ETLColumnType.Boolean;
                case TypeCode.DateTime: return ETLColumnType.DateTime;
                case TypeCode.SByte: return ETLColumnType.SByte;
                case TypeCode.Byte: return ETLColumnType.Byte;
                case TypeCode.Int16: return ETLColumnType.Int16;
                case TypeCode.Int32: return ETLColumnType.Int32;
                case TypeCode.Int64: return ETLColumnType.Int64;
                case TypeCode.UInt16: return ETLColumnType.UInt16;
                case TypeCode.UInt32: return ETLColumnType.UInt32;
                case TypeCode.UInt64: return ETLColumnType.UInt64;
                case TypeCode.Single: return ETLColumnType.Single;
                case TypeCode.Double: return ETLColumnType.Double;
                case TypeCode.Decimal: return ETLColumnType.Decimal;
                default:
                    if (type == typeof(Guid)) return ETLColumnType.Guid;
                    if (type == typeof(DateTimeOffset)) return ETLColumnType.DateTimeOffset;
                    if (type == typeof(byte[])) return ETLColumnType.Binary;
                    break;
            }
            return ETLColumnType.Object;
        }
        #endregion

        #region 数据库列类型转换
        /// <summary>转换为数据库类型</summary>
        public static DbType ETLToDbType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return DbType.Boolean;
                case TypeCode.Byte: return DbType.Byte;
                case TypeCode.Char: return DbType.String;
                case TypeCode.DateTime: return DbType.DateTime;
                case TypeCode.Decimal: return DbType.Decimal;
                case TypeCode.Double: return DbType.Double;
                case TypeCode.Int16: return DbType.Int16;
                case TypeCode.Int32: return DbType.Int32;
                case TypeCode.Int64: return DbType.Int64;
                case TypeCode.SByte: return DbType.SByte;
                case TypeCode.Single: return DbType.Single;
                case TypeCode.String: return DbType.String;
                case TypeCode.UInt16: return DbType.UInt16;
                case TypeCode.UInt32: return DbType.UInt32;
                case TypeCode.UInt64: return DbType.UInt64;
                default:
                    if (type == typeof(Guid)) return DbType.Guid;
                    if (type == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
                    if (type == typeof(byte[])) return DbType.Binary;
                    break;
            }
            return DbType.Object;
        }
        #endregion

        #region 列信息转换
        /// <summary>
        /// 列信息类型符号数组,与列信息类型对应
        /// @[名称]表示输入列
        /// #[名称]表示输出列
        /// $[名称]表示变量
        /// </summary>
        private static string[] ColumnSymbols = { "@", "#", "$" };
        /// <summary>正则模式</summary>
        private static readonly string ColumnSymbolPattern = @"^\s*(?<symbol>@|#|\$)\[(?<column>[^\]]+)\]\s*$";
        /// <summary>转换为列信息</summary>
        public static List<ETLColumnInfo> ETLToColumnInfos(this string columnSymbolInfos)
        {
            var values = new List<ETLColumnInfo>();
            var columnSymbolInfoArray = columnSymbolInfos.Split(',');
            foreach (var columnSymbolInfo in columnSymbolInfoArray)
            {
                values.Add(ETLToColumnInfo(columnSymbolInfo));
            }
            return values;
        }
        /// <summary>转换为列信息</summary>
        public static ETLColumnInfo ETLToColumnInfo(this string columnSymbolInfo)
        {
            if (columnSymbolInfo.IsNullOrEmpty()) return null;
            var match = Regex.Match(columnSymbolInfo, ColumnSymbolPattern);
            if (!match.Success)
            {
                ConfigExceptionCodes.InvalidColumnInfo.ThrowUserFriendly
                    ($"列信息[{columnSymbolInfo}]无效！", "无效的列信息值！");
            }
            var symbol = match.Groups["symbol"].Value;
            var column = match.Groups["column"].Value;
            var columnInfo = new ETLColumnInfo();
            columnInfo.Type = GetColumnInfoType(symbol);
            columnInfo.Name = column;
            return columnInfo;
        }
        /// <summary>根据符号获得列信息类型</summary>
        private static ETLColumnInfoType GetColumnInfoType(string symbol)
        {
            var index = Array.IndexOf(ColumnSymbols, symbol);
            return (ETLColumnInfoType)index;
        }
        /// <summary>转换为列信息</summary>
        public static ETLColumnInfo ETLToInputColumnInfo(this IETLColumn column)
        {
            return new ETLColumnInfo() { Type = ETLColumnInfoType.Input, Name = column.Name };
        }
        #endregion

        #region 值转换
        /// <summary>类型转换器映射</summary>
        private static Dictionary<string, Func<object, object>> TypeConverterMappers = new Dictionary<string, Func<object, object>>();
        /// <summary>加入类型转换器映射</summary>
        private static void AddTypeConverterMapper(Type inputType, Type outputType, Func<object, object> converter)
        {
            var key = $"{inputType.ToString()}_{outputType.ToString()}";
            TypeConverterMappers.Add(key, converter);
        }
        /// <summary>获得类型转换器映射</summary>
        private static Func<object, object> GetTypeConverterMapper(Type inputType, Type outputType)
        {
            var key = $"{inputType.ToString()}_{outputType.ToString()}";
            return TypeConverterMappers.GetValueBy(key);
        }
        /// <summary>初始化类型转换器映射</summary>
        private static void InitTypeConverterMappers()
        {
            AddTypeConverterMapper(typeof(DateTime), typeof(String), e => ETLToISOTimeString((DateTime)e));
            AddTypeConverterMapper(typeof(String), typeof(DateTime), e => ETLToLocalTime(e.ToString()));
            AddTypeConverterMapper(typeof(String), typeof(String[]), e => new string[] { e.ToString() });
            AddTypeConverterMapper(typeof(String), typeof(IList<String>), e => new string[] { e.ToString() });
            AddTypeConverterMapper(typeof(String), typeof(List<String>), e => new List<string> { e.ToString() });
            AddTypeConverterMapper(typeof(String[]), typeof(IList<String>), e => e.As<String[]>());
            AddTypeConverterMapper(typeof(String[]), typeof(List<String>), e => new List<string>(e.As<String[]>()));
        }
        /// <summary>转换为枚举</summary>
        public static TEnum ETLToEnum<TEnum>(this object value, TEnum defaultValue = default(TEnum))
            where TEnum : struct
        {
            if (value == null) return defaultValue;
            var svalue = value.ToString();
            if (svalue.Length == 0) return defaultValue;
            return svalue.ToEnum<TEnum>();
        }
        /// <summary>转换到输出类型</summary>
        public static object ETLConvertValue(this object inputValue, Type outputType)
        {
            if (inputValue == null) return null;
            if (outputType == typeof(Object)) return inputValue;
            var inputType = inputValue.GetType();
            if (inputType == outputType) return inputValue;
            try
            {
                var converter = GetTypeConverterMapper(inputType, outputType);
                if (converter != null)
                {
                    return converter(inputValue);
                }
                return Convert.ChangeType(inputValue, outputType);
            }
            catch (Exception)
            {
                var message = $"从[{inputValue.ToString()}]-[{inputValue.GetType().FullName}]转换到[{outputType.FullName}]";
                Logger.Error(message);
                throw;
            }
        }
        #endregion

        #region ISO时间转换
        /// <summary>ISO日期时间格式</summary>
        public static string ISODateTimeFormat = @"yyyy-MM-dd\THH:mm:ss.fff\Z";
        /// <summary>
        /// 将ISO时间字符串转为本地时间<c>DateTime</c>
        /// </summary>
        /// <param name="isoDateTimeString">ISO时间格式的字符串，如：2012-11-02T07:58:51.718Z</param>
        /// <returns><c>代表本地时间的<c>DateTime</c></c></returns>
        public static DateTime ETLToLocalTime(this string isoDateTimeString)
        {
            DateTime dtValue;
            if (!DateTime.TryParseExact(isoDateTimeString, ISODateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dtValue))
            {
                ConfigExceptionCodes.InvalidDateTimeValue.ThrowUserFriendly
                    ($"日期时间值[{isoDateTimeString}]转换为ISO日期时间失败！", "无效的日期时间值！");
            }
            return dtValue.ToLocalTime();
        }
        /// <summary>
        /// 将本地时间转为ISODate格式的字符串
        /// </summary>
        /// <param name="localTime">代表本地时间的<c>DateTime</c></param>
        /// <returns>ISODate格式的字符串，格式如：2012-11-02T07:58:51.718Z</returns>
        public static string ETLToISOTimeString(this DateTime localTime)
        {
            return localTime.ToUniversalTime().ToString(ISODateTimeFormat);
        }
        #endregion

        #region 实体列转换
        /// <summary>解析实体公共属性为实体列集合</summary>
        public static List<ETLEntityColumn> ETLToEntityColumns(this Type entityType, params string[] exclude)
        {
            var properties = entityType.GetProperties();
            var columns = new List<ETLEntityColumn>();
            foreach (var property in properties)
            {
                if (!exclude.IsNullOrEmpty() && exclude.Contains(property.Name)) continue;
                var column = new ETLEntityColumn();
                column.Name = property.Name;
                column.Type = property.PropertyType;
                column.Accessor = property;
                columns.Add(column);
            }
            return columns;
        }
        #endregion

        #region 日志记录
        /// <summary>记录信息</summary>
        public static void LogInfo(string message)
        {
            Logger.Info(message);
        }
        #endregion
    }
}
