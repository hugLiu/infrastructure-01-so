using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Jurassic.So.Infrastructure
{
    /// <summary>Mongo工具</summary>
    public static class MongoUtil
    {
        /// <summary>Mongo文档转换为强类型字典</summary>
        public static TDictionary ToDictionary<TDictionary>(this BsonDocument doc)
            where TDictionary : class, IDictionary<string, object>
        {
            var options = new BsonTypeMapperOptions
            {
                DuplicateNameHandling = DuplicateNameHandling.ThrowException,
                MapBsonArrayTo = typeof(object[]),
                MapBsonDocumentTo = typeof(TDictionary),
                MapOldBinaryToByteArray = false
            };
            return BsonTypeMapper.MapToDotNetValue(doc, options).As<TDictionary>();
        }
    }
}
