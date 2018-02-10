using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.ETL
{
    /// <summary>组件特性</summary>
    /// <remarks>所有组件必须标注本特性，并且具有无参公共构造函数，并且支持接口<see cref="IETLXmlConfig"/></remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ETLComponentAttribute : Attribute
    {
        /// <summary>构造函数</summary>
        public ETLComponentAttribute(string name)
        {
            this.Name = name;
        }
        /// <summary>名称</summary>
        public string Name { get; private set; }
        /// <summary>标题</summary>
        public string Title { get; set; }
        /// <summary>描述</summary>
        public string Description { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
