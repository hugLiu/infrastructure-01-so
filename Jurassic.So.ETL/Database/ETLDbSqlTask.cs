using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Jurassic.So.ETL
{
    /// <summary>数据库SQL任务</summary>
    [ETLComponent("SqlTask", Title = "SQL任务")]
    [DataContract]
    public class ETLDbSqlTask : ETLExtracterTask
    {
        /// <summary>构造函数</summary>
        public ETLDbSqlTask() { }
        /// <summary>命令类型</summary>
        [DataMember]
        public System.Data.CommandType CommandType { get; set; }
        /// <summary>命令文本</summary>
        [DataMember]
        public string CommandText { get; set; }
        /// <summary>命令超时（以秒为单位）</summary>
        [DataMember]
        public int CommandTimeout { get; set; }
        /// <summary>命令操作</summary>
        [DataMember]
        public ETLDbCommandOperation CommandOperation { get; set; }
        /// <summary>参数信息集合</summary>
        [DataMember]
        public List<ETLParameterInfo> Parameters { get; set; }
        /// <summary>注入命令</summary>
        [IgnoreDataMember]
        public Action<DbCommand, ETLExecuteContext, IETLRow, IETLColumn, object> InjectCommand { get; set; }
        /// <summary>数据库连接</summary>
        private ETLDbConnection ETLDbConnection
        {
            get { return base.Connection.As<ETLDbConnection>(); }
        }
        /// <summary>数据库连接</summary>
        private DbConnection DbConnection
        {
            get { return base.InnerConnection.As<DbConnection>(); }
        }
        /// <summary>内部执行</summary>
        protected override IETLRowCollection ExecuteInternal(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var connection = this.DbConnection;
            var command = connection.CreateCommand();
            PrepareCommand(command, context, inputRow, inputColumn, inputParameter);
            this.InjectCommand?.Invoke(command, context, inputRow, inputColumn, inputParameter);
            return ExecuteCommand(command, context, inputRow, inputColumn, inputParameter);
        }
        /// <summary>准备命令</summary>
        private void PrepareCommand(DbCommand command, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            command.CommandType = this.CommandType;
            command.CommandText = this.CommandText;
            if (this.CommandTimeout > 0)
            {
                command.CommandTimeout = this.CommandTimeout;
            }
            var commandParameters = AddParameters(this.Parameters, context, inputRow, inputColumn, inputParameter);
            if (!commandParameters.IsNullOrEmpty())
            {
                command.Parameters.AddRange(commandParameters.ToArray());
            }
        }
        /// <summary>加入参数</summary>
        private List<DbParameter> AddParameters(IEnumerable<ETLParameterInfo> parameters, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var commandParameters = new List<DbParameter>();
            foreach (var parameter in parameters)
            {
                var commandParameter = AddParameter(parameter, context, inputRow, inputColumn, inputParameter);
                commandParameters.Add(commandParameter);
            }
            return commandParameters;
        }
        /// <summary>加入参数</summary>
        public DbParameter AddParameter(ETLParameterInfo parameter, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var container = this.ETLDbConnection;
            var dbParameter = container.Provider.CreateParameter();
            dbParameter.ParameterName = container.NormalizeParameterName(parameter.Name);
            dbParameter.DbType = parameter.Type.ETLToDbType();
            dbParameter.Value = parameter.GetValue(context, inputRow, null, inputParameter);
            if (dbParameter.Value == null) dbParameter.Value = DBNull.Value;
            return dbParameter;
        }
        /// <summary>规范化对象名称</summary>
        public string NormalizeObjectName(string objectName)
        {
            return this.ETLDbConnection.NormalizeObjectName(objectName);
        }
        /// <summary>生成行号字段</summary>
        public string BuildRowNumberField(string rowNumberColumn)
        {
            return this.ETLDbConnection.BuildRowNumberField(rowNumberColumn);
        }
        /// <summary>执行命令</summary>
        private IETLRowCollection ExecuteCommand(DbCommand command, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            switch (this.CommandOperation)
            {
                case ETLDbCommandOperation.FillTable:
                    {
                        var adapter = this.ETLDbConnection.Provider.CreateDataAdapter();
                        adapter.SelectCommand = command;
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        var table = dataSet.Tables[0];
                        return new ETLDbDataTable(table);
                    }
                case ETLDbCommandOperation.FillDataSet:
                    {
                        var adapter = this.ETLDbConnection.Provider.CreateDataAdapter();
                        adapter.SelectCommand = command;
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return new ETLDbDataSet(dataSet);
                    }
                case ETLDbCommandOperation.FillSchema:
                    {
                        var adapter = this.ETLDbConnection.Provider.CreateDataAdapter();
                        adapter.SelectCommand = command;
                        var stable = new DataTable("Schema");
                        var table = adapter.FillSchema(stable, SchemaType.Source);
                        return new ETLDbDataTable(table);
                    }
                case ETLDbCommandOperation.ExecuteNonQuery:
                    command.ExecuteNonQuery();
                    break;
                case ETLDbCommandOperation.ExecuteScalar:
                    var result = command.ExecuteScalar();
                    if (inputParameter != null && inputParameter is ETLParameterInfo)
                    {
                        inputParameter.As<ETLParameterInfo>().Value = result;
                    }
                    break;
            }
            return null;
        }

        #region 实现克隆接口
        /// <summary>克隆成员方法</summary>
        protected override void CloneMembers(ETLTask clone)
        {

        }
        #endregion

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.CommandType = config.GetElementValue_Enum(node, nameof(this.CommandType), System.Data.CommandType.Text);
            this.CommandText = config.GetElementValue(node, nameof(this.CommandText));
            this.CommandTimeout = config.GetElementValue_Int32(node, nameof(this.CommandTimeout));
            this.CommandOperation = config.GetElementValue_Enum(node, nameof(this.CommandOperation), ETLDbCommandOperation.FillTable);
            this.Parameters = config.LoadParameters(node);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetElementValue(node, nameof(this.CommandType), this.CommandType.ToString());
            config.SetElementValue_CDDATA(node, nameof(this.CommandText), this.CommandText);
            config.SetElementValue(node, nameof(this.CommandTimeout), this.CommandTimeout.ToString());
            config.SetElementValue(node, nameof(this.CommandOperation), this.CommandOperation.ToString());
            node.Add(config.BuildParameters(this.Parameters));
        }
        #endregion
    }
}
