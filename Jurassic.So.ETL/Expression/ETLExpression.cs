using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>控制表达式</summary>
    public abstract class ETLExpression : IETLExpression
    {
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>是否满足条件</summary>
        public abstract bool Match(ETLExecuteContext context);
    }
}
