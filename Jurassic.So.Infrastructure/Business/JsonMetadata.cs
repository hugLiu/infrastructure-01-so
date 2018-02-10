using Jurassic.So.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using Jurassic.PKS.Service;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

namespace Jurassic.So.Business
{
    /// <summary>JSON元数据</summary>
    [Serializable]
    [DataContract]
    public class JsonMetadata : JObject, IMetadata
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public JsonMetadata() { }
        /// <summary>构造函数</summary>
        public JsonMetadata(JObject value) : base(value) { }
        #endregion

        #region 元数据路径
        /// <summary>元数据路径</summary>
        private class MetadataPath
        {
            /// <summary>路径</summary>
            public string Path { get; set; }
            /// <summary>键</summary>
            public string Key { get; set; }
            /// <summary>标记类型</summary>
            public JTokenType TokenType { get; set; }
            /// <summary>生成JSON串</summary>
            public override string ToString()
            {
                return this.ToJson();
            }
        }
        #endregion

        #region 元数据路径映射
        /// <summary>同步根</summary>
        private static object s_SyncRoot = new object();
        /// <summary>元数据路径映射</summary>
        private static Dictionary<string, List<MetadataPath>> s_MetadataPathMappers;
        /// <summary>初始化元数据路径映射</summary>
        private static Dictionary<string, List<MetadataPath>> InitMetadataPathMappers(KMDConfiguration tags)
        {
            var mappers = new Dictionary<string, List<MetadataPath>>();
            var getRegex = new Regex(@"(?<key>\[[^\]]+\])");
            var setRegex = new Regex(@"^\s*(?<key>[^\[]+)\[\d+\]\s*$");
            foreach (var tag in tags.Values)
            {
                foreach (var setItem in tag.Mapping.Set)
                {
                    var keys = setItem.Key.Split('.').ToList();
                    var mtPaths = new List<MetadataPath>();
                    var prefiex = "";
                    for (var i = 0; i < keys.Count; i++)
                    {
                        var key = keys[i];
                        var jTokenType = (i == keys.Count - 1 ? JTokenType.None : JTokenType.Object);
                        var setMatch = setRegex.Match(key);
                        var key2 = key;
                        if (setMatch.Success)
                        {
                            key2 = setMatch.Groups["key"].Value;
                            jTokenType = JTokenType.Array;
                            var getMatch = getRegex.Match(tag.Mapping.Get);
                            keys.Insert(i + 1, getMatch.Groups["key"].Value);
                        }
                        else if (i == keys.Count - 1)
                        {
                            switch (tag.Type)
                            {
                                case TagType.StringArray:
                                case TagType.Base64StringArray:
                                    jTokenType = JTokenType.Array;
                                    break;
                            }
                        }
                        var mtPath = new MetadataPath();
                        mtPath.Key = key2;
                        prefiex += key2;
                        mtPath.Path = prefiex;
                        mtPath.TokenType = jTokenType;
                        if (i < keys.Count - 1)
                        {
                            var mtPaths2 = mappers.GetValueBy(mtPath.Path);
                            if (mtPaths2 == null)
                            {
                                mappers.Add(mtPath.Path, new List<MetadataPath>() { mtPath });
                            }
                            else
                            {
                                var mtPath2 = mtPaths2[0];
                                if (mtPath.Key != mtPath2.Key || mtPath.TokenType != mtPath2.TokenType)
                                {
                                    throw new Exception($"{mtPath.Path}不匹配！");
                                }
                                mtPath = mtPath2;
                            }
                        }
                        prefiex += ".";
                        mtPaths.Add(mtPath);
                    }
                    mappers.Add(setItem.Key, mtPaths);
                }
            }
            return mappers;
        }
        /// <summary>元数据定义</summary>
        private static KMDConfiguration s_MetadataDefinitions { get; set; }
        /// <summary>初始化元数据定义</summary>
        public static void InitMetadataDefinitions(KMDConfiguration tags)
        {
            lock (s_SyncRoot)
            {
                s_MetadataPathMappers = InitMetadataPathMappers(tags);
                s_MetadataDefinitions = tags;
            }
        }
        /// <summary>获得某个元数据定义</summary>
        private MetadataDefinition GetDefinition(string key)
        {
            var metadataDefinitions = s_MetadataDefinitions;
            if (metadataDefinitions == null)
            {
                InitMetadataDefinitions(KMD.DefaultKmdConfiguration);
                metadataDefinitions = s_MetadataDefinitions;
            }
            return metadataDefinitions[key];
        }
        /// <summary>获得元数据路径映射</summary>
        private static List<MetadataPath> GetMetadataPathMapper(string key)
        {
            var mappers = s_MetadataPathMappers;
            return mappers[key];
        }
        #endregion

