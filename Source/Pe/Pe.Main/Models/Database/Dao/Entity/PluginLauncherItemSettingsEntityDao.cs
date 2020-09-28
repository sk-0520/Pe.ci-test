using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginLauncherItemSettingsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginLauncherItemSettingDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }
            public Guid LauncherItemId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        #endregion

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

        PluginSettingRawValue ConvertFromDto(PluginLauncherItemSettingDto dto)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistentFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        PluginLauncherItemSettingDto ConvertFromData(Guid pluginId, Guid launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var dto = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
                DataType = pluginPersistentFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelecteExistsPluginLauncherItemSetting(Guid pluginId, Guid launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Commander.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginLauncherItemValue(Guid pluginId, Guid launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            var dto = Commander.QueryFirstOrDefault<PluginLauncherItemSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        internal bool InsertPluginLauncherItemSetting(Guid pluginId, Guid launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            return Commander.Execute(statement, parameter) == 1;
        }

        internal bool UpdatePluginLauncherItemSetting(Guid pluginId, Guid launcherItemId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, launcherItemId, key, data, databaseCommonStatus);

            return Commander.Execute(statement, parameter) == 1;
        }

        public bool DeletePluginLauncherItemSetting(Guid pluginId, Guid launcherItemId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginLauncherItemSettingDto() {
                PluginId = pluginId,
                LauncherItemId = launcherItemId,
                PluginSettingKey = key,
            };

            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion

    }
}
