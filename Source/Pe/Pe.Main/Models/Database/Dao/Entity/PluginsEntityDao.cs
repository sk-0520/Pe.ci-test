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
    public class PluginsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class PluginStateDto: CommonDtoBase
        {
            #region property

            public PluginId PluginId { get; set; }

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

        public IEnumerable<PluginStateData> SelectPluginStateData()
        {
            var statement = LoadStatement();
            return Context
                .Query<PluginStateDto>(statement)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public PluginStateData? SelectPluginStateDataByPluginId(PluginId pluginId)
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

        public Version? SelectLastUsePluginVersion(PluginId pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId
            };
            return Context.QueryFirstOrDefault<Version>(statement, parameter);
        }

        public bool SelectExistsPlugin(PluginId pluginId)
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

        public void InsertPluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);

            Context.InsertSingle(statement, dto);
        }

        public bool UpdatePluginStateData(PluginStateData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, databaseCommonStatus);

            return Context.UpdateByKeyOrNothing(statement, dto);
        }

        public bool UpdatePluginRunningState(PluginId pluginId, Version pluginVersion, Version applicationVersion, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.PluginId] = pluginId;
            parameter[Column.LastUseTimestamp] = DateTime.UtcNow; // DAO層でまぁいっかぁ
            parameter[Column.LastUsePluginVersion] = pluginVersion;
            parameter[Column.LastUseAppVersion] = applicationVersion;

            return Context.UpdateByKeyOrNothing(statement, parameter);
        }

        public void DeletePlugin(PluginId pluginId)
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
