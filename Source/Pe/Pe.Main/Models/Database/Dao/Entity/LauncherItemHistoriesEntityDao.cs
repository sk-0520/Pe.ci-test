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

        #endregion
    }
}
