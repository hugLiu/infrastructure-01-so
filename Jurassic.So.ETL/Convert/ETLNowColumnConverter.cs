using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>当前时间列转换器</summary>
    [ETLComponent("Now", Title = "当前时间列转换器", Description = "设置当前时间值到输出列")]
    public class ETLNowColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var outputValue = DateTime.Now;
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
