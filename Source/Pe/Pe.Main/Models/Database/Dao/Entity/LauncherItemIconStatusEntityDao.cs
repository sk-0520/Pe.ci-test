using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class launcherItemIconStatusEntityDao : EntityDaoBase
    {
        public launcherItemIconStatusEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId => "LauncherItemId";
            public static string IconBox => "IconBox";
            public static string LastUpdatedTimestamp => "LastUpdatedTimestamp";

            #endregion
        }

        #endregion

        #region function

        public bool SelecteExistLauncherItemIconState(Guid launcherItemId, IconBox iconBox)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox)
            };
            return Commander.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public bool InsertLastUpdatedIconTimestamp(Guid launcherItemId, IconBox iconBox, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconBox);
            parameter[Column.LastUpdatedTimestamp] = timestamp;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateLastUpdatedIconTimestamp(Guid launcherItemId, IconBox iconBox, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconBox);
            parameter[Column.LastUpdatedTimestamp] = timestamp;
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
