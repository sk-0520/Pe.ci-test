using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginWidgetSettingsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class PluginWidgetSettingDto: CommonDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }
            public double? X { get; set; }
            public double? Y { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public bool IsVisible { get; set; }
            public bool IsTopmost { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string X { get; } = "X";
            public static string Y { get; } = "Y";
            public static string Width { get; } = "Width";
            public static string Height { get; } = "Height";
            public static string IsVisible { get; } = "IsVisible";
            public static string IsTopmost { get; } = "IsTopmost";

            #endregion
        }

        #endregion

        public PluginWidgetSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
           : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginWidgetSettingData ConvertFromDto(PluginWidgetSettingDto dto)
        {
            static double NullOrNanCoalescing(double? value)
            {
                if(value.HasValue && !double.IsNaN(value.Value)) {
                    return value.Value;
                }

                return double.NaN;
            }

            var data = new PluginWidgetSettingData() {
                X = NullOrNanCoalescing(dto.X),
                Y = NullOrNanCoalescing(dto.Y),
                Width = NullOrNanCoalescing(dto.Width),
                Height = NullOrNanCoalescing(dto.Height),
                IsVisible = dto.IsVisible,
                IsTopmost = dto.IsTopmost,
            };

            return data;
        }

        private PluginWidgetSettingDto ConvertFromData(PluginId pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new PluginWidgetSettingDto() {
                PluginId = pluginId,
                X = data.X,
                Y = data.Y,
                Width = data.Width,
                Height = data.Height,
                IsVisible = data.IsVisible,
                IsTopmost = data.IsTopmost,
            };

            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        public bool SelectExistsPluginWidgetSetting(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginWidgetSettingData SelectPluginWidgetSetting(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };

            var dto = Context.QueryFirst<PluginWidgetSettingDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public bool SelectPluginWidgetTopmost(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public void InsertPluginWidgetSetting(PluginId pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            Context.InsertSingle(statement, parameter);
        }

        public void InsertPluginWidgetTopmost(PluginId pluginId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.IsTopmost] = isTopmost;

            Context.InsertSingle(statement, parameter);
        }

        public void UpdatePluginWidgetSetting(PluginId pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            Context.UpdateByKey(statement, parameter);
        }


        public void UpdatePluginWidgetTopmost(PluginId pluginId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.IsTopmost] = isTopmost;

            Context.UpdateByKey(statement, parameter);
        }

        public int DeletePluginWidgetSettingsByPluginId(PluginId pluginId)
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
