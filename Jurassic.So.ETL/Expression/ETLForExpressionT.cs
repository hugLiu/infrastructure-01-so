using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>For控制表达式</summary>
    public abstract class ETLForExpression<T> : ETLExpression, IETLForExpression
    {
        /// <summary>当前值</summary>
        protected T CurrentValue { get; set; }
        /// <summary>循环值</summary>
        public object Value
        {
            get { return this.CurrentValue; }
        }
        /// <summary>初始化</summary>
        public void Init(ETLExecuteContext context)
        {
            this.CurrentValue = GetInitValue(context);
        }
        /// <summary>获得初值</summary>
        protected abstract T GetInitValue(ETLExecuteContext context);
        /// <summary>进入下一步循环</summary>
        public virtual void Next(ETLExecuteContext context)
        {
            this.CurrentValue = GetNextValue(context);
        }
        /// <summary>获得下一个值</summary>
        protected abstract T GetNextValue(ETLExecuteContext context);
    }
}
