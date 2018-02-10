using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Jurassic.So.Infrastructure
{
    /// <summary>JSON工具</summary>
    public static class JsonUtil
    {

        /// <summary>生成对象缩进的JSON串</summary>
        public static string ToJsonString(this object value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, new IsoDateTimeConverter());
        }
        /// <summary>生成对象非缩进的JSON串</summary>
        public static string ToJsonPlain(this object value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, new IsoDateTimeConverter());
        }
        /// <summary>根据JSON串生成对象</summary>
        public static object JsonTo(this string value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject(value, settings);
        }
        /// <summary>根据JSON串生成对象</summary>
        public static T JsonTo<T>(this string value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
        /// <summary>JObject转换为字典对象</summary>
        public static Dictionary<string, object> JsonToDictionary(this JObject jobject)
        {
            return JsonToDictionary(jobject, new Dictionary<string, object>());
        }
        /// <summary>JObject转换为字典对象</summary>
        public static TDict JsonToDictionary<TDict>(this JObject jobject, TDict values)
            where TDict : IDictionary<string, object>
        {
            foreach (var pair in jobject)
            {
                //values.Add(pair.Key, pair.Value.JsonToObject());
                values.Add(pair.Key, pair.Value);
            }
            return values;
        }
        /// <summary>JObject转换为对象</summary>
        public static object JsonToObject(this JToken jtoken)
        {
            switch (jtoken.Type)
            {
                case JTokenType.Object:
                    var jobject = jtoken.As<JObject>();
                    var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    foreach (var pair in jobject)
                    {
                        dict.Add(pair.Key, JsonToObject(pair.Value));
                    }
                    return dict;
                case JTokenType.Array:
                    var jarray = jtoken.As<JArray>();
                    var list = new List<object>();
                    for (int i = 0; i < jarray.Count; i++)
                    {
                        list.Add(JsonToObject(jarray[i]));
                    }
                    return list;
                case JTokenType.Comment:
                    return null;
                default:
                    var jvalue = jtoken.As<JValue>();
                    return jvalue.Value;
            }
        }
    }
}
