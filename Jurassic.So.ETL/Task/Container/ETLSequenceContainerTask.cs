using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>顺序容器任务</summary>
    public class ETLSequenceContainerTask : ETLTask
    {
        /// <summary>构造函数</summary>
        public ETLSequenceContainerTask()
        {
            this.Tasks = new List<ETLTask>();
        }
        /// <summary>数据任务集合</summary>
        public List<ETLTask> Tasks { get; private set; }
        /// <summary>准备执行</summary>
        public override void Prepare(ETLExecuteContext context)
        {
            this.Tasks.ForEach(task => task.Prepare(context));
        }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            foreach (var task in this.Tasks)
            {
                task.Execute(context, inputRow, inputColumn, inputParameter);
                context.Watch(task.Name + "执行完成");
            }
        }
        /// <summary>完成执行</summary>
        public override void Finish(ETLExecuteContext context)
        {
            this.Tasks.ForEach(task => task.Finish(context));
        }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                this.Tasks.ForEach(e => e.Dispose());
            }
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            var tasks = config.LoadTasks(node, nameof(this.Tasks));
            this.Tasks.AddRange(tasks);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            node.Add(config.BuildTasks(this.Tasks, nameof(this.Tasks)));
        }
        #endregion
    }
}
