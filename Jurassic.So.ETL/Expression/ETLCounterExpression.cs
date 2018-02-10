using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>For计数控制表达式</summary>
    public class ETLCounterExpression : ETLForExpression<int>
    {
        /// <summary>初值，默认为0</summary>
        public int InitValue { get; set; } = 0;
        /// <summary>最大值</summary>
        public int MaxValue { get; set; }
        /// <summary>计数</summary>
        public int Count
        {
            get { return this.MaxValue + 1; }
            set { this.MaxValue = value - 1; }
        }
        /// <summary>增量值，默认为1</summary>
        public int Increment { get; set; } = 1;
        /// <summary>获得初值</summary>
        protected override int GetInitValue(ETLExecuteContext context)
        {
            return this.InitValue;
        }
        /// <summary>是否满足循环条件</summary>
        public override bool Match(ETLExecuteContext context)
        {
            return this.CurrentValue <= this.MaxValue;
        }
        /// <summary>获得下一个值</summary>
        protected override int GetNextValue(ETLExecuteContext context)
        {
            return this.CurrentValue + this.Increment;
        }
    }
}
