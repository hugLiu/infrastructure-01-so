using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.ETL
{
    /// <summary>数据库命令操作</summary>
    public enum ETLDbCommandOperation
    {
        /// <summary>填充数据表</summary>
        FillTable,
        /// <summary>填充数据集</summary>
        FillDataSet,
        /// <summary>填充表结构</summary>
        FillSchema,
        /// <summary>无结果查询</summary>
        ExecuteNonQuery,
        /// <summary>单值查询</summary>
        ExecuteScalar,
    }
}
