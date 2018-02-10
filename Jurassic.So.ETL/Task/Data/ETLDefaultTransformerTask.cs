using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>默认转换任务</summary>
    [ETLComponent("DefaultTransformerTask", Title = "默认转换任务", Description = "使用预定义列字典的字典行集合执行转换任务")]
    public class ETLDefaultTransformerTask : ETLTransformerTask
    {
        /// <summary>构造函数</summary>
        public ETLDefaultTransformerTask() { }
        /// <summary>列字典</summary>
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>准备输入行集合</summary>
        protected override IETLRowCollection PrepareInput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var input = base.PrepareInput(context, inputRow, inputColumn, inputParameter);
            if (input != null) return input;
            return new ETLDictionaryRowCollection(1);
        }
        /// <summary>准备输出行集合</summary>
        protected override IETLRowCollection PrepareOutput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var output = new ETLDictionaryRowCollection();
            output.Columns.AddRange(this.Columns);
            return output;
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.Columns = config.LoadColumns(node);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            node.Add(config.BuildColumns(this.Columns));
        }
        #endregion
    }
}
