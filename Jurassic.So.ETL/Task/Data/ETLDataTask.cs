using Jurassic.So.Expression;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>数据任务，处理数据任务的基类</summary>
    public abstract class ETLDataTask : ETLTask
    {
        /// <summary>构造函数</summary>
        protected ETLDataTask() { }
        /// <summary>输出类型</summary>
        public ETLDataOutputType OutputType { get; set; }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var output = ExecuteInternal(context, inputRow, inputColumn, inputParameter);
            context.Push(this.OutputType, output);
        }
        /// <summary>内部执行</summary>
        protected virtual IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            throw new NotImplementedException();
        }
        /// <summary>推入输出</summary>
        protected void PushOutput(ETLExecuteContext context, IETLRowCollection output)
        {
            context.Push(this.OutputType, output);
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.OutputType = config.GetAttributeValue_Enum(node, nameof(this.OutputType), ETLDataOutputType.New);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetAttributeValue(node, nameof(this.OutputType), this.OutputType.ToString());
        }
        #endregion
    }
}
