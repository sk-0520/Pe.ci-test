#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database.Vender.Public.SQLite
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
                if(Version.TryParse(s, out var ver)) {
                    return ver;
                }
            }

            return null;
        }
    }

    public class SqliteImplementation: DatabaseImplementation
    {
        static SqliteImplementation()
        {
            SqlMapper.AddTypeHandler(typeof(bool), new SqliteBooleanHandler());
            SqlMapper.AddTypeHandler(typeof(Version), new SqliteVersionHandler());
        }

        #region DatabaseImplementation

        public override bool SupportedTransactionDDL { get; } = true;

        #endregion
    }
}

#endif
