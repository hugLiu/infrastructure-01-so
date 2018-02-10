using AutoMapper;
using Jurassic.So.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>映射列转换器</summary>
    [ETLComponent("MapperColumn", Title = "映射列转换器", Description = "源列的值可自动映射到目标列！")]
    public sealed class ETLMapperColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var inputType = this.DefaultInputColumn.GetType(context, input, output, inputParameter);
            var outputType = this.DefaultOutputColumn.GetType(context, input, output, inputParameter);
            var outputValue = Mapper.Map(inputValue, inputType, outputType);
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
