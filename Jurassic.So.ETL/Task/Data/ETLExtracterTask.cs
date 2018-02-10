using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>数据提取任务</summary>
    public abstract class ETLExtracterTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        protected ETLExtracterTask() { }
        /// <summary>连接名称</summary>
        public string ConnectionName { get; set; }
        /// <summary>连接</summary>
        protected ETLConnection Connection { get; private set; }
        /// <summary>连接</summary>
        protected object InnerConnection { get; private set; }
        /// <summary>准备执行</summary>
        public override void Prepare(ETLExecuteContext context)
        {
            this.InnerConnection = this.Connection.Open();
        }
        /// <summary>完成执行</summary>
        public override void Finish(ETLExecuteContext context)
        {
            var connection = this.Connection;
            if (connection != null) connection.Close(this.InnerConnection);
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.ConnectionName = config.GetElementValue(node, nameof(this.ConnectionName));
            this.Connection = config.ValidateConnection(this.ConnectionName);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetElementValue(node, nameof(this.ConnectionName), this.ConnectionName);
        }
        #endregion
    }
}
