using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jurassic.PKS.Service.Search;
using MongoDB.Driver;

namespace Jurassic.So.Infrastructure
{
    /// <summary>
    /// Mongodb数据访问
    /// </summary>
    /// <typeparam name="T">数据表对应模型</typeparam>
    public class MongoAccess<T>
    {
        private readonly string _connectionStringDefault = ConfigurationManager.AppSettings["mongo_con"];
        private readonly string _databaseDefault = ConfigurationManager.AppSettings["mongo_database"];
        private readonly string _collectionDefault = ConfigurationManager.AppSettings["mongo_collection"];

        /// <summary>
        /// Mongodb链接字符串
        /// </summary>
        public string ConnectStr;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database;
        /// <summary>
        /// 集合表名称
        /// </summary>
        public string Collection;

        #region 构造函数
        /// <summary>
        /// 无参构造
        /// </summary>
        public MongoAccess()
        {
            ConnectStr = _connectionStringDefault;
            Database = _databaseDefault;
            Collection = _collectionDefault;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="collection">集合名称</param>
        public MongoAccess(string collection)
        {
            ConnectStr = _connectionStringDefault;
            Database = _databaseDefault;
            Collection = collection;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="connectStr">链接字符串</param>
        /// <param name="database">数据库名称</param>
        /// <param name="collection">集合名称</param>
        public MongoAccess(string connectStr, string database, string collection)
        {
            ConnectStr = connectStr;
            Database = database;
            Collection = collection;
        }
        #endregion

        /// <summary>
        /// 获得集合
        /// </summary>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection()
        {
            MongoClient client = new MongoClient(ConnectStr);
            IMongoDatabase database = client.GetDatabase(Database);
            IMongoCollection<T> myCollection = database.GetCollection<T>(Collection);
            return myCollection;
        }
        #region 新增
        /// <summary>
        /// 插入一份文档（异步）
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public async Task InsertAsync(T doc)
        {
            await GetCollection().InsertOneAsync(doc);
        }
        /// <summary>
        /// 插入多份文档（异步）
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public async Task InsertAsync(IEnumerable<T> docs)
        {
            await GetCollection().InsertManyAsync(docs);
        }
        /// <summary>
        /// 插入一份文档
        /// </summary>
        /// <param name="doc"></param>
        public void Insert(T doc)
        {
            GetCollection().InsertOne(doc);
        }
        /// <summary>
        /// 插入多份文档
        /// </summary>
        /// <param name="docs"></param>
        public void Insert(IEnumerable<T> docs)
        {
            GetCollection().InsertMany(docs);
        }
        #endregion
        #region 更新
        /// <summary>
        /// 替换一份文档
        /// </summary>
        /// <param name="filter">过滤找到待替换的文档</param>
        /// <param name="t">新的文档</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public async Task<ReplaceOneResult> ReplaceAsync(Expression<Func<T, bool>> filter, T t, UpdateOptions options = null)
        {
            return await GetCollection().ReplaceOneAsync(filter, t, options);
        }
        public async Task<ReplaceOneResult> ReplaceAsync(FilterDefinition<T> filter, T t, UpdateOptions options = null)
        {
            return await GetCollection().ReplaceOneAsync(filter, t, options);
        }
        /// <summary>
        /// 替换一份文档
        /// </summary>
        /// <param name="filter">过滤找到待替换的文档</param>
        /// <param name="t">新的文档</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public ReplaceOneResult Replace(Expression<Func<T, bool>> filter, T t, UpdateOptions options = null)
        {
            return GetCollection().ReplaceOne(filter, t, options);
        }

        /// <summary>
        /// 替换多份文档
        /// </summary>
        /// <param name="docs">文档集合</param>
        /// <param name="filterBuidler"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task ReplaceManyAsync(IEnumerable<T> docs, Func<T, FilterDefinition<T>> filterBuidler, UpdateOptions options = null)
        {
            var collection = GetCollection();
            foreach (var doc in docs)
            {
                var filter = filterBuidler(doc);
                await collection.ReplaceOneAsync(filter, doc, options);
            }
        }
        /// <summary>
        /// 替换多份文档
        /// </summary>
        /// <param name="docs">文档集合</param>
        /// <param name="filterBuidler"></param>
        /// <param name="options"></param>
        public void ReplaceMany(IEnumerable<T> docs, Func<T, FilterDefinition<T>> filterBuidler, UpdateOptions options = null)
        {
            var collection = GetCollection();
            foreach (var doc in docs)
            {
                var filter = filterBuidler(doc);
                collection.ReplaceOne(filter, doc, options);
            }
        }
        /// <summary>
        /// 更新一份文档（异步）
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新的定义</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            var a = await GetCollection().UpdateOneAsync(filter, update, options);
            return a;// await GetCollection().UpdateOneAsync(filter, update, options);
        }
        /// <summary>
        /// 更新一份文档
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新定义</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public UpdateResult Update(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            return GetCollection().UpdateOne(filter, update, options);
        }
        /// <summary>
        /// 更新多份文档（异步）
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新的定义</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            return await GetCollection().UpdateManyAsync(filter, update, options);
        }
        /// <summary>
        /// 更新多份文档
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新定义</param>
        /// <param name="options">更新选项</param>
        /// <returns></returns>
        public UpdateResult UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            return GetCollection().UpdateMany(filter, update, options);
        }
        #endregion
        #region 删除

