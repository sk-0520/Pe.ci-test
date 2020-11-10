using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginSettingsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginSettingDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }
            public string PluginSettingKey { get; set; } = string.Empty;
            public string DataType { get; set; } = string.Empty;
            public string DataValue { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public PluginSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string PluginSettingKey { get; } = "PluginSettingKey";
            public static string DataType { get; } = "DataType";
            public static string DataValue { get; } = "DataValue";

            #endregion
        }

        #endregion

        #region function

        PluginSettingRawValue ConvertFromDto(PluginSettingDto dto)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var data = new PluginSettingRawValue(
                pluginPersistentFormatTransfer.ToEnum(dto.DataType),
                dto.DataValue
            );
            return data;
        }

        PluginSettingDto ConvertFromData(Guid pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginPersistentFormatTransfer = new EnumTransfer<PluginPersistentFormat>();

            var dto = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
                DataType = pluginPersistentFormatTransfer.ToString(data.Format),
                DataValue = data.Value,
            };
            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelecteExistsPluginSetting(Guid pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.QueryFirst<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginSettingValue(Guid pluginId, string key)
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

        public bool InsertPluginSetting(Guid pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }

        public bool UpdatePluginSetting(Guid pluginId, string key, PluginSettingRawValue data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, key, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }

        public bool DeletePluginSetting(Guid pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Context.Execute(statement, parameter) == 1;
        }

        public int DeleteAllPluginSettings(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
            };

            return Context.Execute(statement, parameter);
        }


        #endregion

    }
}
