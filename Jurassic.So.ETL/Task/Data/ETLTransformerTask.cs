using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>数据转换任务</summary>
    public abstract class ETLTransformerTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        protected ETLTransformerTask() { }
        /// <summary>转换器集合</summary>
        public List<IETLConverter> Converters { get; private set; }
        /// <summary>准备执行</summary>
        public override void Prepare(ETLExecuteContext context)
        {
            this.Converters.ForEach(converter => converter.Prepare(context));
        }
        /// <summary>准备输入行集合</summary>
        protected virtual IETLRowCollection PrepareInput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            return context.InputRows;
        }
        /// <summary>准备输出行集合</summary>
        protected virtual IETLRowCollection PrepareOutput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            throw new NotImplementedException();
        }
        /// <summary>内部执行</summary>
        protected override IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var input = PrepareInput(context, inputRow, inputColumn, inputParameter);
            var output = PrepareOutput(context, inputRow, inputColumn, inputParameter);
            foreach (var newInputRow in input.Rows)
            {
                var newOutputRow = output.NewRow();
                foreach (var converter in this.Converters)
                {
                    converter.Execute(context, newInputRow, newOutputRow, inputParameter);
                }
                output.AddRow(newOutputRow);
            }
            return output;
        }
        /// <summary>完成执行</summary>
        public override void Finish(ETLExecuteContext context)
        {
            this.Converters.ForEach(converter => converter.Finish(context));
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.Converters = config.LoadConverters(node);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            node.Add(config.BuildConverters(this.Converters));
        }
        #endregion
    }
}
