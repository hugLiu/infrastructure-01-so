using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>转换器_转换为查询串</summary>
    [ETLComponent("QueryStringEncoder", Title = "查询串编码转换器", Description = "把输入列的值编码为查询串并设置到输出列")]
    public class ETLQueryStringEncoderConverter : ETLConverter
    {
        /// <summary>获得键</summary>
        private string GetKey(int index)
        {
            if (index == 0) return "ID";
            return "ID" + index.ToString();
        }
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < this.InputColumns.Count; i++)
            {
                var inputValue = this.InputColumns[i].GetValue(context, input, output, inputParameter);
                if (!ValidatePolicy(inputValue)) return;
                var value = inputValue?.ToString().QueryStringEncode();
                builder.Append(GetKey(i))
                    .Append("=")
                    .Append(value ?? "")
                    .Append("@");
            }
            if (builder.Length > 0) builder.Length -= 1;
            var outputValue = builder.ToString();
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }

    /// <summary>转换器_从查询串转换</summary>
    [ETLComponent("QueryStringDecoder", Title = "查询串解码转换器", Description = "把输入列的值解码为查询串并设置到输出列")]
    public class ETLQueryStringDecoderConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var queryStringArray = inputValue?.ToString().Split('&');
            if (queryStringArray == null || queryStringArray.Length != this.OutputColumns.Count)
            {
                ConfigExceptionCodes.InvalidQueryStringInputValue.ThrowUserFriendly
                    ($"查询串解析列数量[{inputValue}]与输出列数量不一致！", "无效的转换器输出列配置！");
            }
            for (int i = 0; i < this.OutputColumns.Count; i++)
            {
                var pair = queryStringArray[i].Split('=');
                if (pair.Length != 2)
                {
                    ConfigExceptionCodes.InvalidQueryStringInputValue.ThrowUserFriendly
                        ($"查询串解析值[{queryStringArray[i]}]长度应为2！", "无效的转换器输出列配置！");
                }
                var outputValue = pair[1].QueryStringDecode();
                this.OutputColumns[i].SetValue(context, output, inputParameter, outputValue);
            }
        }
    }
}