        #region IMetadata接口
        /// <summary>索引数据ID</summary>
        [IgnoreDataMember, JsonIgnore]
        public string IIId
        {
            get { return ValueToString(GetValue(MetadataConsts.IIId, false)); }
            set { SetValue(MetadataConsts.IIId, value); }
        }
        /// <summary>索引数据日期</summary>
        [IgnoreDataMember, JsonIgnore]
        public DateTime? IndexedDate
        {
            get { return ValueToDate(GetValue(MetadataConsts.IndexedDate, false)); }
            set { SetValue(MetadataConsts.IndexedDate, value); }
        }
        /// <summary>适配器URL</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Url
        {
            get { return ValueToString(GetValue(MetadataConsts.SourceUrl, false)); }
        }
        /// <summary>正式标题</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Title
        {
            get { return ValueToString(GetValue(MetadataConsts.FormalTitle, false)); }
        }
        /// <summary>描述</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Description
        {
            get { return ValueToString(GetValue(MetadataConsts.Description, false)); }
        }
        /// <summary>创建者</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Creator
        {
            get { return ValueToString(GetValue(MetadataConsts.Author, false)); }
        }
        /// <summary>创建日期</summary>
        [IgnoreDataMember, JsonIgnore]
        public DateTime? CreatedDate
        {
            get { return ValueToDate(GetValue(MetadataConsts.CreatedDate, false)); }
        }
        /// <summary>缩略图</summary>
        [IgnoreDataMember, JsonIgnore]
        public Image Thumbnail
        {
            get { return ValueToImage(GetValue(MetadataConsts.Thumbnail, true)); }
        }
        /// <summary>全文</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Fulltext
        {
            get { return ValueToString(GetValue(MetadataConsts.Fulltext, false)); }
        }
        /// <summary>获得某个元数据值</summary>
        object IMetadata.GetValue(string key)
        {
            return GetValue(key, false);
        }
        /// <summary>转换为索引信息</summary>
        public string ToIndex()
        {
            return this.ToJson();
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
        #endregion

        #region 值访问方法
        /// <summary>获得某个元数据值</summary>
        public object GetValue(string key, bool firstIsArray)
        {
            var definition = GetDefinition(key);
            var keyPath = "$." + definition.Mapping.Get;
            if (firstIsArray) key += "[0]";
            var jToken = this.SelectToken(keyPath);
            if (jToken == null) return null;
            switch (jToken.Type)
            {
                case JTokenType.Array:
                    return ((JArray)jToken).Values().Cast<JValue>()
                        .Select(e => e.Value?.ToString()).ToArray();
                default:
                    var value = ((JValue)jToken).Value;
                    if (value != null && definition.Type == TagType.DateString)
                    {
                        return value.ToString().ToISODate();
                    }
                    return value;
            }
        }
        /// <summary>转换为字符串</summary>
        protected string ValueToString(object value)
        {
            return value?.ToString();
        }
        /// <summary>转换为日期</summary>
        protected DateTime? ValueToDate(object value)
        {
            return (DateTime?)value;
        }
        /// <summary>转换为图片</summary>
        protected Image ValueToImage(object value)
        {
            if (value == null) return null;
            try
            {
                var content = Convert.FromBase64String(value.ToString());
                return Image.FromStream(new MemoryStream(content));
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>设置某个元数据值</summary>
        public void SetValue(string key, object value)
        {
            var definition = GetDefinition(key);
            var setItems = definition.Mapping.Set;
            if (value == null)
            {
                for (var i = setItems.Count - 1; i >= 0; i--)
                {
                    RemoveValueByPath(definition, setItems[i].Key);
                }
                return;
            }
            foreach (var setItem in setItems)
            {
                var value2 = (setItem.Value[0] == '@' ? value : setItem.Value);
                SetValueByPath(definition, setItem.Key, value2);
            }
        }
        /// <summary>设置某个元数据值</summary>
        private void SetValueByPath(MetadataDefinition definition, string keyPath, object value)
        {
            var mtPaths = GetMetadataPathMapper(keyPath);
            JContainer parent = this;
            for (int i = 0; i < mtPaths.Count - 1; i++)
            {
                var mtPath = mtPaths[i];
                var child = (JContainer)JsonGet(parent, mtPath.Key);
                if (child == null)
                {
                    child = JsonCreate(mtPath.TokenType);
                    JsonAdd(parent, mtPath.Key, child);
                }
                parent = child;
            }
            var leaf = mtPaths.Last();
            object value2 = null;
            switch (definition.Type)
            {
                case TagType.StringArray:
                case TagType.Base64StringArray:
                    AddArray(parent, leaf.Key, value);
                    return;
                case TagType.DateString:
                    value2 = ValueFromDate(value);
                    break;
                default:
                    value2 = value;
                    break;
            }
            var jValue = (JValue)JsonGet(parent, leaf.Key);
            if (jValue == null)
            {
                jValue = new JValue(value2);
                JsonAdd(parent, leaf.Key, jValue);
            }
            else
            {
                jValue.Value = value2;
            }
        }
        /// <summary>加入数组</summary>
        private void AddArray(JContainer parent, string key, object value)
        {
            var type = value.GetType();
            IEnumerable<string> items = null;
            if (typeof(ICollection<string>).IsAssignableFrom(type))
            {
                items = value.As<ICollection<string>>();
            }
            else if (typeof(ICollection).IsAssignableFrom(type))
            {
                items = value.As<ICollection>().Cast<object>().Select(e => e.ToString());
            }
            else
            {
                items = new string[] { value.ToString() };
            }
            var jArray = (JArray)JsonGet(parent, key);
            if (jArray == null)
            {
                jArray = new JArray();
                JsonAdd(parent, key, jArray);
            }
            foreach (var item in items)
            {
                if (ExistsInArray(jArray, item)) continue;
                jArray.Add(new JValue(item));
            }
        }
        /// <summary>从日期转换</summary>
        private string ValueFromDate(object value)
        {
            var type = value.GetType();
            if (type == typeof(DateTime))
            {
                return ((DateTime)value).ToISODateString();
            }
            if (type == typeof(DateTime?))
            {
                return ((DateTime?)value).Value.ToISODateString();
            }
            return value.ToString();
        }
        /// <summary>根据路径删除值</summary>
        private void RemoveValueByPath(MetadataDefinition definition, string keyPath)
        {
            var mtPaths = GetMetadataPathMapper(keyPath);
            JToken parent = this;
            for (int i = 0; i < mtPaths.Count; i++)
            {
                var mtPath = mtPaths[i];
                var child = JsonGet(parent, mtPath.Key);
                if (child == null) break;
                if (i == mtPaths.Count - 1)
                {
                    JsonRemove(parent, mtPath.Key, child);
                    break;
                }
                parent = child;
            }
        }
        #endregion

        #region JSON序列化
        /// <summary>使用自定义序列化器</summary>
        public static T JsonTo<T>(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.DateParseHandling = DateParseHandling.None;
            settings.Converters.Add(new JsonMetadataConverter());
            return json.JsonTo<T>(settings);
        }
        /// <summary>
        /// Loads an <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />. 
        /// </summary>
        /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
        /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
        public static JsonMetadata LoadMyself(JsonReader reader)
        {
            return LoadMyself(reader, null);
        }
        /// <summary>
        /// Loads an <see cref="T:Newtonsoft.Json.Linq.JObject" /> from a <see cref="T:Newtonsoft.Json.JsonReader" />. 
        /// </summary>
        /// <param name="reader">A <see cref="T:Newtonsoft.Json.JsonReader" /> that will be read for the content of the <see cref="T:Newtonsoft.Json.Linq.JObject" />.</param>
        /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonLoadSettings" /> used to load the JSON.
        /// If this is null, default load settings will be used.</param>
        /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JObject" /> that contains the JSON that was read from the specified <see cref="T:Newtonsoft.Json.JsonReader" />.</returns>
        public static JsonMetadata LoadMyself(JsonReader reader, JsonLoadSettings settings)
        {
            var value = JObject.Load(reader, settings);
            return new JsonMetadata(value);
        }
        /// <summary>获得子成员</summary>
        private static JToken JsonGet(JToken container, string key)
        {
            switch (container.Type)
            {
                case JTokenType.Object:
                    return container.As<JObject>()[key];
                case JTokenType.Array:
                    return container.SelectToken(key);
                default: throw new NotSupportedException();
            }
        }
        /// <summary>创建容器</summary>
        private static JContainer JsonCreate(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object: return new JObject();
                case JTokenType.Array: return new JArray();
                default: throw new NotSupportedException();
            }
        }
        /// <summary>加入</summary>
        private static void JsonAdd(JContainer container, string key, JToken child)
        {
            switch (container.Type)
            {
                case JTokenType.Object:
                    container.As<JObject>().Add(key, child);
                    break;
                case JTokenType.Array:
                    container.As<JArray>().Add(child);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        /// <summary>删除</summary>
        private static void JsonRemove(JToken parent, string key, JToken child)
        {
            switch (parent.Type)
            {
                case JTokenType.Object:
                    parent.As<JObject>().Remove(key);
                    break;
                case JTokenType.Array:
                    parent.As<JArray>().Remove(child);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        /// <summary>创建容器</summary>
        private static bool ExistsInArray(JArray container, string value)
        {
            foreach (var item in container)
            {
                if (value.Equals(item.As<JValue>().Value.ToString(), StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
        #endregion
    }

    /// <summary>JSON元数据自定义格式化器</summary>
    public class JsonMetadataConverter : CustomCreationConverter<IMetadata>
    {
        #region 序列化
        /// <summary>
        /// Creates an object which will then be populated by the serializer.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>The created object.</returns>
        public override IMetadata Create(Type objectType)
        {
            return new JsonMetadata();
        }
        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            return JsonMetadata.LoadMyself(reader);
        }
        #endregion
    }
}
