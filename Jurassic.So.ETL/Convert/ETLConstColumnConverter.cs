using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>常数列转换器</summary>
    [ETLComponent("Const", Title = "常数列转换器", Description = "设置常数值到输出列")]
    public class ETLConstColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.Pattern;
            if (!ValidatePolicy(inputValue)) return;
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, inputValue);
        }
    }
}
