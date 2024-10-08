using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherRedoItemsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherRedoItemsDto: CommonDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string RedoMode { get; set; } = string.Empty;
            public TimeSpan WaitTime { get; set; }
            public long RetryCount { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        public LauncherRedoItemsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherRedoData ConvertFromDto(LauncherRedoItemsDto dto)
        {
            var redoModeTransfer = new EnumTransfer<RedoMode>();

            return new LauncherRedoData() {
                RedoMode = redoModeTransfer.ToEnum(dto.RedoMode),
                RetryCount = ToInt(dto.RetryCount),
                WaitTime = dto.WaitTime,
            };
        }

        private LauncherRedoItemsDto ConvertFromData(LauncherItemId launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus commonStatus)
        {
            var redoModeTransfer = new EnumTransfer<RedoMode>();

            var dto = new LauncherRedoItemsDto() {
                LauncherItemId = launcherItemId,
                RedoMode = redoModeTransfer.ToString(data.RedoMode),
                RetryCount = ToInt(data.RetryCount),
                WaitTime = data.WaitTime,
            };
            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        public bool SelectExistsLauncherRedoItem(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.QueryFirst<bool>(statement, parameter);
        }

        public LauncherRedoData SelectLauncherRedoItem(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QueryFirst<LauncherRedoItemsDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public void InsertRedoItem(LauncherItemId launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);

            Context.InsertSingle(statement, dto);
        }


        public void UpdateRedoItem(LauncherItemId launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);

            Context.UpdateByKey(statement, dto);
        }

        public bool DeleteRedoItemByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Context.DeleteByKeyOrNothing(statement, parameter);
        }

        #endregion
    }
}
