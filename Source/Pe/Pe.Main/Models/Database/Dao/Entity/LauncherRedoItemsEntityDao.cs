using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherRedoItemsDto: CommonDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string RedoMode { get; set; } = string.Empty;
        public TimeSpan WaitTime { get; set; }
        public long RetryCount { get; set; }

        #endregion
    }

    public class LauncherRedoItemsEntityDao: EntityDaoBase
    {
        public LauncherRedoItemsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        #region function

        LauncherRedoData ConvertFromDto(LauncherRedoItemsDto dto)
        {
            var redoModeTransfer = new EnumTransfer<RedoMode>();

            return new LauncherRedoData() {
                RedoMode = redoModeTransfer.ToEnum(dto.RedoMode),
                RetryCount = ToInt(dto.RetryCount),
                WaitTime = dto.WaitTime,
            };
        }

        LauncherRedoItemsDto ConvertFromData(Guid launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus commonStatus)
        {
            var redoModeTransfer = new EnumTransfer<RedoMode>();

            var dto = new LauncherRedoItemsDto() {
                LauncherItemId = launcherItemId,
                RedoMode = redoModeTransfer.ToString(data.RedoMode),
                RetryCount = ToInt(data.RetryCount),
                WaitTime = data.WaitTime,
            };
            commonStatus.WriteCommon(dto);

            return dto;
        }

        public bool SelectExistsLauncherRedoItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.QueryFirst<bool>(statement, parameter);
        }

        public LauncherRedoData SelectLauncherRedoItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QueryFirst<LauncherRedoItemsDto>(statement, parameter);
            return ConvertFromDto(dto);
        }

        public bool InsertRedoItem(Guid launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);
            return Commander.Execute(statement, dto) == 1;
        }


        public bool UpdateRedoItem(Guid launcherItemId, IReadOnlyLauncherRedoData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(launcherItemId, data, databaseCommonStatus);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool DeleteRedoItemByLauncherItemId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
