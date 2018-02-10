using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库数据集</summary>
    public class ETLDbDataSet : ETLRowCollection<ETLDbDataRow>
    {
        /// <summary>构造函数</summary>
        public ETLDbDataSet() { }
        /// <summary>构造函数</summary>
        public ETLDbDataSet(DataSet dataSet)
        {
            this.InnerDataSet = dataSet;
            this.Columns = new Dictionary<string, IETLColumn>();
        }
        /// <summary>数据集</summary>
        public DataSet InnerDataSet { get; set; }
    }
}
