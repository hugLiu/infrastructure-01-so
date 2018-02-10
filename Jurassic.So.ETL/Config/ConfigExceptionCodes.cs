namespace Jurassic.So.ETL
{
    /// <summary>配置异常代码</summary>
    public enum ConfigExceptionCodes
    {
        /// <summary>连接未发现</summary>
        ConnectionNotFound,
        /// <summary>连接未发现</summary>
        TaskNotFound,
        /// <summary>无效的列信息</summary>
        InvalidColumnInfo,
        /// <summary>无效的列类型</summary>
        InvalidColumnType,
        /// <summary>无效的日期时间值</summary>
        InvalidDateTimeValue,
        /// <summary>无效的查询串输入值</summary>
        InvalidQueryStringInputValue,
        /// <summary>无效的Base64输入值</summary>
        InvalidBase64InputValue,
        /// <summary>不允许null值</summary>
        NullNotAllowed,
    }
}
