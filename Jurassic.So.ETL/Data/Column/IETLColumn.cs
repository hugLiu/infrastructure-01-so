using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>列接口</summary>
    public interface IETLColumn
    {
        /// <summary>名称</summary>
        string Name { get; }
        /// <summary>类型</summary>
        Type Type { get; }
        /// <summary>是否键列</summary>
        bool IsKey { get; }
    }
}
