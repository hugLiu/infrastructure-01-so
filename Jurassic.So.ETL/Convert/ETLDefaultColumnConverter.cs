using Jurassic.So.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>默认列转换器</summary>
    [ETLComponent("Default", Title = "默认列转换器", Description = "设置源列的值到目标列！")]
    public sealed class ETLDefaultColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, inputValue);
        }
    }
}
