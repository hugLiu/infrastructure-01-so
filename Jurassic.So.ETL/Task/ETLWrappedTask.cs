using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>包装任务</summary>
    [ETLComponent("WrappedTask", Title = "包装任务")]
    public class ETLWrappedTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        public ETLWrappedTask() { }
        /// <summary>引用任务名称</summary>
        public string RefName { get; set; }
        /// <summary>任务</summary>
        protected ETLTask WrappedTask { get; set; }
        /// <summary>准备执行</summary>
        public override void Prepare(ETLExecuteContext context)
        {
            this.WrappedTask.Prepare(context);
        }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            this.WrappedTask.Execute(context, inputRow, inputColumn, inputParameter);
        }
        /// <summary>完成执行</summary>
        public override void Finish(ETLExecuteContext context)
        {
            this.WrappedTask.Finish(context);
        }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                var task = this.WrappedTask;
                if (task != null) task.Dispose();
            }
            this.WrappedTask = null;
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.RefName = config.GetAttributeValue(node, nameof(this.RefName).JsonToCamelCase());
            this.WrappedTask = config.ValidateTask(this.RefName);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetAttributeValue(node, nameof(this.RefName).JsonToCamelCase(), this.RefName);
        }
        #endregion
    }
}
