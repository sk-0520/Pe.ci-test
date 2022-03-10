using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginsEntityDao: EntityDaoBase
    {
        #region define

        private class PluginStateDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }

            public string Name { get; set; } = string.Empty;
            public string State { get; set; } = string.Empty;


            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string Name { get; } = "Name";
            public static string State { get; } = "State";
            public static string LastUseTimestamp { get; } = "LastUseTimestamp";
            public static string LastUsePluginVersion { get; } = "LastUsePluginVersion";
            public static string LastUseAppVersion { get; } = "LastUseAppVersion";
            public static string ExecuteCount { get; } = "ExecuteCount";

            #endregion
        }

        #endregion

        public PluginsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private PluginStateDto ConvertFromData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var pluginStateTransfer = new EnumTransfer<PluginState>();

            var dto = new PluginStateDto() {
                PluginId = data.PluginId,
                Name = data.PluginName,
                State = pluginStateTransfer.ToString(data.State),
            };
            databaseCommonStatus.WriteCommonTo(dto);

            return dto;
        }

        private PluginStateData ConvertFromDto(PluginStateDto dto)
        {
            var pluginStateTransfer = new EnumTransfer<PluginState>();

            var data = new PluginStateData() {
                PluginId = dto.PluginId,
                PluginName = dto.Name,
                State = pluginStateTransfer.ToEnum(dto.State),
            };

            return data;
        }

        public IEnumerable<PluginStateData> SelectePlguinStateData()
        {
            var statement = LoadStatement();
            return Context
                .Query<PluginStateDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public PluginStateData? SelectePlguinStateDataByPLuginId(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            var dto = Context.QueryFirstOrDefault<PluginStateDto>(statement, parameter);
            if(dto == null) {
                return null;
            }

            return ConvertFromDto(dto);
        }

        public Version? SelectLastUsePluginVersion(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<Version>(statement, parameter);
        }

        public bool SelecteExistsPlugin(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public IEnumerable<PluginLastUsedData> SelectAllLastUsedPlugins()
        {
            var statement = LoadStatement();
            return Context.Query<PluginLastUsedData>(statement);
        }

        public bool SelectExistsPluginByState(PluginState pluginState)
        {
            var pluginStateTransfer = new EnumTransfer<PluginState>();

            var statement = LoadStatement();
            var parameter = new {
                State = pluginStateTransfer.ToString(pluginState),
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public bool InsertPluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool UpdatePluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);
            return Context.Execute(statement, dto) == 1;
        }

        public bool UpdatePluginRunningState(Guid pluginId, Version pluginVersion, Version applicationVersio, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.LastUseTimestamp] = DateTime.UtcNow; // DAO層でまぁいっかぁ
            parameter[Column.LastUsePluginVersion] = pluginVersion;
            parameter[Column.LastUseAppVersion] = applicationVersio;
            return Context.Execute(statement, parameter) == 1;
        }

        public bool DeletePlugin(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
