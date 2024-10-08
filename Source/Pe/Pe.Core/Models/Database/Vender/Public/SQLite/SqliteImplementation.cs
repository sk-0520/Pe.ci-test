#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Dapper;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    /// <summary>
    /// booleanを制御
    /// </summary>
    /// <remarks>
    /// <para>0: 偽, 0以外: 真</para>
    /// </remarks>
    internal class SqliteBooleanHandler: SqlMapper.TypeHandler<bool>
    {
        public override void SetValue(IDbDataParameter parameter, bool value)
        {
            parameter.Value = value ? 1 : 0;
        }

        public override bool Parse(object value)
        {
            if(value == null) {
                return false;
            }

            if(value is bool) {
                return (bool)value;
            }

            return (long)value != 0;
        }
    }

    internal class SqliteVersionHandler: SqlMapper.TypeHandler<Version>
    {
        public override void SetValue(IDbDataParameter parameter, Version? value)
        {
            parameter.DbType = DbType.String;
            parameter.Value = value?.ToString(3);
        }

        public override Version Parse(object value)
        {
            if(value is string s) {
                if(Version.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return null!;
        }
    }

    internal class SqliteGuidHandler: SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            // 00000000-0000-0000-0000-000000000000
            parameter.Value = value.ToString("D");
        }

        public override Guid Parse(object value)
        {
            var s = (string)value;
            if(s != null) {
                if(Guid.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return Guid.Empty;
        }
    }

    internal class SqliteTimeSpanHandler: SqlMapper.TypeHandler<TimeSpan>
    {
        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
        {
            // [-][d.]hh:mm:ss[.fffffff]
            parameter.Value = value.ToString("c");
        }

        public override TimeSpan Parse(object value)
        {
            var s = (string)value;
            if(s != null) {
                if(TimeSpan.TryParse(s, out var ret)) {
                    return ret;
                }
            }

            return TimeSpan.Zero;
        }

    }

    public class SqliteImplementation: DatabaseImplementation
    {
        static SqliteImplementation()
        {
            SqlMapper.AddTypeMap(typeof(TimeSpan), DbType.String);
            SqlMapper.AddTypeHandler(typeof(bool), new SqliteBooleanHandler());
            SqlMapper.AddTypeHandler(typeof(Version), new SqliteVersionHandler());
            SqlMapper.AddTypeHandler(typeof(Guid), new SqliteGuidHandler());
            SqlMapper.AddTypeHandler(typeof(TimeSpan), new SqliteTimeSpanHandler());
        }

        #region DatabaseImplementation

        public override bool SupportedTransactionDDL { get; } = true;

        public override string ToStatementTableName(string tableName) => "[" + tableName + "]";
        public override string ToStatementColumnName(string columnName) => "[" + columnName + "]";
        public override string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        public override IDatabaseManagement CreateManagement(IDatabaseContext context)
        {
            return new SqliteManagement(context, this);
        }

        #endregion
    }
}

#endif
