using System.Linq;
using AutoMapper;
using System;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;
using Jurassic.So.Infrastructure;
using System.Collections.Generic;

namespace Jurassic.So.Infrastructure
{
    /// <summary>自动映射工具</summary>
    public static class AutoMapperUtil
    {
        /// <summary>从配置文件加载</summary>
        public static void LoadConfig(string sectionName = "autoMapper")
        {
            var config = ConfigurationManager.GetSection(sectionName).As<NameValueCollection>();
            var assemlies = config.Keys
                .Cast<string>()
                .Select(e => config[e])
                .Where(e => !e.IsNullOrEmpty())
                .ToArray();
            Mapper.Initialize(cfg => cfg.AddProfiles(assemlies));
            Mapper.AssertConfigurationIsValid();
        }
        /// <summary>映射到目标类型数据</summary>
        public static TMapperType MapTo<TMapperType>(this object value)
        {
            return Mapper.Map<TMapperType>(value);
        }
        /// <summary>映射到目标类型数据集合</summary>
        public static IEnumerable<TDestination> MapTo<TDestination>(this IEnumerable<object> values)
        {
            foreach(var value in values)
            {
                yield return value.MapTo<TDestination>();
            }
        }
    }
}
