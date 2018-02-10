using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库数据行</summary>
    public class ETLDbDataRow : ETLRow
    {
        /// <summary>构造函数</summary>
        public ETLDbDataRow() { }
        /// <summary>构造函数</summary>
        public ETLDbDataRow(DataRow row)
        {
            this.InnerRow = row;
        }
        /// <summary>内部行</summary>
        public DataRow InnerRow { get; set; }
        /// <summary>名称</summary>
        public override object this[IETLColumn column]
        {
            get { return this.InnerRow[column.As<ETLDbDataColumn>().InnerColumn]; }
            set { this.InnerRow[column.As<ETLDbDataColumn>().InnerColumn] = value; }
        }
    }
}
