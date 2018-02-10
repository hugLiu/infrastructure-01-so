using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>列信息</summary>
    public class ETLColumnInfo
    {
        /// <summary>类型</summary>
        public ETLColumnInfoType Type { get; set; }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>获得值</summary>
        public object GetValue(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            switch (this.Type)
            {
                case ETLColumnInfoType.Input: return input[this.Name];
                case ETLColumnInfoType.Output: return output[this.Name];
                case ETLColumnInfoType.Variable: return context.Variables[this.Name];
            }
            return null;
        }
        /// <summary>获得值类型</summary>
        public Type GetType(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            switch (this.Type)
            {
                case ETLColumnInfoType.Input: return input.Columns[this.Name].Type;
                case ETLColumnInfoType.Output: return output.Columns[this.Name].Type;
                case ETLColumnInfoType.Variable: return context.Variables[this.Name].GetType();
            }
            return null;
        }
        /// <summary>设置值</summary>
        public void SetValue(ETLExecuteContext context, IETLRow output, object inputParameter, object value)
        {
            switch (this.Type)
            {
                case ETLColumnInfoType.Output:
                    var column = output.Columns[this.Name];
                    var value2 = value.ETLConvertValue(column.Type);
                    output[this.Name] = value2;
                    break;
                case ETLColumnInfoType.Variable:
                    context.Variables[this.Name] = value;
                    break;
            }
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
