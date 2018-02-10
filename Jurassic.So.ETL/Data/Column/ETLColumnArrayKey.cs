using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>��ֵ�����</summary>
    public class ETLColumnArrayKey : IEquatable<ETLColumnArrayKey>
    {
        /// <summary>���캯��</summary>
        /// <param name="columnValues">��ֵ����</param>
        public ETLColumnArrayKey(object[] columnValues)
        {
            this.ColumnValues = columnValues;
        }
        /// <summary>��ֵ����</summary>
        private object[] ColumnValues { get; set; }
        /// <summary>�Ƚ�������ֵ������Ƿ����</summary>
        public override bool Equals(object other)
        {
            return Equals(other.As<ETLColumnArrayKey>());
        }
        /// <summary>�Ƚ�������ֵ������Ƿ����</summary>
        public bool Equals(ETLColumnArrayKey other)
        {
            if (other == null || other.ColumnValues.Length != this.ColumnValues.Length) return false;
            for (int i = 0; i < this.ColumnValues.Length; i++)
            {
                if (!Equals(this.ColumnValues[i], other.ColumnValues[i])) return false;
            }
            return true;
        }
        /// <summary>�����ֵ�����ɢ����</summary>
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