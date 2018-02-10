using Jurassic.So.Expression;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>聚合任务</summary>
    public class ETLAggregateTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        public ETLAggregateTask() { }
        /// <summary>分组列数组</summary>
        public IETLColumn[] GroupByColumns { get; set; }
        /// <summary>聚合运算符</summary>
        public AggregateOperator Operator { get; set; }
        /// <summary>内部执行</summary>
        protected override IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            throw new NotImplementedException();
        }
    }
}
