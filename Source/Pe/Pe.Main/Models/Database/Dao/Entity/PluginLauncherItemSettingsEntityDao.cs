using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginLauncherItemSettingsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class PluginLauncherItemSettingDto: CommonDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }
            public LauncherItemId LauncherItemId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #endregion

        public PluginLauncherItemSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginSettingRawValue ConvertFromDto(PluginLauncherItemSettingDto dto)
        {
            var pluginPersistenceFormatTransfer = new EnumTransfer<PluginPersistenceFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistenceFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        private PluginLauncherItemSettingDto ConvertFromData(PluginId pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistenceFormatTransfer = new EnumTransfer<PluginPersistenceFormat>();

            var dto = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
                DataType = pluginPersistenceFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public IEnumerable<string> SelectPluginLauncherItemSettingKeys(PluginId pluginId, LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
            };

            return Context.Query<string>(statement, parameter);
        }

        public bool SelectExistsPluginLauncherItemSetting(PluginId pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginLauncherItemValue(PluginId pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            var dto = Context.QueryFirstOrDefault<PluginLauncherItemSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertPluginLauncherItemSetting(PluginId pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            Context.InsertSingle(statement, parameter);
        }

        public void UpdatePluginLauncherItemSetting(PluginId pluginId, LauncherItemId launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            Context.UpdateByKey(statement, parameter);
        }

        public bool DeletePluginLauncherItemSetting(PluginId pluginId, LauncherItemId launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }

        public int DeletePluginLauncherItemSettingsByPluginId(PluginId pluginId)
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
