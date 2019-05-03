using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public abstract class DatabaseStatementBuilderBase
    {
        DatabaseStatementBuilderBase(IDatabaseImplementation implementation)
        {
            Implementation = implementation;
        }

        public DatabaseStatementBuilderBase(IDatabaseImplementation implementation, ILogger logger)
            : this(implementation)
        {
            Logger = logger;
        }
        public DatabaseStatementBuilderBase(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : this(implementation)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected IDatabaseImplementation Implementation { get; }
        protected ILogger Logger { get; }

        protected IDictionary<string, object> ParametersImpl { get; } = new Dictionary<string, object>();
        public IReadOnlyDictionary<string, object> Parameters => (IReadOnlyDictionary<string, object>)ParametersImpl;

        ISet<string> KeyColumns { get; } = new HashSet<string>();

        #endregion

        #region function

        public abstract string BuildStatement();

        #endregion
    }

    public class DatabaseUpdateStatementBuilder : DatabaseStatementBuilderBase
    {
        public DatabaseUpdateStatementBuilder(IDatabaseImplementation implementation, ILogger logger)
            : base(implementation, logger)
        { }
        public DatabaseUpdateStatementBuilder(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region property

        public string TableName { get; private set; }

        ISet<string> UpdateColumns { get; } = new HashSet<string>();
        ISet<string> KeyColumns { get; } = new HashSet<string>();
        ISet<string> PlainValues { get; } = new HashSet<string>();

        ISet<string> IgnoreWhereParameters { get; } = new HashSet<string>();

        #endregion

        #region function

        public DatabaseUpdateStatementBuilder SetTable(string tableName)
        {
            TableName = tableName;

            return this;
        }

        public DatabaseUpdateStatementBuilder AddKey(string column, object value)
        {
            KeyColumns.Add(column);
            ParametersImpl.Add(column, value);

            return this;
        }
        public DatabaseUpdateStatementBuilder AddValue(string column, object value)
        {
            UpdateColumns.Add(column);
            ParametersImpl.Add(column, value);

            return this;
        }

        public DatabaseUpdateStatementBuilder AddPlain(string column, string value)
        {
            UpdateColumns.Add(column);
            ParametersImpl.Add(column, value);
            PlainValues.Add(column);
            IgnoreWhereParameters.Add(column);

            return this;
        }

        public DatabaseUpdateStatementBuilder AddIgnoreWhere(string column)
        {
            IgnoreWhereParameters.Add(column);

            return this;
        }

        #endregion

        #region DatabaseStatementBuilderBase

        public override string BuildStatement()
        {
            var sb = new StringBuilder();

            sb.AppendLine("update");
            sb.Append('\t');
            sb.AppendLine(Implementation.ToStatementTableName(TableName));

            var parameterIndex = 0;
            sb.AppendLine("set");
            foreach(var item in ParametersImpl.Keys.Select((k, i) => (index: i, key: k))) {
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(item.key));
                sb.Append('=');
                if(PlainValues.Contains(item.key)) {
                    sb.Append(Parameters[item.key]);
                } else {
                    sb.Append(Implementation.ToStatementParameterName(item.key, parameterIndex++));
                }
                if(item.index + 1 != ParametersImpl.Count) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }

            sb.AppendLine("where");
            var enabledWhereItems = ParametersImpl.Keys.Except(IgnoreWhereParameters).ToList();
            var keyItems = enabledWhereItems.Intersect(KeyColumns).Select(i => (column: i, equal: true));
            var paramItems = enabledWhereItems.Except(KeyColumns).Select(i => (column: i, equal: false));
            var whereItems = keyItems.Concat(paramItems).ToArray();
            for(var i = 0; i < whereItems.Length; i++) {
                var whereItem = whereItems[i];
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(whereItem.column));
                if(whereItem.equal) {
                    sb.Append('=');
                } else {
                    sb.Append("!=");
                }
                sb.Append(Implementation.ToStatementParameterName(whereItem.column, parameterIndex++));
                sb.AppendLine();

                if(i + 1 != whereItems.Length) {
                    sb.AppendLine("\tand");
                }
            }

            return sb.ToString();
        }

        #endregion
    }

    public class DatabaseDeleteStatementBuilder : DatabaseStatementBuilderBase
    {
        public DatabaseDeleteStatementBuilder(IDatabaseImplementation implementation, ILogger logger)
            : base(implementation, logger)
        { }

        public DatabaseDeleteStatementBuilder(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region proeprty
        public string TableName { get; private set; }
        ISet<string> UpdateColumns { get; } = new HashSet<string>();
        ISet<string> PlainValues { get; } = new HashSet<string>();

        #endregion

        #region function

        public DatabaseDeleteStatementBuilder SetTable(string tableName)
        {
            TableName = tableName;

            return this;
        }
        public DatabaseDeleteStatementBuilder AddValue(string column, object value)
        {
            UpdateColumns.Add(column);
            ParametersImpl.Add(column, value);

            return this;
        }

        public DatabaseDeleteStatementBuilder AddPlain(string column, string value)
        {
            UpdateColumns.Add(column);
            ParametersImpl.Add(column, value);
            PlainValues.Add(column);

            return this;
        }

        #endregion

        #region DatabaseStatementBuilderBase

        public override string BuildStatement()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Implementation.GetCommonStatementKeyword(DatabaseCommonStatementKeyword.Delete));
            sb.Append('\t');
            sb.AppendLine(Implementation.ToStatementTableName(TableName));

            var parameterIndex = 0;

            sb.AppendLine(Implementation.GetCommonStatementKeyword(DatabaseCommonStatementKeyword.Where));
            var whereItems = Parameters.ToArray();
            for(var i = 0; i < whereItems.Length; i++) {
                var whereItem = whereItems[i];
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(whereItem.Key));
                sb.Append('=');
                sb.Append(Implementation.ToStatementParameterName(whereItem.Key, parameterIndex++));
                sb.AppendLine();

                if(i + 1 != whereItems.Length) {
                    sb.AppendLine("\tand");
                }
            }

            return sb.ToString();
        }

        #endregion

    }
}
