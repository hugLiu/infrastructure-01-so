using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Jurassic.PKS.Service;
using System.Reflection;
using System.IO;

namespace Jurassic.So.Business
{
    /// <summary>元数据常量</summary>
    public static class MetadataConsts
    {
        #region 叶标签
        /// <summary>索引质量，为百分比格式，默认为"100%"</summary>
        public const string IndexQuality = "indexquality";
        /// <summary>索引ID</summary>
        public const string IIId = "IIId";
        /// <summary>索引日期</summary>
        public const string IndexedDate = "IndexedDate"; 
        /// <summary>URL</summary>
        public const string SourceUrl = "SourceUrl";
        /// <summary>数据源类型</summary>
        public const string SourceType = "SourceType";
        /// <summary>数据源名称</summary>
        public const string SourceName = "SourceName";
        /// <summary>数据源格式</summary>
        public const string SourceFormat = "SourceFormat";
        /// <summary>缩略图</summary>
        public const string Thumbnail = "Thumbnail";
        /// <summary>全文</summary>
        public const string Fulltext = "Fulltext";
        /// <summary>标题</summary>
        public const string FormalTitle = "FormalTitle";
        /// <summary>作者</summary>
        public const string Author = "Author";
        /// <summary>描述</summary>
        public const string Description = "Abstract";
        /// <summary>创建日期</summary>
        public const string CreatedDate = "CreatedDate";
        /// <summary>成果类型</summary>
        public const string ProductType = "ProductType";
        #endregion
    }
}
