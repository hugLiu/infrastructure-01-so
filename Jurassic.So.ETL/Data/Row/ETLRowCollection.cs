using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>行集</summary>
    public class ETLRowCollection<TRow> : IETLRowCollection
        where TRow : IETLRow, new()
    {
        /// <summary>构造函数</summary>
        public ETLRowCollection() { }
        /// <summary>列字典</summary>
        public IDictionary<string, IETLColumn> Columns { get; protected set; }
        /// <summary>行集合</summary>
        public virtual IEnumerable<IETLRow> Rows
        {
            get { return null; }
        }
        /// <summary>生成新行</summary>
        IETLRow IETLRowCollection.NewRow()
        {
            return NewRow();
        }
        /// <summary>生成新行</summary>
        public virtual TRow NewRow()
        {
            return new TRow();
        }
        /// <summary>加入行</summary>
        void IETLRowCollection.AddRow(IETLRow row)
        {
            AddRow((TRow)row);
        }
        /// <summary>加入行</summary>
        public virtual void AddRow(TRow row)
        {
            throw new NotImplementedException();
        }
        /// <summary>加入新行</summary>
        public virtual IETLRow AddNewRow()
        {
            var row = NewRow();
            AddRow(row);
            return row;
        }
        /// <summary>加入行集合</summary>
        public virtual void AddRows(IEnumerable<IETLRow> rows)
        {
            foreach (TRow row in rows)
            {
                AddRow(row);
            }
        }
    }
}
