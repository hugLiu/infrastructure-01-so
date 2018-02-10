using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>XML配置接口</summary>
    public interface IETLXmlConfig
    {
        /// <summary>加载</summary>
        void LoadXml(ETLXmlConfiguration config, XElement node);
        /// <summary>生成</summary>
        void BuildXml(ETLXmlConfiguration config, XElement node);
    }
}
