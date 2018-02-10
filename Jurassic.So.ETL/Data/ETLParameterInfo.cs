using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>参数信息</summary>
    public class ETLParameterInfo
    {
        /// <summary>构造函数</summary>
        public ETLParameterInfo() { }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>类型</summary>
        public Type Type { get; set; }
        /// <summary>来源</summary>
        public string Source { get; set; }
        /// <summary>来源列</summary>
        public ETLColumnInfo SourceColumn { get; set; }
        /// <summary>文本值</summary>
        public string Text { get; set; }
        /// <summary>参数值</summary>
        public object Value { get; set; }
        /// <summary>获得值</summary>
        public object GetValue(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var result = this.Value;
            if (result == null && this.SourceColumn != null)
            {
                result = this.SourceColumn.GetValue(context, inputRow, null, inputParameter);
            }
            return result.ETLConvertValue(this.Type);
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
