using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class InstallPluginsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class InstallPluginRowDto: CreateDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }
            public string PluginName { get; set; } = string.Empty;
            public Version? PluginVersion { get; set; }
            public string PluginInstallMode { get; set; } = string.Empty;

            public string ExtractedDirectoryPath { get; set; } = string.Empty;
            public string PluginDirectoryPath { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string ExtractedDirectoryPath { get; } = "ExtractedDirectoryPath";
            public static string PluginDirectoryPath { get; } = "PluginDirectoryPath";

            #endregion
        }

        #endregion

        public InstallPluginsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginInstallData ConvertFromDto(InstallPluginRowDto dto)
        {
            var pluginInstallModeTransfer = new EnumTransfer<PluginInstallMode>();

            return new PluginInstallData(
                dto.PluginId,
                dto.PluginName,
                dto.PluginVersion ?? new Version(),
                pluginInstallModeTransfer.ToEnum(dto.PluginInstallMode),
                dto.ExtractedDirectoryPath,
                dto.PluginDirectoryPath
            );
        }

        public bool SelectExistsInstallPluginByPluginId(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public IEnumerable<PluginInstallData> SelectInstallPlugins()
        {
            var statement = LoadStatement();
            return Context.Query<InstallPluginRowDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool SelectExistsInstallPlugin()
        {
            var statement = LoadStatement();
            return Context.QueryFirstOrDefault<bool>(statement);
        }

        public void InsertInstallPlugin(PluginInstallData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginInstallModeTransfer = new EnumTransfer<PluginInstallMode>();

            var statement = LoadStatement();
            var parameter = new InstallPluginRowDto() {
                PluginId = data.PluginId,
                PluginName = data.PluginName,
                PluginVersion = data.PluginVersion,
                PluginInstallMode = pluginInstallModeTransfer.ToString(data.PluginInstallMode),
                ExtractedDirectoryPath = data.ExtractedDirectoryPath,
                PluginDirectoryPath = data.PluginDirectoryPath,
            };
            databaseCommonStatus.WriteCreateTo(parameter);

            Context.Execute(statement, parameter);
        }

        public void DeleteInstallPlugin(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
