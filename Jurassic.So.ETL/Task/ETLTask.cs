using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>任务，所有任务的基类</summary>
    public abstract class ETLTask : DisposableObject, IETLXmlConfig, ICloneable
    {
        /// <summary>构造函数</summary>
        protected ETLTask() { }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>准备执行</summary>
        public virtual void Prepare(ETLExecuteContext context) { }
        /// <summary>执行</summary>
        public virtual void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            throw new NotImplementedException();
        }
        /// <summary>完成执行</summary>
        public virtual void Finish(ETLExecuteContext context) { }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            //无资源释放
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public virtual void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            this.Name = config.GetAttributeValue_Name(node);
        }
        /// <summary>生成</summary>
        public virtual void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            config.SetAttributeValue_Name(node, this.Name);
        }
        #endregion

        #region 实现克隆接口
        /// <summary>克隆</summary>
        object ICloneable.Clone()
        {
            return Clone();
        }
        /// <summary>克隆</summary>
        public ETLTask Clone()
        {
            var clone = this.MemberwiseClone().As<ETLTask>();
            CloneMembers(clone);
            return clone;
        }
        /// <summary>克隆成员方法</summary>
        protected virtual void CloneMembers(ETLTask clone) { }
        #endregion
    }
}
