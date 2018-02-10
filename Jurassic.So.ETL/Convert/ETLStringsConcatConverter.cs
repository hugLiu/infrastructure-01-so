using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>字符串连接转换器</summary>
    [ETLComponent("StringsConcat", Title = "字符串连接转换器", Description ="把输入列的值按模式连接并设置到输出列")]
    public class ETLStringsConcatConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValues = this.InputColumns.Select(e => e.GetValue(context, input, output, inputParameter)?.ToString()).ToArray();
            foreach (var inputValue in inputValues)
            {
                if (!ValidatePolicy(inputValue)) return;
            }
            var outputValue = this.Pattern.FormatBy(inputValues);
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
