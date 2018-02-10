using Jurassic.So.Infrastructure;
using System;

namespace Jurassic.So.ETL
{
    /// <summary>连接任务</summary>
    public class ETLJoinTask : ETLDataTask
    {
        /// <summary>构造函数</summary>
        public ETLJoinTask() { }
        /// <summary>左输入行集合</summary>
        public IETLRowCollection LeftInputRows { get; set; }
        /// <summary>左输入列数组</summary>
        public IETLColumn[] LeftColumns { get; set; }
        /// <summary>右输入行集合</summary>
        public IETLRowCollection RightInputRows { get; set; }
        /// <summary>右输入列数组</summary>
        public IETLColumn[] RightColumns { get; set; }
        /// <summary>连接运算符</summary>
        public JoinOperator Operator { get; set; }
        /// <summary>内部执行</summary>
        protected override IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            throw new NotImplementedException();
        }
    }
}