        /// <summary>
        /// 删除多份文档（异步）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync(FilterDefinition<T> filter)
        {
            return await GetCollection().DeleteManyAsync(filter);
        }
        /// <summary>
        /// 删除多份文档
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DeleteResult Delete(FilterDefinition<T> filter)
        {
            return GetCollection().DeleteMany(filter);
        }
        /// <summary>
        /// 删除多份文档（异步）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await GetCollection().DeleteManyAsync(filter);
        }
        /// <summary>
        /// 删除多份文档
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DeleteResult Delete(Expression<Func<T, bool>> filter)
        {
            return GetCollection().DeleteMany(filter);
        }
        #endregion
        #region IQuerable
        public IQueryable<T> GetQueryable()
        {
            IMongoCollection<T> myCollection = GetCollection();
            return myCollection.AsQueryable();
        }
        public IAggregateFluent<T> GetAggregateFluent(AggregateOptions opts)
        {
            IMongoCollection<T> myCollection = GetCollection();
            return myCollection.Aggregate(opts);
        }
        #endregion
        #region 查询
        /// <summary>
        /// 查询过滤返回符合条件的一份文档（异步）
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>

        public async Task<T> GetOneAsync(FilterDefinition<T> filter)
        {
            return await GetCollection().Find(filter).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 查询过滤返回符合条件的一份文档（异步）
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="projection">字段投影</param>
        /// <returns></returns>
        public async Task<T> GetOneAsync(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            return await GetCollection().Find(filter).Project<T>(projection).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 查询过滤返回符合条件的一份文档
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T GetOne(FilterDefinition<T> filter)
        {
            return GetCollection().Find(filter).FirstOrDefault();
        }
        /// <summary>
        /// 查询获得多份文档（异步）
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorter">结果排序</param>
        /// <param name="projection">字段投影</param>
        /// <returns></returns>
        public async Task<List<T>> GetManyAsync(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection)
        {
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            var finder = GetCollection().Find(filter);

            if (sorter != null)
            {
                finder.Sort(sorter);
            }
            if (projection != null)
            {
                finder = finder.Project<T>(projection);
            }
            return await finder.ToListAsync();
        }
        /// <summary>
        /// 查询获得多份文档
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorter">结果排序</param>
        /// <param name="projection">字段投影</param>
        /// <returns></returns>
        public List<T> GetMany(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection)
        {
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            var finder = GetCollection().Find(filter);

            if (sorter != null)
            {
                finder.Sort(sorter);
            }
            if (projection != null)
            {
                finder = finder.Project<T>(projection);
            }
            List<T> result = finder.ToList();
            return result;
        }
        /// <summary>
        /// 获得匹配文档个数（异步）
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(FilterDefinition<T> filter)
        {
            var myCollection = GetCollection();

            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            var finder = myCollection.Find(filter);
            return await finder.CountAsync();
        }
        /// <summary>
        /// 获得匹配文档个数
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public long GetCount(FilterDefinition<T> filter)
        {
            var myCollection = GetCollection();

            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            var finder = myCollection.Find(filter);
            return finder.Count();
        }

        /// <summary>
        /// 获得分页查询结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorter">结果排序</param>
        /// <param name="projection">字段投影</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录条数</param>
        /// <returns></returns>
        public List<T> GetPagination(FilterDefinition<T> filter, SortDefinition<T> sorter, ProjectionDefinition<T> projection, int from = 0, int size = 10)
        {
            IFindFluent<T, T> finder = default(IFindFluent<T, T>);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            finder = GetCollection().Find(filter);
            if (sorter != null)
            {
                finder.Sort(sorter);
            }
            if (projection != null)
            {
                finder = finder.Project<T>(projection);
            }
            try
            {
                if (size == 0)
                {
                    return finder.ToList();
                }
                return finder.Skip(from).Limit(size).ToList();
            }
            catch (Exception ex)
            {
                ex.Throw(SearchExceptionCodes.MongoProcessFaild, "Mongo Query Fail!");
                return null;
            }
        }
        /// <summary>
        /// 获得分页查询结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="sorter">结果排序</param>
        /// <param name="projection">字段投影</param>
        /// <param name="from">记录开始</param>
        /// <param name="size">记录条数</param>
        /// <returns></returns>
        public async Task<List<T>> GetPaginationAsync(FilterDefinition<T> filter, SortDefinition<T> sorter = null, ProjectionDefinition<T> projection = null, int from = 0, int size = 10)
        {
            IFindFluent<T, T> finder = default(IFindFluent<T, T>);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            finder = GetCollection().Find(filter);
            if (sorter != null)
            {
                finder.Sort(sorter);
            }
            if (projection != null)
            {
                finder = finder.Project<T>(projection);
            }
            try
            {
                if (size == 0)
                {
                    return await finder.ToListAsync();
                }
                return await finder.Skip(from).Limit(size).ToListAsync();
            }
            catch (Exception ex)
            {
                ex.Throw(SearchExceptionCodes.MongoProcessFaild, "Mongo Query Fail!");
                return null;
            }
        }

        /// <summary>
        /// 获得分页结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="projection">字段投影</param>
        /// <returns></returns>
        public List<T> GetPagination(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            return GetPagination(filter, null, projection, 0, 0);
        }
        /// <summary>
        /// 获得分页结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<T> GetPagination(FilterDefinition<T> filter)
        {
            return GetPagination(filter, null, null);
        }
        /// <summary>
        /// 获得分页结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="projection">字段投影</param>
        /// <returns></returns>
        public async Task<List<T>> GetPaginationAsync(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            return await GetPaginationAsync(filter, null, projection);
        }
        /// <summary>
        /// 获得分页结果
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public async Task<List<T>> GetPaginationAsync(FilterDefinition<T> filter)
        {
            return await GetPaginationAsync(filter, null, null);
        }
        #endregion
        #region 去重

        public async Task<List<string>> DistinctAsync(FieldDefinition<T, string> field, FilterDefinition<T> filter)
        {
            using (var cursor = await GetCollection().DistinctAsync(field, filter))
            {
                return await cursor.ToListAsync();
            }
        }
        public List<string> Distinct(FieldDefinition<T, string> field, FilterDefinition<T> filter)
        {
            using (var cursor = GetCollection().Distinct(field, filter))
            {
                return cursor.ToList();
            }
        }

        public async Task<List<string>> DistinctAsync(Expression<Func<T, string>> field, FilterDefinition<T> filter)
        {
            using (var cursor = await GetCollection().DistinctAsync(field, filter))
            {
                return await cursor.ToListAsync();
            }
        }
        public List<string> Distinct(Expression<Func<T, string>> field, FilterDefinition<T> filter)
        {
            using (var cursor = GetCollection().Distinct(field, filter))
            {
                return cursor.ToList();
            }
        }
        #endregion
    }
}
