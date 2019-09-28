using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    public abstract class DatabaseStatementBuilderBase
    {
#pragma warning disable CS8618 // Null 非許容フィールドが初期化されていません。
        private DatabaseStatementBuilderBase(IDatabaseImplementation implementation)
#pragma warning restore CS8618 // Null 非許容フィールドが初期化されていません。
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
            Logger = loggerFactory.CreateLogger(GetType());
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

    public class DatabaseSelectStatementBuilder : DatabaseStatementBuilderBase
    {
        public DatabaseSelectStatementBuilder(IDatabaseImplementation implementation, ILogger logger)
            : base(implementation, logger)
        { }
        public DatabaseSelectStatementBuilder(IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(implementation, loggerFactory)
        { }

        #region property

        Dictionary<string, string> AliasNames { get; } = new Dictionary<string, string>();
        IList<string> ColumnNames { get; } = new List<string>();

        public string TableName { get; private set; } = string.Empty;
        ISet<string> PlainValues { get; } = new HashSet<string>();

        #endregion

        #region function

        DatabaseSelectStatementBuilder AddSelectCore(string columnName, string aliasName)
        {
            if(string.IsNullOrWhiteSpace(columnName)) {
                throw new ArgumentNullException(nameof(columnName));
            }
            if(string.IsNullOrWhiteSpace(aliasName)) {
                throw new ArgumentNullException(nameof(aliasName));
            }
            if(ColumnNames.IndexOf(columnName) != -1) {
                throw new ArgumentException($"{nameof(columnName)}: {columnName}");
            }

            ColumnNames.Add(columnName);
            if(columnName != aliasName) {
                AliasNames.Add(columnName, aliasName);
            }

            return this;
        }

        public DatabaseSelectStatementBuilder AddSelect(string columnName) => AddSelectCore(columnName, columnName);
        public DatabaseSelectStatementBuilder AddSelect(string columnName, string aliasName) => AddSelectCore(columnName, aliasName);

        public DatabaseSelectStatementBuilder SetTable(string tableName)
        {
            TableName = tableName;

            return this;
        }

        public DatabaseSelectStatementBuilder AddValue(string column, object value)
        {
            ParametersImpl.Add(column, value);

            return this;
        }

        public DatabaseSelectStatementBuilder AddPlain(string column, string value)
        {
            ParametersImpl.Add(column, value);
            PlainValues.Add(column);

            return this;
        }

        #endregion

        #region DatabaseStatementBuilderBase
        public override string BuildStatement()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Implementation.GetCommonStatementKeyword(DatabaseCommonStatementKeyword.Select));

            var parameterIndex = 0;

            foreach(var columnItem in ColumnNames.Counting()) {
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(columnItem.Value));
                if(AliasNames.TryGetValue(columnItem.Value, out var aliasName)) {
                    sb.Append(' ');
                    sb.Append(Implementation.GetSelectStatementKeyword(DatabaseSelectStatementKeyword.As));
                    sb.Append(' ');
                }
                if(columnItem.Number + 1 != ColumnNames.Count) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }

            sb.AppendLine(Implementation.GetSelectStatementKeyword(DatabaseSelectStatementKeyword.From));
            sb.Append('\t');
            sb.AppendLine(Implementation.ToStatementTableName(TableName));


            sb.AppendLine(Implementation.GetCommonStatementKeyword(DatabaseCommonStatementKeyword.Where));
            var parameterKeys = ParametersImpl.Keys.ToArray();
            for(var i = 0; i < parameterKeys.Length; i++) {
                var columnName = parameterKeys[i];
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(columnName));
                sb.Append('=');

                var parameterValue = ParametersImpl[columnName];
                if(PlainValues.Contains(columnName)) {
                    sb.Append(columnName);
                } else {
                    sb.Append(Implementation.ToStatementParameterName(columnName, parameterIndex++));
                }
                sb.AppendLine();

                if(i + 1 != parameterKeys.Length) {
                    sb.AppendLine("\tand");
                }
            }

            return sb.ToString();
        }
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

        public string TableName { get; private set; } = string.Empty;

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
            foreach(var item in ParametersImpl.Keys.Counting()) {
                sb.Append('\t');
                sb.Append(Implementation.ToStatementColumnName(item.Value));
                sb.Append('=');
                if(PlainValues.Contains(item.Value)) {
                    sb.Append(Parameters[item.Value]);
                } else {
                    sb.Append(Implementation.ToStatementParameterName(item.Value, parameterIndex++));
                }
                if(item.Number + 1 != ParametersImpl.Count) {
                    sb.Append(',');
                }
                sb.AppendLine();
            }

            sb.AppendLine("where");
            var enabledWhereItems = ParametersImpl.Keys.Except(IgnoreWhereParameters).ToList();
            var keyItems = enabledWhereItems.Intersect(KeyColumns).Select(i => (column: i, equal: true));
            //var paramItems = enabledWhereItems.Except(KeyColumns).Select(i => (column: i, equal: false));
            //var whereItems = keyItems.Concat(paramItems).ToArray();
            var whereItems = keyItems.ToArray();
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
        public string TableName { get; private set; } = string.Empty;
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
