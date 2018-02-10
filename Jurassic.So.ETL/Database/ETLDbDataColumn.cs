using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库数据列</summary>
    public class ETLDbDataColumn : ETLColumn
    {
        /// <summary>构造函数</summary>
        public ETLDbDataColumn(DataColumn column)
        {
            this.InnerColumn = column;
            this.Name = this.InnerColumn.ColumnName;
            this.Type = this.InnerColumn.DataType;
            this.IsKey = this.InnerColumn.Table.PrimaryKey.Contains(this.InnerColumn);
        }
        /// <summary>内部列</summary>
        public DataColumn InnerColumn { get; private set; }
    }
}
