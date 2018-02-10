using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>For控制表达式接口</summary>
    public interface IETLForExpression : IETLExpression//, IETLParameter
    {
        /// <summary>循环值</summary>
        object Value { get; }
        /// <summary>初始化</summary>
        void Init(ETLExecuteContext context);
        /// <summary>进入下一步循环</summary>
        void Next(ETLExecuteContext context);
    }
}
