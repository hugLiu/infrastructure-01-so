using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Jurassic.So.Infrastructure
{
    public class MongoAccessAuto : MongoAccess<BsonDocument>
    {
        public MongoAccessAuto()
        {
        }
        public MongoAccessAuto(string collection) : base(collection)
        {
        }
        public MongoAccessAuto(string connectStr, string database, string collection)
            : base(connectStr, database, collection)
        {
        }
        /// <summary>
        /// 获得集合
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<BsonDocument> GetCollection(string collectionName = null)
        {
            var server = new MongoClient(ConnectStr);
            if (collectionName == null) collectionName = Collection;
            return server.GetDatabase(Database).GetCollection<BsonDocument>(collectionName);
        }

        /// <summary>
        /// 获得聚合结果
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public async Task<List<BsonDocument>> GetAggregateAsync(PipelineDefinition<BsonDocument, BsonDocument> pipeline)
        {
            var aggs = await GetCollection().AggregateAsync(pipeline);
            return await aggs.ToListAsync();
        }
        /// <summary>
        /// 获得聚合结果
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public List<BsonDocument> GetAggregate(PipelineDefinition<BsonDocument, BsonDocument> pipeline)
        {
            return GetCollection().Aggregate(pipeline).ToList();
        }
    }
}

