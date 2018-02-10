using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>参数接口，已经放弃使用</summary>
    internal interface IETLParameter
    {
        /// <summary>参数值</summary>
        object Value { get; }
    }
}
