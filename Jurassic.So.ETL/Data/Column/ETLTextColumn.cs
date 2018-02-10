using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>ETL文本列</summary>
    public class ETLTextColumn : ETLColumn
    {
        /// <summary>构造函数</summary>
        public ETLTextColumn()
        {
            this.Type = typeof(string);
        }
        /// <summary>列值</summary>
        public string Value { get; set; }
    }
}
