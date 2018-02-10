using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using Jurassic.So.Infrastructure.Logging;

namespace Jurassic.So.ETL
{
    /// <summary>XML配置上下文</summary>
    public class ETLXmlConfiguration
    {
        #region XML访问方法
        /// <summary>获得特性值</summary>
        public string GetAttributeValue(XElement node, string name, string defaultValue = "")
        {
            var child = node.Attribute(name);
            return child == null ? defaultValue : child.Value;
        }
        /// <summary>获得特性值相关枚举值</summary>
        public TEnum GetAttributeValue_Enum<TEnum>(XElement node, string name, TEnum defaultValue = default(TEnum))
            where TEnum : struct
        {
            var child = node.Attribute(name);
            return child == null ? defaultValue : child.Value.ToEnum<TEnum>();
        }
        /// <summary>设置特性值</summary>
        public void SetAttributeValue(XElement node, string name, string value)
        {
            node.SetAttributeValue(name, value ?? "");
        }
        /// <summary>获得元素</summary>
        public XElement GetElement(XElement node, string name)
        {
            return node.Element(name);
        }
        /// <summary>获得元素值</summary>
        public string GetElementValue(XElement node, string name, string defaultValue = "")
        {
            var child = node.Element(name);
            return child == null ? defaultValue : child.Value;
        }
        /// <summary>获得元素值相关枚举值</summary>
        public TEnum GetElementValue_Enum<TEnum>(XElement node, string name, TEnum defaultValue = default(TEnum))
            where TEnum : struct
        {
            var child = node.Element(name);
            return child == null ? defaultValue : child.Value.ToEnum<TEnum>();
        }
        /// <summary>获得元素值相关整数值</summary>
        public int GetElementValue_Int32(XElement node, string name, int defaultValue = 0)
        {
            var child = node.Element(name);
            return child == null ? defaultValue : child.Value.ToInt32();
        }
        /// <summary>设置元素值</summary>
        public void SetElementValue(XElement node, string name, string value)
        {
            node.SetElementValue(name, value);
        }
        /// <summary>设置元素CDDATA值</summary>
        public void SetElementValue_CDDATA(XElement node, string name, string value)
        {
            var cddata = new XCData(value);
            var element = new XElement(name, cddata);
            node.Add(element);
        }
        /// <summary>加入元素</summary>
        public XElement AddElement(XElement node, string name)
        {
            var element = new XElement(name);
            node?.Add(element);
            return element;
        }
        /// <summary>加载元素集合</summary>
        public List<T> LoadElements<T>(XElement node, string childName, string grandChildName, Func<XElement, T> loadElement)
        {
            var values = new List<T>();
            var child = node.Element(childName);
            if (child != null)
            {
                IEnumerable<XElement> gchildren;
                if (grandChildName.IsNullOrEmpty())
                {
                    gchildren = child.Elements();
                }
                else
                {
                    gchildren = child.Elements(grandChildName);
                }
                foreach (var gchild in gchildren)
                {
                    var value = loadElement(gchild);
                    values.Add(value);
                }
            }
            return values;
        }
        /// <summary>生成元素集合</summary>
        public XElement BuildElements<T>(IEnumerable<T> values, string nodeName, string childName, Action<XElement, T> buildElement)
        {
            var node = new XElement(nodeName);
            if (values != null)
            {
                foreach (var value in values)
                {
                    var child = new XElement(childName);
                    buildElement(child, value);
                    node.Add(child);
                }
            }
            return node;
        }
        #endregion

        #region 元素共用特性
        /// <summary>元素特性_名称</summary>
        private string XAttribute_Name { get { return "name"; } }
        /// <summary>获取元素特性_名称</summary>
        public string GetAttributeValue_Name(XElement node)
        {
            return GetAttributeValue(node, XAttribute_Name);
        }
        /// <summary>设置元素特性_名称</summary>
        public void SetAttributeValue_Name(XElement node, string value)
        {
            node.SetAttributeValue(XAttribute_Name, value);
        }
        #endregion

