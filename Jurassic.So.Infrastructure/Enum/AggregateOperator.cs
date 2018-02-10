using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Expression
{
    /// <summary>聚合运算符</summary>
    [Serializable]
    [DataContract]
    public enum AggregateOperator
    {
        #region 运算符
        /// <summary>计数</summary>
        Count,
        /// <summary>最大值</summary>
        Max,
        /// <summary>最小值</summary>
        Min,
        /// <summary>平均值</summary>
        Avg,
        /// <summary>和值</summary>
        Sum,
        /// <summary>自定义</summary>
        Custom,
        #endregion
    }
}

