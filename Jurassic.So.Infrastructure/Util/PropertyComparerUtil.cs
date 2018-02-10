using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jurassic.So.Infrastructure.Util
{
    /// <summary>
    /// 属性比较
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyComparerUtil<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> _getProertyValueFunc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public PropertyComparerUtil(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName,
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"{propertyName} is not a property of type {typeof(T)}");
            }

            var expPara = System.Linq.Expressions.Expression.Parameter(typeof(T), "obj");
            var me = System.Linq.Expressions.Expression.Property(expPara, propertyInfo);
            _getProertyValueFunc = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(me, expPara).Compile();

        }

        public bool Equals(T x, T y)
        {
            var xValue = _getProertyValueFunc(x);
            var yValue = _getProertyValueFunc(y);

            if (xValue == null)
                return yValue == null;
            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            var propertyValue = _getProertyValueFunc(obj);
            return propertyValue?.GetHashCode() ?? 0;
        }
    }
}
