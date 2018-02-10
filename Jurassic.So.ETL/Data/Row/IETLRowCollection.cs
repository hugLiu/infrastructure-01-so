using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>行集接口</summary>
    public interface IETLRowCollection //: IETLParameter
    {
        /// <summary>列字典</summary>
        IDictionary<string, IETLColumn> Columns { get; }
        /// <summary>行集合</summary>
        IEnumerable<IETLRow> Rows { get; }
        /// <summary>生成新行</summary>
        IETLRow NewRow();
        /// <summary>加入行</summary>
        void AddRow(IETLRow row);
        /// <summary>加入行集合</summary>
        void AddRows(IEnumerable<IETLRow> rows);
    }
}
