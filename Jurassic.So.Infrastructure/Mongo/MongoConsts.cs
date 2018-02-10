using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jurassic.So.Infrastructure
{
    /// <summary>Mongo库常量</summary>
    public static class MongoConsts
    {
        /// <summary>元数据表</summary>
        public static readonly string Collection_MetaData = "MetaData";
        /// <summary>元数据定义表</summary>
        public static readonly string Collection_MetaDataDefinition = "MetaDataDefinition";
    }
}
