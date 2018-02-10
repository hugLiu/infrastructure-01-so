using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库数据表</summary>
    public class ETLDbDataTable : ETLRowCollection<ETLDbDataRow>
    {
        /// <summary>构造函数</summary>
        public ETLDbDataTable() { }
        /// <summary>构造函数</summary>
        public ETLDbDataTable(DataTable table)
        {
            this.InnerTable = table;
            this.Columns = table.Columns
                .Cast<DataColumn>()
                .Select(column => (IETLColumn)new ETLDbDataColumn(column))
                .ToDictionary(column => column.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>数据表</summary>
        protected DataTable InnerTable { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerTable.Rows.Cast<DataRow>().Select(row => new ETLDbDataRow(row) { Columns = this.Columns }); }
        }
        /// <summary>生成新行</summary>
        public override ETLDbDataRow NewRow()
        {
            var row = this.InnerTable.NewRow();
            return new ETLDbDataRow(row) { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(ETLDbDataRow row)
        {
            this.InnerTable.Rows.Add(row.InnerRow);
        }
    }
}
