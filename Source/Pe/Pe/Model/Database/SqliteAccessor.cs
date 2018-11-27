#define USE_DB_SQLITE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Dapper;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class SqliteAccessor : DatabaseAccessorBase<SQLiteConnection>
    {
        #region define

#if USE_DB_SQLITE
        /// <summary>
        /// booleanを制御
        /// <para>0: 偽, 0以外: 真</para>
        /// </summary>
        class SqliteBooleanHandler : SqlMapper.TypeHandler<bool>
        {
            public override void SetValue(IDbDataParameter parameter, bool value)
            {
                parameter.Value = value ? 1: 0;
            }

            public override bool Parse(object value)
            {
                return (long)value != 0;
            }
        }
#endif

        #endregion

        static SqliteAccessor()
        {
#if USE_DB_SQLITE
            SqlMapper.AddTypeHandler(typeof(bool), new SqliteBooleanHandler());
#endif
        }

        public SqliteAccessor(IDatabaseConnectionCreator<SQLiteConnection> connectionCreator, ILoggerFactory loggerFactory)
            : base(connectionCreator, loggerFactory)
        { }
    }
}
