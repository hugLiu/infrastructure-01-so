using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>行，所有行对象的基类</summary>
    public abstract class ETLRow : IETLRow, ICloneable, IEquatable<ETLRow>
    {
        /// <summary>构造函数</summary>
        protected ETLRow() { }
        /// <summary>列字典</summary>
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>根据列名获得或设置列值</summary>
        public virtual object this[string name]
        {
            get { return this[this.Columns[name]]; }
            set { this[this.Columns[name]] = value; }
        }
        /// <summary>根据列获得或设置列值</summary>
        public virtual object this[IETLColumn column]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #region 键列
        /// <summary>键列集合</summary>
        public virtual IETLColumn[] KeyColumns
        {
            get { return this.Columns.Values.Where(e => e.IsKey).ToArray(); }
        }
        /// <summary>键列值集合</summary>
        public virtual object[] KeyColumnsValue
        {
            get { return GetKeyColumnsValue(this.KeyColumns); }
        }
        /// <summary>获得键列值集合</summary>
        protected object[] GetKeyColumnsValue(IETLColumn[] keyColumns)
        {
            return keyColumns.Select(e => this[e]).ToArray();
        }
        #endregion

        #region 实现克隆接口
        /// <summary>克隆</summary>
        object ICloneable.Clone()
        {
            return Clone();
        }
        /// <summary>克隆</summary>
        public ETLRow Clone()
        {
            var clone = this.MemberwiseClone().As<ETLRow>();
            CloneMembers(clone);
            return clone;
        }
        /// <summary>克隆成员方法</summary>
        protected virtual void CloneMembers(ETLRow clone) { }
        #endregion

        #region 实现相等接口
        /// <summary>是否相等</summary>
        public override bool Equals(object other)
        {
            return Equals(other.As<ETLRow>());
        }
        /// <summary>是否相等</summary>
        public bool Equals(IETLRow other)
        {
            return Equals(other.As<ETLRow>());
        }
        /// <summary>是否相等</summary>
        public bool Equals(ETLRow other)
        {
            if (this == other) return true;
            if (other == null) return false;
            if (this.GetType() != other.GetType()) return false;
            return ValuesEquals(other);
        }
        /// <summary>值集合是否相等</summary>
        protected virtual bool ValuesEquals(ETLRow other)
        {
            var keyColumns = this.KeyColumns;
            var thisItems = GetKeyColumnsValue(keyColumns);
            var otherItems = other.GetKeyColumnsValue(keyColumns);
            for (int i = 0; i < thisItems.Length; i++)
            {
                if (!Equals(thisItems[i], otherItems[i])) return false;
            }
            return true;
        }
        /// <summary>获得散列值</summary>
        public override int GetHashCode()
        {
            var thisItems = this.KeyColumnsValue;
            var result = 0;
            foreach (var value in thisItems)
            {
                if (value == null) continue;
                unchecked
                {
                    result = 29 * result + value.GetHashCode();
                }
            }
            return result;
        }
        #endregion
    }
}
