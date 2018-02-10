using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>转换器</summary>
    [Serializable]
    [DataContract]
    public abstract class ETLConverter : IETLConverter
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        protected ETLConverter() { }
        #endregion

        #region 数据成员
        /// <summary>输入</summary>
        [DataMember]
        public string Input { get; private set; }
        /// <summary>输入列集合</summary>
        [DataMember]
        public List<ETLColumnInfo> InputColumns { get; private set; }
        /// <summary>默认输入列</summary>
        protected ETLColumnInfo DefaultInputColumn
        {
            get { return this.InputColumns[0]; }
        }
        /// <summary>null值处理策略</summary>
        [DataMember]
        public ETLNullHandlingPolicy NullPolicy { get; private set; }
        /// <summary>输出</summary>
        [DataMember]
        public string Output { get; private set; }
        /// <summary>输出列集合</summary>
        [DataMember]
        public List<ETLColumnInfo> OutputColumns { get; private set; }
        /// <summary>默认输出列</summary>
        protected ETLColumnInfo DefaultOutputColumn
        {
            get { return this.OutputColumns[0]; }
        }
        /// <summary>处理模式</summary>
        [DataMember]
        public string Pattern { get; protected set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
        #endregion

        #region 转换方法
        /// <summary>准备执行</summary>
        public virtual void Prepare(ETLExecuteContext context) { }
        /// <summary>执行转换</summary>
        public abstract void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter);
        /// <summary>完成执行</summary>
        public virtual void Finish(ETLExecuteContext context) { }
        /// <summary>验证策略</summary>
        protected bool ValidatePolicy(object inputValue)
        {
            if (inputValue == null || inputValue is DBNull)
            {
                if (this.NullPolicy == ETLNullHandlingPolicy.Ignore) return false;
                if (this.NullPolicy == ETLNullHandlingPolicy.Required)
                {
                    ConfigExceptionCodes.NullNotAllowed.ThrowUserFriendly("不允许null值！", "不允许null值！");
                }
            }
            return true;
        }
        #endregion

        #region 配置方法
        /// <summary>加载</summary>
        public virtual void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            this.NullPolicy = config.GetAttributeValue_Enum(node, nameof(this.NullPolicy).JsonToCamelCase(), ETLNullHandlingPolicy.Ignore);
            this.Input = config.GetAttributeValue(node, nameof(this.Input).ToLower());
            this.InputColumns = this.Input.ETLToColumnInfos();
            this.Pattern = config.GetAttributeValue(node, nameof(this.Pattern).ToLower());
            this.Output = config.GetAttributeValue(node, nameof(this.Output).ToLower());
            this.OutputColumns = this.Output.ETLToColumnInfos();
        }
        /// <summary>生成</summary>
        public virtual void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            if (this.NullPolicy != ETLNullHandlingPolicy.Ignore)
            {
                config.SetAttributeValue(node, nameof(this.NullPolicy).JsonToCamelCase(), this.NullPolicy.ToString());
            }
            if (!this.Input.IsNullOrEmpty())
            {
                config.SetAttributeValue(node, nameof(this.Input).ToLower(), this.Input);
            }
            if (!this.Pattern.IsNullOrEmpty())
            {
                config.SetAttributeValue(node, nameof(this.Pattern).ToLower(), this.Pattern);
            }
            if (!this.Output.IsNullOrEmpty())
            {
                config.SetAttributeValue(node, nameof(this.Output).ToLower(), this.Output);
            }
        }
        #endregion
    }
}
