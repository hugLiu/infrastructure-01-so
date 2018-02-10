using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Jurassic.So.ETL
{
    /// <summary>执行上下文</summary>
    public sealed class ETLExecuteContext
    {
        /// <summary>构造函数</summary>
        public ETLExecuteContext()
        {
            this.RowsStack = new Stack<IETLRowCollection>();
            this.Connections = new Dictionary<string, ETLConnection>(StringComparer.OrdinalIgnoreCase);
            this.Tasks = new Dictionary<string, ETLTask>(StringComparer.OrdinalIgnoreCase);
            this.Variables = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>行集堆栈</summary>
        private Stack<IETLRowCollection> RowsStack { get; set; }
        /// <summary>当前输入行集</summary>
        public IETLRowCollection InputRows
        {
            get { return this.OutputRows; }
        }
        /// <summary>当前输出行集</summary>
        public IETLRowCollection OutputRows { get; private set; }
        /// <summary>推入输出行集</summary>
        public void Push(ETLDataOutputType outputType, IETLRowCollection outputRows)
        {
            switch (outputType)
            {
                case ETLDataOutputType.New:
                    this.RowsStack.Push(this.OutputRows);
                    this.OutputRows = outputRows;
                    break;
                case ETLDataOutputType.Append:
                    this.OutputRows.AddRows(outputRows.Rows);
                    break;
                case ETLDataOutputType.Ignore:
                    break;
            }
        }
        /// <summary>连接集合</summary>
        public Dictionary<string, ETLConnection> Connections { get; private set; }
        /// <summary>任务集合</summary>
        public Dictionary<string, ETLTask> Tasks { get; private set; }
        /// <summary>变量集合</summary>
        public Dictionary<string, object> Variables { get; private set; }
        /// <summary>日志计时器</summary>
        public Stopwatch LogWatch { get; set; }
        /// <summary>日志键</summary>
        public Guid LogKey { get; set; }
        /// <summary>启动</summary>
        public void StartWatch()
        {
            this.LogWatch = new Stopwatch();
            this.LogKey = Guid.NewGuid();
            ETLExtension.LogInfo($"{this.LogKey}:开始执行");
            this.LogWatch.Start();
        }
        /// <summary>记时</summary>
        public void Watch(string message)
        {
            //ETLExtension.LogInfo($"{this.LogKey}:{message}，共花费{this.LogWatch.Elapsed.TotalSeconds.ToString()}秒！");
        }
        /// <summary>停止</summary>
        public void StopWatch()
        {
            this.LogWatch.Stop();
            ETLExtension.LogInfo($"{this.LogKey}:执行完成，共花费{this.LogWatch.Elapsed.TotalSeconds.ToString()}秒！");
        }
    }
}
