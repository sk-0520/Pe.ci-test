using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemHistoriesEntityDao : EntityDaoBase
    {
        public LauncherItemHistoriesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string Kind { get; } = "Kind";
            public static string Value { get; } = "Value";
            public static string LastExecuteTimestamp { get; } = "LastExecuteTimestamp";

            #endregion
        }

        #endregion

        #region function

        LauncherHistoryData ConvertFromDto(LauncherItemHistoriesEntityDto dto)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var result = new LauncherHistoryData() {
                Kind = launcherHistoryKindTransfer.ToEnum(dto.Kind),
                Value = dto.Value,
                LastExecuteTimestamp = dto.LastExecuteTimestamp,
            };

            return result;
        }

        public IEnumerable<LauncherHistoryData> SelectHistories(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.Kind);
            builder.AddSelect(Column.Value);
            builder.AddSelect(Column.LastExecuteTimestamp);

            builder.AddValueParameter(Column.LauncherItemId, launcherItemId);

            builder.AddOrder(Column.LastExecuteTimestamp, DatabaseOrder.Desc);

            return Select<LauncherItemHistoriesEntityDto>(builder)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public bool InsertHistory(Guid launcherItemId, LauncherHistoryKind kind, string value, IDatabaseCommonStatus commonStatus)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var dto = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
                Kind = launcherHistoryKindTransfer.ToString(kind),
                Value = value,
                LastExecuteTimestamp = DateTime.UtcNow,
            };
            commonStatus.WriteCreate(dto);

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return Commander.Execute(statement, dto) == 1;
        }

        public bool DeleteHistory(Guid launcherItemId, LauncherHistoryKind kind, string value)
        {
            var launcherHistoryKindTransfer = new EnumTransfer<LauncherHistoryKind>();

            var dto = new LauncherItemHistoriesEntityDto() {
                LauncherItemId = launcherItemId,
                Kind = launcherHistoryKindTransfer.ToString(kind),
                Value = value,
            };

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var result = Commander.Execute(statement, dto);
            if(1 < result) {
                Logger.LogWarning("削除件数がちょっとあれ: {0}", result);
            }
            return 0 < result;
        }

        #endregion
    }
}
