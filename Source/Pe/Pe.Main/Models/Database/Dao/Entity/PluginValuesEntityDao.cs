using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginValuesEntityDao: EntityDaoBase
    {
        public PluginValuesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string Sequence { get; } = "Sequence";
            public static string Data { get; } = "Data";

            #endregion
        }

        #endregion

    }
}
