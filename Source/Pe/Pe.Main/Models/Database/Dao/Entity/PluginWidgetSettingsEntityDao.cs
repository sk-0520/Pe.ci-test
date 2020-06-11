using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class PluginWidgetSettingDto: CommonDtoBase
    {
        #region property

        public Guid PluginId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsVisible { get; set; }

        #endregion
    }

    public class PluginWidgetSettingsEntityDao: EntityDaoBase
    {
        public PluginWidgetSettingsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
           : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string X { get; } = "X";
            public static string Y { get; } = "Y";
            public static string IsVisible { get; } = "IsVisible";

            #endregion
        }

        #endregion

        #region function

        PluginWidgetSettingData ConvertFromDto(PluginWidgetSettingDto dto)
        {
            var data = new PluginWidgetSettingData() {
                X = dto.X,
                Y = dto.Y,
                IsVisible = dto.IsVisible,
            };

            return data;
        }

        PluginWidgetSettingDto ConvertFromData(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new PluginWidgetSettingDto() {
                PluginId = pluginId,
                X = data.X,
                Y = data.Y,
                IsVisible = data.IsVisible,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelectExistsPluginWidgetSetting(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Commander.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginWidgetSettingData SelectPluginWidgetSetting(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };

            var dto = Commander.QueryFirst<PluginWidgetSettingDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public bool InsertPluginWidgetSetting(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdatePluginWidgetSetting(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
