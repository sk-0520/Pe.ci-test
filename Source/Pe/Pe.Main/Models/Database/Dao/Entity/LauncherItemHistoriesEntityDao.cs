using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemHistoriesEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherItemHistoriesEntityDto: CreateDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string Kind { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;

            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime LastExecuteTimestamp { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string Kind { get; } = "Kind";
            public static string Value { get; } = "Value";
            public static string LastExecuteTimestamp { get; } = "LastExecuteTimestamp";

            #endregion
        }

        #endregion

        public LauncherItemHistoriesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherHistoryData ConvertFromDto(LauncherItemHistoriesEntityDto dto)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var result = new LauncherHistoryData() {
                Kind = launcherHistoryKindTransfer.ToEnum(dto.Kind),
                Value = dto.Value,
                LastExecuteTimestamp = dto.LastExecuteTimestamp,
            };

            return result;
        }

        public IEnumerable<LauncherHistoryData> SelectHistories(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<LauncherItemHistoriesEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public void InsertHistory(LauncherItemId launcherItemId, LauncherHistoryKind kind, string value, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var dto = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
                Kind = launcherHistoryKindTransfer.ToString(kind),
                Value = value,
                LastExecuteTimestamp = timestamp,
            };
            commonStatus.WriteCreateTo(dto);

            var statement = StatementLoader.LoadStatementByCurrent(GetType());

            Context.InsertSingle(statement, dto);
        }

        public int DeleteHistory(LauncherItemId launcherItemId, LauncherHistoryKind kind, string value)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var dto = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
                Kind = launcherHistoryKindTransfer.ToString(kind),
                Value = value,
            };

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var result = Context.Delete(statement, dto);
            if(1 < result) {
                // これだけ正しさが分からん
                Logger.LogWarning("削除件数がちょっとあれ: {0}", result);
            }
            return result;
        }

        public int DeleteHistoryByLauncherItemId(LauncherItemId launcherItemId, LauncherHistoryKind kind, [DateTimeKind(DateTimeKind.Utc)] DateTime lastExecuteTimestamp)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var statement = LoadStatement();
            var parameter = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
                Kind = launcherHistoryKindTransfer.ToString(kind),
                LastExecuteTimestamp = lastExecuteTimestamp,
            };

            return Context.Delete(statement, parameter);
        }

        public int DeleteHistoriesByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
            };
            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
