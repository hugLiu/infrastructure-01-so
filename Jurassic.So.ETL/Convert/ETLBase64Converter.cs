using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>转换器_转换为Base64编码</summary>
    [ETLComponent("Base64Encoder", Title = "Base64编码转换器", Description = "把输入列的值编码为Base64并设置到输出列")]
    public class ETLBase64EncoderConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var outputValue = inputValue?.ToString().Base64Utf8Encode();
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }

    /// <summary>转换器_从Base64编码转换</summary>
    [ETLComponent("Base64Decoder", Title = "Base64解码转换器", Description = "把输入列的值解码为Base64并设置到输出列")]
    public class ETLBase64DecoderConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            object outputValue = null;
            try
            {
                outputValue = inputValue?.ToString().Base64Utf8Decode();
            }
            catch (Exception ex)
            {
                ex.Throw(ConfigExceptionCodes.InvalidBase64InputValue, $"无效的Base64输入值[{inputValue ?? ""}]！");
            }
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