        #region 组件方法
        /// <summary>组件映射字典</summary>
        private Dictionary<string, Type> Components { get; set; }
        /// <summary>组件载入委托</summary>
        public Action<object> OnComponentLoaded { get; set; }
        /// <summary>加载运行时组件</summary>
        public void LoadRuntimeComponents()
        {
            this.Components = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            AppDomain.CurrentDomain.GetAssemblies().ForEach(LoadRuntimeComponentsFromAssembly);
        }
        /// <summary>从程序集加载运行时组件</summary>
        private void LoadRuntimeComponentsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(e => !e.IsAbstract && !e.IsGenericType && e.IsClass && e.IsDefined(typeof(ETLComponentAttribute), false));
            foreach (var type in types)
            {
                var attribute = GetComponentAttribute(type);
                var componentType = GetComponentType(type);
                var key = BuildComponentKey(componentType, attribute.Name);
                this.Components.Add(key, type);
            }
        }
        /// <summary>根据类型获得组件特性</summary>
        private ETLComponentAttribute GetComponentAttribute<T>()
        {
            return GetComponentAttribute(typeof(T));
        }
        /// <summary>根据类型获得组件特性</summary>
        private ETLComponentAttribute GetComponentAttribute(Type type)
        {
            return type.GetCustomAttribute<ETLComponentAttribute>(false);
        }
        /// <summary>根据类型获得组件类型</summary>
        private ETLComponentType GetComponentType<T>()
        {
            return GetComponentType(typeof(T));
        }
        /// <summary>根据类型获得组件类型</summary>
        private ETLComponentType GetComponentType(Type type)
        {
            var componentType = ETLComponentType.Custom;
            if (typeof(ETLTask).IsAssignableFrom(type))
            {
                componentType = ETLComponentType.Task;
            }
            else if (typeof(ETLConnection).IsAssignableFrom(type))
            {
                componentType = ETLComponentType.Connection;
            }
            else if (typeof(IETLConverter).IsAssignableFrom(type))
            {
                componentType = ETLComponentType.Converter;
            }
            return componentType;
        }
        /// <summary>生成组件Key</summary>
        private string BuildComponentKey(ETLComponentType componentType, string name)
        {
            return componentType.ToString() + ":" + name;
        }
        /// <summary>加载组件</summary>
        public T LoadComponent<T>(XElement node)
        {
            var key = GetComponentName(node);
            var component = GetComponent<T>(key);
            var config = component.As<IETLXmlConfig>();
            config?.LoadXml(this, node);
            this.OnComponentLoaded?.Invoke(component);
            return component;
        }
        /// <summary>获得组件实例</summary>
        private T GetComponent<T>(string name)
        {
            var componentType = GetComponentType<T>();
            var key = BuildComponentKey(componentType, name);
            var type = this.Components.GetValueBy(key);
            if (type == null)
            {
                throw new KeyNotFoundException($"组件名称[{name}]未找到！");
            }
            return (T)Activator.CreateInstance(type);
        }
        /// <summary>获得组件名称</summary>
        private string GetComponentName(XElement node)
        {
            return node.Name.LocalName;
        }
        /// <summary>加载组件集合</summary>
        public List<T> LoadComponents<T>(XElement node, string childName)
        {
            return LoadElements<T>(node, childName, null, LoadComponent<T>);
        }
        /// <summary>生成组件集合</summary>
        public XElement BuildComponents(string nodeName, IEnumerable<object> components)
        {
            return BuildElements(components, nodeName, "component", BuildComponent);
        }
        /// <summary>生成组件</summary>
        public void BuildComponent(XElement node, object component)
        {
            var attribute = GetComponentAttribute(component.GetType());
            node.Name = attribute.Name;
            var config = component.As<IETLXmlConfig>();
            if (config != null) config.BuildXml(this, node);
        }
        /// <summary>生成组件</summary>
        public XElement BuildComponent(object component)
        {
            var node = new XElement("component");
            BuildComponent(node, component);
            return node;
        }
        #endregion

