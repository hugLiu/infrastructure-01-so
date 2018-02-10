using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Jurassic.So.ETL
{
    /// <summary>包，独立的完整的执行过程</summary>
    public sealed class ETLPackage : DisposableObject
    {
        /// <summary>构造函数</summary>
        public ETLPackage()
        {
            this.TaskContainer = new ETLSequenceContainerTask();
        }
        /// <summary>任务容器</summary>
        public ETLSequenceContainerTask TaskContainer { get; private set; }
        /// <summary>执行</summary>
        public IETLRowCollection Execute(ETLExecuteContext context)
        {
            try
            {
                //context.StartWatch();
                this.TaskContainer.Prepare(context);
                //context.Watch("准备完成");
                this.TaskContainer.Execute(context, null, null, null);
                //context.Watch("执行完成");
                //context.StopWatch();
                return context.OutputRows;
            }
            finally
            {
                this.TaskContainer.Finish(context);
            }
        }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                this.TaskContainer.Dispose();
            }
        }
    }
}
