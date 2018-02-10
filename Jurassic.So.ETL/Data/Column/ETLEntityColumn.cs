using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>实体列</summary>
    public class ETLEntityColumn : ETLColumn
    {
        /// <summary>构造函数</summary>
        public ETLEntityColumn() { }
        /// <summary>属性访问器</summary>
        public PropertyInfo Accessor { get; set; }
    }
}
