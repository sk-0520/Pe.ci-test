#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace ContentTypeTextNet.Pe.Core.Model.Database.Vender.Public.SQLite
{
    /// <summary>
    /// booleanを制御
    /// <para>0: 偽, 0以外: 真</para>
    /// </summary>
    class SqliteBooleanHandler : SqlMapper.TypeHandler<bool>
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

            if(value.GetType() == typeof(bool)) {
                return (bool)value;
            }

            return (long)value != 0;
        }
    }

    class SqliteVersionHandler : SqlMapper.TypeHandler<Version>
    {
        public override void SetValue(IDbDataParameter parameter, Version value)
        {
            parameter.Value = value.ToString(4);
        }

        public override Version Parse(object value)
        {
            var s = (string)value;
            if(s != null) {
                if(Version.TryParse(s, out var ret)) {
                    return ret;
                }
            }

#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return null;
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
        }
    }

    class SqliteGuidHandler : SqlMapper.TypeHandler<Guid>
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

    public class SqliteImplementation : DatabaseImplementation
    {
        static SqliteImplementation()
        {
            SqlMapper.AddTypeHandler(typeof(bool), new SqliteBooleanHandler());
            SqlMapper.AddTypeHandler(typeof(Version), new SqliteVersionHandler());
            SqlMapper.AddTypeHandler(typeof(Guid), new SqliteGuidHandler());
        }

        #region DatabaseImplementation

        public override bool SupportedTransactionDDL { get; } = true;

        public override string GetCommonStatementKeyword(DatabaseCommonStatementKeyword keyword)
        {
            if(keyword == DatabaseCommonStatementKeyword.Delete) {
                return "delete from";
            }
            return base.GetCommonStatementKeyword(keyword);
        }

        public override string ToStatementTableName(string tableName) => "[" + tableName + "]";
        public override string ToStatementColumnName(string columnName) => "[" + columnName + "]";
        public override string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        #endregion
    }
}

#endif
