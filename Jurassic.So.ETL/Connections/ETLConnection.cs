using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Jurassic.So.ETL
{
    /// <summary>ETL连接，所有ETL连接对象的基类</summary>
    [Serializable]
    [DataContract]
    public abstract class ETLConnection : DisposableObject, IETLXmlConfig
    {
        /// <summary>构造函数</summary>
        protected ETLConnection() { }
        /// <summary>名称</summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>测试连接</summary>
        public virtual void Test()
        {
            var innerConnection = Open();
            Close(innerConnection);
        }
        /// <summary>打开连接</summary>
        public virtual object Open()
        {
            throw new NotImplementedException();
        }
        /// <summary>关闭</summary>
        public virtual void Close(object innerConnection)
        {
            throw new NotImplementedException();
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }

        #region 配置方法
        /// <summary>加载</summary>
        public virtual void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            this.Name = config.GetAttributeValue_Name(node);
        }
        /// <summary>生成</summary>
        public virtual void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            config.SetAttributeValue_Name(node, this.Name);
        }
        #endregion
    }
}
