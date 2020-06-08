using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class PluginSettingDto: CommonDtoBase
    {
        #region property

        public Guid PluginId { get; set; }
        public string PluginSettingKey { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string DataValue { get; set; } = string.Empty;

        #endregion
    }

    public class PluginSettingsEntityDao: EntityDaoBase
    {
        public PluginSettingsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
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

        public bool SelecteExistsPluginSetting(Guid pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            return Commander.QueryFirst<bool>(statement, parameter);
        }

        public PluginSettingRawValue? SelectPluginSettingValue(Guid pluginId, string key)
        {
            var statement = LoadStatement();
            var parameter = new PluginSettingDto() {
                PluginId = pluginId,
                PluginSettingKey = key,
            };

            var dto = Commander.QueryFirstOrDefault<PluginSettingDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            var data = ConvertFromDto(dto);
            return data;
        }

        #endregion

    }
}
