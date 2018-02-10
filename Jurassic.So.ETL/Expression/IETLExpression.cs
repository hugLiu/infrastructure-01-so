using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>控制表达式接口</summary>
    public interface IETLExpression
    {
        /// <summary>名称</summary>
        string Name { get; set; }
        /// <summary>是否满足循环条件</summary>
        bool Match(ETLExecuteContext context);
    }
}