        #region 连接方法
        /// <summary>连接字典</summary>
        public Dictionary<string, ETLConnection> Connections { get; set; }
        /// <summary>加载连接字典</summary>
        public Dictionary<string, ETLConnection> LoadConnections(XElement node, string nodeName)
        {
            return LoadComponents<ETLConnection>(node, nodeName)
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>生成连接字典</summary>
        public XElement BuildConnections(Dictionary<string, ETLConnection> values, string nodeName)
        {
            return BuildComponents(nodeName, values.Values);
        }
        /// <summary>验证连接</summary>
        public ETLConnection ValidateConnection(string name)
        {
            var value = this.Connections.GetValueBy(name);
            if (value == null)
            {
                ConfigExceptionCodes.ConnectionNotFound.ThrowUserFriendly
                    ($"连接[{name}]在连接集合中未发现！", "连接未发现！");
            }
            return value;
        }
        #endregion

        #region 变量方法
        /// <summary>变量合</summary>
        private string XElementName_Variable { get { return "Variable"; } }
        /// <summary>加载变量字典</summary>
        public Dictionary<string, object> LoadVariables(XElement node, string nodeName)
        {
            return LoadElements(node, nodeName, null, LoadVariable)
                .ToDictionary(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
        }
        /// <summary>加载变量</summary>
        private KeyValuePair<string, object> LoadVariable(XElement node)
        {
            var name = GetComponentName(node);
            if (name.Equals(XElementName_Variable, StringComparison.OrdinalIgnoreCase))
            {
                name = GetAttributeValue_Name(node);
            }
            var value = node.Value;
            return new KeyValuePair<string, object>(name, value);
        }
        /// <summary>生成变量字典</summary>
        public XElement BuildVariables(Dictionary<string, object> variables, string nodeName)
        {
            return BuildElements(variables, nodeName, XElementName_Variable, BuildVariable);
        }
        /// <summary>生成变量</summary>
        private void BuildVariable(XElement node, KeyValuePair<string, object> variable)
        {
            node.Name = variable.Key;
            if (variable.Value != null) node.Value = variable.Value.ToString();
        }
        #endregion

        #region 任务方法
        /// <summary>公共任务</summary>
        public Dictionary<string, ETLTask> CommonTasks { get; set; }
        /// <summary>加载任务集合</summary>
        public List<ETLTask> LoadTasks(XElement node, string xname)
        {
            return LoadComponents<ETLTask>(node, xname);
        }
        /// <summary>处理公共任务加载</summary>
        public void OnCommonTaskLoaded(ETLTask task)
        {
            this.CommonTasks.Add(task.Name, task);
        }
        /// <summary>生成任务集合</summary>
        public XElement BuildTasks(IEnumerable<ETLTask> tasks, string xname)
        {
            return BuildComponents(xname, tasks);
        }
        /// <summary>验证任务</summary>
        public ETLTask ValidateTask(string name)
        {
            var value = this.CommonTasks.GetValueBy(name);
            if (value == null)
            {
                ConfigExceptionCodes.TaskNotFound.ThrowUserFriendly
                    ($"任务[{name}]在公共任务中未发现！", "任务未发现！");
            }
            return value.Clone();
        }
        #endregion

        #region 列方法
        /// <summary>元素名称_列集合</summary>
        private string XElementName_Columns { get { return "Columns"; } }
        /// <summary>元素名称_列</summary>
        private string XElementName_Column { get { return "Column"; } }
        /// <summary>加载列集合</summary>
        public Dictionary<string, IETLColumn> LoadColumns(XElement node)
        {
            return LoadElements<IETLColumn>(node, XElementName_Columns, XElementName_Column, LoadColumn)
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>加载列</summary>
        private IETLColumn LoadColumn(XElement node)
        {
            var column = new ETLColumn();
            column.Name = GetAttributeValue(node, nameof(column.Name).JsonToCamelCase());
            column.Type = GetAttributeValue_Enum(node, nameof(column.Type).JsonToCamelCase(), ETLColumnType.String).ETLToRuntimeType();
            column.IsKey = GetAttributeValue(node, nameof(column.IsKey).JsonToCamelCase(), bool.FalseString).ToBool();
            return column;
        }
        /// <summary>生成列集合</summary>
        public XElement BuildColumns(IDictionary<string, IETLColumn> columns)
        {
            return BuildElements(columns.Values, XElementName_Columns, XElementName_Column, BuildColumn);
        }
        /// <summary>生成列</summary>
        private void BuildColumn(XElement node, IETLColumn column)
        {
            SetAttributeValue(node, nameof(column.Name).JsonToCamelCase(), column.Name);
            SetAttributeValue(node, nameof(column.Type).JsonToCamelCase(), column.Type.ETLToColumnType().ToString());
            if (column.IsKey)
            {
                SetAttributeValue(node, nameof(column.IsKey).JsonToCamelCase(), column.IsKey.ToString());
            }
        }
        #endregion

        #region 参数方法
        /// <summary>元素名称_参数集合</summary>
        private string XElementName_Parameters { get { return "Parameters"; } }
        /// <summary>元素名称_参数</summary>
        private string XElementName_Parameter { get { return "Parameter"; } }
        /// <summary>加载参数集合</summary>
        public List<ETLParameterInfo> LoadParameters(XElement node)
        {
            return LoadElements(node, XElementName_Parameters, XElementName_Parameter, LoadParameter);
        }
        /// <summary>加载参数</summary>
        private ETLParameterInfo LoadParameter(XElement node)
        {
            var info = new ETLParameterInfo();
            info.Name = GetAttributeValue(node, nameof(info.Name).JsonToCamelCase());
            info.Type = GetAttributeValue_Enum(node, nameof(info.Type).JsonToCamelCase(), ETLColumnType.String).ETLToRuntimeType();
            info.Source = GetAttributeValue(node, nameof(info.Source).JsonToCamelCase());
            if (info.Source.IsNullOrEmpty())
            {
                info.Text = node.Value;
                if (!info.Text.IsNullOrEmpty())
                {
                    info.Value = info.Text.ETLConvertValue(info.Type);
                }
            }
            else
            {
                info.SourceColumn = info.Source.ETLToColumnInfo();
            }
            return info;
        }
        /// <summary>生成参数集合</summary>
        public XElement BuildParameters(List<ETLParameterInfo> parameters)
        {
            return BuildElements(parameters, XElementName_Parameters, XElementName_Parameter, BuildParameter);
        }
        /// <summary>生成参数</summary>
        private void BuildParameter(XElement node, ETLParameterInfo info)
        {
            SetAttributeValue(node, nameof(info.Name).JsonToCamelCase(), info.Name);
            SetAttributeValue(node, nameof(info.Type).JsonToCamelCase(), info.Type.ETLToColumnType().ToString());
            SetAttributeValue(node, nameof(info.Source).JsonToCamelCase(), info.Source);
            if (!info.Text.IsNullOrEmpty())
            {
                node.Value = info.Text;
            }
        }
        #endregion

        #region 转换器方法
        /// <summary>转换器_集合_元素名称</summary>
        public static string XElementName_Converters { get { return "Converters"; } }
        /// <summary>加载转换器集合</summary>
        public List<IETLConverter> LoadConverters(XElement node)
        {
            return LoadComponents<IETLConverter>(node, XElementName_Converters);
        }
        /// <summary>生成转换器集合</summary>
        public XElement BuildConverters(List<IETLConverter> converters)
        {
            return BuildComponents(XElementName_Converters, converters);
        }
        #endregion
    }
}
