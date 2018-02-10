using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Infrastructure
{
    /// <summary>连接运算符</summary>
    [Serializable]
    [DataContract]
    public enum JoinOperator
    {
        #region 运算符
        /// <summary>内连接</summary>
        Inner,
        /// <summary>左连接</summary>
        Left,
        /// <summary>右连接</summary>
        Right,
        /// <summary>外连接</summary>
        Outer,
        /// <summary>交叉</summary>
        Cross,
        #endregion
    }
}

