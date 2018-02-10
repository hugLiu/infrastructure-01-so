using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>列</summary>
    public class ETLColumn : IETLColumn
    {
        /// <summary>构造函数</summary>
        public ETLColumn() { }
        /// <summary>名称</summary>
        public string Name { get; set; }
        /// <summary>是否键列</summary>
        public bool IsKey { get; set; }
        /// <summary>类型</summary>
        public Type Type { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
