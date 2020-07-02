using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginLauncherItemSettingsEntityDao: EntityDaoBase
    {
        public PluginLauncherItemSettingsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #region funtion

        #endregion

    }
}
