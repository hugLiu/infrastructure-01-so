using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>null值处理策略</summary>
    public enum ETLNullHandlingPolicy
    {
        /// <summary>忽略</summary>
        Ignore,
        /// <summary>需要</summary>
        Required,
        /// <summary>设置</summary>
        Set,
    }
}
