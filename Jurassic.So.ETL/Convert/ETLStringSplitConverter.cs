using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>字符串分割转换器</summary>
    [ETLComponent("StringSplit", Title = "字符串分割转换器", Description = "把输入列的值按分隔符分割并设置到输出列")]
    public class ETLStringSplitConverter : ETLConverter
    {
        /// <summary>分隔符</summary>
        private char[] Separators { get; set; }
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var outputValue = inputValue?.ToString().Split(this.Separators);
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }

        #region 配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            if (this.Pattern.IsNullOrEmpty()) this.Pattern = ",";
            this.Separators = this.Pattern.ToArray();
        }
        #endregion
    }
}
