using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>ETL转换器接口</summary>
    public interface IETLConverter : IETLXmlConfig
    {
        /// <summary>准备执行</summary>
        void Prepare(ETLExecuteContext context);
        /// <summary>执行转换</summary>
        void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter);
        /// <summary>完成执行</summary>
        void Finish(ETLExecuteContext context);
    }
}
