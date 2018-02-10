using System.Linq;
using AutoMapper;
using System;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using Jurassic.So.Infrastructure;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;
using System.Text;

namespace Jurassic.So.Infrastructure
{
    /// <summary>缓存工具</summary>
    public static class CacheUtil
    {
        /// <summary>缓存实例</summary>
        private static ObjectCache Cache { get; set; } = MemoryCache.Default;
        /// <summary>生成区域相关键</summary>
        private static string BuildRegionKey<T>(string region, string key)
        {
            return BuildRegionKey(region, key, typeof(T));
        }
        /// <summary>生成区域相关键</summary>
        private static string BuildRegionKey(string region, string key, Type valueType)
        {
            var builder = new StringBuilder(128);
            if (region.IsNullOrEmpty()) region = "Default";
            builder.Append(region);
            builder.Append(".");
            if (!key.IsNullOrEmpty())
            {
                builder.Append(key);
                builder.Append(".");
            }
            builder.Append(valueType.FullName);
            return builder.ToString();
        }
        /// <summary>获得实例</summary>
        public static T Get<T>(string key)
        {
            string rkey = BuildRegionKey<T>(null, key);
            return (T)Cache.Get(rkey);
        }
        /// <summary>获得或创建实例</summary>
        public static T GetOrCreate<T>(string key, Func<T> creator, TimeSpan offset)
        {
            string rkey = BuildRegionKey<T>(null, key);
            var value = (T)Cache.Get(rkey);
            if (value == null)
            {
                value = creator();
                if (value != null)
                {
                    var policy = new CacheItemPolicy() { AbsoluteExpiration = new DateTimeOffset(DateTime.Now, offset) };
                    Cache.Set(rkey, value, policy);
                }
            }
            return value;
        }
        /// <summary>获得或创建实例</summary>
        public static T GetOrCreateSliding<T>(string key, Func<T> creator, TimeSpan sliding)
        {
            string rkey = BuildRegionKey<T>(null, key);
            var value = (T)Cache.Get(rkey);
            if (value == null)
            {
                value = creator();
                if (value != null)
                {
                    var policy = new CacheItemPolicy() { SlidingExpiration = sliding };
                    Cache.Set(rkey, value, policy);
                }
            }
            return value;
        }
        /// <summary>加入缓存</summary>
        public static void Add(string key, object value, TimeSpan offset)
        {
            string rkey = BuildRegionKey(null, key, value.GetType());
            var policy = new CacheItemPolicy() { AbsoluteExpiration = new DateTimeOffset(DateTime.Now, offset) };
            Cache.Set(rkey, value, policy);
        }
        /// <summary>加入(滑动)</summary>
        public static void AddSliding(string region, string key, object value, TimeSpan sliding)
        {
            string rkey = BuildRegionKey(region, key, value.GetType());
            var policy = new CacheItemPolicy() { SlidingExpiration = sliding };
            Cache.Set(rkey, value, policy);
        }
    }
}
