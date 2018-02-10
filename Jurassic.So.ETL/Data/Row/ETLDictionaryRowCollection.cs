using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>字典行行集</summary>
    public class ETLDictionaryRowCollection : ETLRowCollection<ETLDictionaryRow>
    {
        /// <summary>构造函数</summary>
        public ETLDictionaryRowCollection()
        {
            this.Columns = new Dictionary<string, IETLColumn>(StringComparer.OrdinalIgnoreCase);
            this.InnerRows = new List<ETLDictionaryRow>();
        }
        /// <summary>构造函数</summary>
        public ETLDictionaryRowCollection(int count) : this()
        {
            for (int i = 0; i < count; i++)
            {
                AddNewRow();
            }
        }
        /// <summary>行集合</summary>
        private List<ETLDictionaryRow> InnerRows { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerRows; }
        }
        /// <summary>生成新行</summary>
        public override ETLDictionaryRow NewRow()
        {
            return new ETLDictionaryRow() { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(ETLDictionaryRow row)
        {
            this.InnerRows.Add(row);
        }
    }
}
