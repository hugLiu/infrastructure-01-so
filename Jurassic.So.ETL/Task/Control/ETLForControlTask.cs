using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>For控制任务</summary>
    public class ETLForControlTask : ETLControlTask
    {
        /// <summary>构造函数</summary>
        public ETLForControlTask() { }
        /// <summary>For表达式</summary>
        public new IETLForExpression Expression
        {
            get { return base.Expression.As<IETLForExpression>(); }
            set { this.Expression = value; }
        }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var expression = this.Expression;
            for (expression.Init(context); expression.Match(context); expression.Next(context))
            {
                var newInputParameter = expression;
                this.WrappedTask.Execute(context, inputRow, inputColumn, newInputParameter);
            }
        }
    }
}
