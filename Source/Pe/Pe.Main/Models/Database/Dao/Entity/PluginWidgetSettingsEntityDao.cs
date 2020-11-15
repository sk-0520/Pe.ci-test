using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginWidgetSettingsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginWidgetSettingDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }
            public double? X { get; set; }
            public double? Y { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public bool IsVisible { get; set; }
            public bool IsTopmost { get; set; }

            #endregion
        }

        #endregion

        public PluginWidgetSettingsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
           : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
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

        #region function

        PluginWidgetSettingData ConvertFromDto(PluginWidgetSettingDto dto)
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

        PluginWidgetSettingDto ConvertFromData(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
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

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelectExistsPluginWidgetSetting(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public PluginWidgetSettingData SelectPluginWidgetSetting(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };

            var dto = Context.QueryFirst<PluginWidgetSettingDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public bool SelectPluginWidgetTopmost(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public bool InsertPluginWidgetSetting(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }

        public bool InsertPluginWidgetTopmost(Guid pluginId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.IsTopmost] = isTopmost;

            return Context.Execute(statement, parameter) == 1;
        }

        public bool UpdatePluginWidgetSetting(Guid pluginId, PluginWidgetSettingData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = ConvertFromData(pluginId, data, databaseCommonStatus);

            return Context.Execute(statement, parameter) == 1;
        }


        public bool UpdatePluginWidgetTopmost(Guid pluginId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.IsTopmost] = isTopmost;

            return Context.Execute(statement, parameter) == 1;
        }

        public int DeletePluginWidgetSettingsByPluginId(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
