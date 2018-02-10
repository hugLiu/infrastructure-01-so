using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Jurassic.So.ETL
{
    /// <summary>数据库连接</summary>
    [ETLComponent("DbConnection", Title = "数据库连接")]
    [DataContract]
    public class ETLDbConnection : ETLConnection
    {
        /// <summary>构造函数</summary>
        public ETLDbConnection() { }
        /// <summary>提供者</summary>
        [DataMember]
        public string ProviderName { get; set; }
        /// <summary>连接串</summary>
        [DataMember]
        public string ConnectionString { get; set; }
        /// <summary>数据库提供者</summary>
        [IgnoreDataMember]
        public DbProviderFactory Provider { get; set; }
        /// <summary>打开连接</summary>
        public override object Open()
        {
            var connectionStringBuilder = this.Provider.CreateConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = this.ConnectionString;
            var connection = this.Provider.CreateConnection();
            connection.ConnectionString = connectionStringBuilder.ConnectionString;
            connection.Open();
            return connection;
        }
        /// <summary>关闭</summary>
        public override void Close(object innerConnection)
        {
            var connection = innerConnection.As<DbConnection>();
            if (connection != null) connection.Close();
        }
        /// <summary>释放资源内部方法</summary>
        protected override void DisposeInternal(bool disposing)
        {
            //无资源释放
        }
        /// <summary>规范化对象名称</summary>
        public string NormalizeObjectName(string objectName)
        {
            switch (this.ProviderName)
            {
                case ETLDbProvider.SqlClient:
                    if (objectName[0] != '[') return $"[{objectName}]";
                    break;
                case ETLDbProvider.OracleClient:
                    if (objectName[0] != '"') return $"\"{objectName.ToUpperInvariant()}\"";
                    return objectName.ToUpperInvariant();
            }
            return objectName;
        }
        /// <summary>规范化参数名称</summary>
        public string NormalizeParameterName(string parameterName)
        {
            switch (this.ProviderName)
            {
                case ETLDbProvider.SqlClient:
                    if (parameterName[0] != '@') return $"@{parameterName}";
                    break;
                case ETLDbProvider.OracleClient:
                    if (parameterName[0] != ':') return $":{parameterName}";
                    break;
            }
            return parameterName;
        }
        /// <summary>生成行号字段</summary>
        public string BuildRowNumberField(string rowNumberColumn)
        {
            switch (this.ProviderName)
            {
                case ETLDbProvider.SqlClient:
                    return "ROW_NUMBER() over ( order by {0} ) AS " + NormalizeObjectName(rowNumberColumn);
                case ETLDbProvider.OracleClient:
                    return "ROW_NUMBER() over ( order by {0} ) " + NormalizeObjectName(rowNumberColumn);
            }
            throw new NotImplementedException();
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.ProviderName = config.GetElementValue(node, nameof(this.ProviderName));
            if (this.Provider == null) this.Provider = DbProviderFactories.GetFactory(this.ProviderName);
            this.ConnectionString = config.GetElementValue(node, nameof(this.ConnectionString));
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetElementValue(node, nameof(this.ProviderName), this.ProviderName);
            config.SetElementValue(node, nameof(this.ConnectionString), this.ConnectionString);
        }
        #endregion
    }
}
