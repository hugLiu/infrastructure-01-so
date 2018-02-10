using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>ETL字典行</summary>
    public class ETLDictionaryRow : ETLRow
    {
        /// <summary>构造函数</summary>
        public ETLDictionaryRow() : this(StringComparer.OrdinalIgnoreCase) { }
        /// <summary>构造函数</summary>
        public ETLDictionaryRow(StringComparer comparer)
        {
            if (comparer == null) comparer = StringComparer.OrdinalIgnoreCase;
            this.InnerRow = new Dictionary<string, object>(comparer);
        }
        /// <summary>内部行</summary>
        public Dictionary<string, object> InnerRow { get; private set; }
        /// <summary>获得或设置列值</summary>
        public override object this[string name]
        {
            get { return this.InnerRow.GetValueBy(name); }
            set { this.InnerRow[name] = value; }
        }
        /// <summary>根据列获得或设置列值</summary>
        public override object this[IETLColumn column]
        {
            get { return this[column.Name]; }
            set { this[column.Name] = value; }
        }
    }
}
