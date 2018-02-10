using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>ETL行接口</summary>
    public interface IETLRow
    {
        /// <summary>列字典</summary>
        IDictionary<string, IETLColumn> Columns { get; }
        /// <summary>根据列名获得或设置列值</summary>
        object this[string name] { get; set; }
        /// <summary>根据列获得或设置列值</summary>
        object this[IETLColumn column] { get; set; }
    }
}
