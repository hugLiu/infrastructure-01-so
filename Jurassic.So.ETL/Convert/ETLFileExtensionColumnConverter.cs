using Jurassic.So.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>文件扩展名列转换器</summary>
    [ETLComponent("FileExtension", Title = "文件扩展名列转换器", Description = "获取源列的值中的扩展名并设置到目标列！")]
    public sealed class ETLFileExtensionColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var outputValue = inputValue?.ToString();
            if (!outputValue.IsNullOrEmpty())
            {
                outputValue = Path.GetExtension(outputValue).TrimStart('.');
            }
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
