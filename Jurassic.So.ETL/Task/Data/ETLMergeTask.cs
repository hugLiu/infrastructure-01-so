using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据合并任务</summary>
    /// <remarks>根据多个输出行生成输出行集</remarks>
    public class ETLMergeTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        public ETLMergeTask() { }
        /// <summary>输出任务</summary>
        public ETLTask OutputTask { get; set; }
        /// <summary>内部执行</summary>
        protected override IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            return null;
        }
    }
}
