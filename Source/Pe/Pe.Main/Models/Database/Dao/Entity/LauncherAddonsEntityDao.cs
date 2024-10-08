using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherAddonsEntityDao: EntityDaoBase
    {
        #region define

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string PluginId { get; } = "PluginId";

            #endregion
        }

        #endregion

        public LauncherAddonsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public PluginId SelectAddonPluginId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Context.QueryFirst<PluginId>(statement, parameter);
        }

        public IEnumerable<LauncherItemId> SelectLauncherItemIdsByPluginId(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.Query<LauncherItemId>(statement, parameter);
        }

        public void InsertAddonPluginId(LauncherItemId launcherItemId, PluginId pluginId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.PluginId] = pluginId;

            Context.InsertSingle(statement, parameter);
        }

        public int DeleteLauncherAddonsByPluginId(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
