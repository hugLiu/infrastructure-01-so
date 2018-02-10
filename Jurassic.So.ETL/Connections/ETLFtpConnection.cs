using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jurassic.So.ETL
{
    /// <summary>FTP连接</summary>
    [ETLComponent("FtpConnection", Title = "FTP连接")]
    public class ETLFtpConnection : ETLConnection
    {
        /// <summary>构造函数</summary>
        public ETLFtpConnection() { }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {

        }
        #region 配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
        }
        #endregion
    }
}
