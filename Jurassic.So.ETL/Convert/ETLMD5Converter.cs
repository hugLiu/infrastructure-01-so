using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>转换器_转换为MD5</summary>
    [ETLComponent("MD5", Title = "MD5转换器", Description = "把输入列的值按MD5计算散列值并设置到输出列")]
    public class ETLMD5Converter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var outputValue = inputValue?.ToString().ToMD5();
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
