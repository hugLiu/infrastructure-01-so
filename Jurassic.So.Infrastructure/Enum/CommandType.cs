using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Infrastructure
{
    /// <summary>命令类型</summary>
    [Serializable]
    public enum CommandType
    {
        /// <summary>插入</summary>
        Insert,
        /// <summary>更新</summary>
        Update,
        ///// <summary>更新或插入</summary>
        //Upsert,
        /// <summary>删除</summary>
        Delete,
    }
}
