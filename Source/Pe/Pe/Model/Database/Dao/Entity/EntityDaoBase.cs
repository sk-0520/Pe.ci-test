using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class EntityUpdateStatementBuilder
    {
        #region property

        public string TableName { get; private set; }

        IDictionary<string, object> ParametersImpl { get; } = new Dictionary<string, object>();
        public IReadOnlyDictionary<string, object> Parameters => (IReadOnlyDictionary<string, object>)ParametersImpl;

        ISet<string> UpdateColumns { get; } = new HashSet<string>();
        ISet<string> KeyColumns { get; } = new HashSet<string>();

        ISet<string> IgnoreWhereParameters { get; } = new HashSet<string>();

        #endregion

        #region function

        public EntityUpdateStatementBuilder SetTable(string tableName)
        {
            TableName = tableName;

            return this;
        }

        public EntityUpdateStatementBuilder AddKey(string column, object value)
        {
            KeyColumns.Add(column);
            ParametersImpl.Add(column, value);

            return this;
        }
        public EntityUpdateStatementBuilder AddValue(string column, object value)
        {
            UpdateColumns.Add(column);
            ParametersImpl.Add(column, value);

            return this;
        }

        public EntityUpdateStatementBuilder AddIgnoreWhere(string column)
        {
            IgnoreWhereParameters.Add(column);

            return this;
        }

        public string BuildStatement()
        {
            var sb = new StringBuilder();

            sb.AppendLine("update");
            sb.Append('\t');
            sb.Append('[');
            sb.Append(TableName);
            sb.Append(']');
            sb.AppendLine();

            sb.AppendLine("set");
            foreach(var item in ParametersImpl.Keys.Select((k, i) => (index: i, key: k))) {
                sb.Append('\t');
                sb.Append('[');
                sb.Append(item.key);
                sb.Append(']');
                sb.Append('=');
                sb.Append('@');
                sb.Append(item.key);
                if(item.index + 1 != ParametersImpl.Count) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }

            sb.AppendLine("where");
            var whereItems = ParametersImpl.Keys.Except(IgnoreWhereParameters).ToList();
            var keyWheres = whereItems.Intersect(KeyColumns).Select(i => $"\t[{i}]=@{i}");
            var paramWheres = whereItems.Except(KeyColumns).Select(i => $"\t[{i}]!=@{i}");
            var wheres = keyWheres.Concat(paramWheres).ToArray();
            for(var i = 0; i < wheres.Length; i++) {
                var where = wheres[i];
                sb.AppendLine(where);
                if(i + 1 != wheres.Length) {
                    sb.AppendLine("\tAND");
                }
            }

            return sb.ToString();
        }

        #endregion
    }

    public abstract class EntityDaoBase : ApplicationDatabaseObjectBase
    {
        public EntityDaoBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILogger logger)
            : base(commander, statementLoader, implementation, logger)
        { }
        public EntityDaoBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        protected static IReadOnlyList<string> CommonCreateColumns { get; } = new[] {
            "CreatedTimestamp",
            "CreatedAccount",
            "CreatedProgramName",
            "CreatedProgramVersion",
        };
        protected static IReadOnlyList<string> CommonUpdateColumns { get; } = new[] {
            "UpdatedTimestamp",
            "UpdatedAccount",
            "UpdatedProgramName",
            "UpdatedProgramVersion",
            "UpdatedCount",
        };

        public virtual string TableName
        {
            get
            {
                var className = GetType().Name;
                var suffix = "EntityDao";
                if(className.EndsWith(suffix)) {
                    return className.Substring(0, className.Length - suffix.Length);
                }

                throw new NotImplementedException();
            }
        }

        protected EntityUpdateStatementBuilder CreateUpdateBuilder(IDatabaseCommonStatus databaseCommonStatus)
        {
            var result = new EntityUpdateStatementBuilder();
            result.SetTable(TableName);
            foreach(var ignoreColumn in CommonCreateColumns.Concat(CommonUpdateColumns)) {
                result.AddIgnoreWhere(ignoreColumn);
            }
            var mapping = databaseCommonStatus.CreateCommonDtoMapping();
            foreach(var pair in mapping) {
                result.AddValue(pair.Key, pair.Value);
            }

            return result;
        }

        public int ExecuteUpdate(EntityUpdateStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.Execute(statement, param);
        }

        #endregion
    }
}
