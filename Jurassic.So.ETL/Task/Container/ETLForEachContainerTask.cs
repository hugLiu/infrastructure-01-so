using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>ForEach容器任务</summary>
    [ETLComponent("ForEachContainer", Title = "ForEach容器", Description = "迭代输入行执行多个任务")]
    public class ETLForEachContainerTask : ETLSequenceContainerTask
    {
        /// <summary>构造函数</summary>
        public ETLForEachContainerTask() { }
        /// <summary>迭代器，默认使用上一个任务的输出行集</summary>
        public IEnumerable<IETLRow> Iterator { get; set; }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var iterator = this.Iterator;
            if (iterator == null) iterator = context.InputRows.Rows;
            foreach (var newInputRow in iterator)
            {
                foreach (var task in this.Tasks)
                {
                    task.Execute(context, newInputRow, inputColumn, inputParameter);
                }
            }
        }
    }
}
