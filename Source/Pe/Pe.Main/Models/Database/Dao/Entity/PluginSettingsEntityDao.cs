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
    public class PluginSettingsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class PluginSettingDto: CommonDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #endregion

        public PluginSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginSettingRawValue ConvertFromDto(PluginSettingDto dto)
        {
            var pluginPersistenceFormatTransfer = new EnumTransfer<PluginPersistenceFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistenceFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        private PluginSettingDto ConvertFromData(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistenceFormatTransfer = new EnumTransfer<PluginPersistenceFormat>();

            var dto = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
                DataType = pluginPersistenceFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public IEnumerable<string> SelectPluginSettingKeys(PluginId pluginId)
        {
            var statement = LoadStatement();

            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
            };

            return Context.Query<string>(statement, parameter);
        }

        public bool SelectExistsPluginSetting(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.QueryFirst<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginSettingValue(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            var dto = Context.QueryFirstOrDefault<PluginSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertPluginSetting(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            Context.InsertSingle(statement, parameter);
        }

        public void UpdatePluginSetting(PluginId pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            Context.UpdateByKey(statement, parameter);
        }

        public bool DeletePluginSetting(PluginId pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }

        public int DeleteAllPluginSettings(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
            };

            return Context.Delete(statement, parameter);
        }


        #endregion

    }
}
