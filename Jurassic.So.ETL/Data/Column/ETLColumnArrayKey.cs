using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>列值数组键</summary>
    public class ETLColumnArrayKey : IEquatable<ETLColumnArrayKey>
    {
        /// <summary>构造函数</summary>
        /// <param name="columnValues">列值集合</param>
        public ETLColumnArrayKey(object[] columnValues)
        {
            this.ColumnValues = columnValues;
        }
        /// <summary>列值数组</summary>
        private object[] ColumnValues { get; set; }
        /// <summary>比较两个列值数组键是否相等</summary>
        public override bool Equals(object other)
        {
            return Equals(other.As<ETLColumnArrayKey>());
        }
        /// <summary>比较两个列值数组键是否相等</summary>
        public bool Equals(ETLColumnArrayKey other)
        {
            if (other == null || other.ColumnValues.Length != this.ColumnValues.Length) return false;
            for (int i = 0; i < this.ColumnValues.Length; i++)
            {
                if (!Equals(this.ColumnValues[i], other.ColumnValues[i])) return false;
            }
            return true;
        }
        /// <summary>获得列值数组的散列码</summary>
        public override int GetHashCode()
        {
            var result = 0;
            foreach (var value in this.ColumnValues)
            {
                if (value == null) continue;
                unchecked
                {
                    result = 29 * result + value.GetHashCode();
                }
            }
            return result;
        }
    }
}