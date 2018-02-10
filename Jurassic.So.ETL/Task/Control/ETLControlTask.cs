using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>控制任务</summary>
    public class ETLControlTask : ETLWrappedTask
    {
        /// <summary>构造函数</summary>
        public ETLControlTask() { }
        /// <summary>表达式</summary>
        public IETLExpression Expression { get; set; }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var expression = this.Expression;
            if (expression.Match(context))
            {
                this.WrappedTask.Execute(context, inputRow, inputColumn, inputParameter);
            }
        }
    }
}
