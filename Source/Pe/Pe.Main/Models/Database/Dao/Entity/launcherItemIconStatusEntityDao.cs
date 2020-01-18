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
    public class LauncherItemIconStatusEntityDao : EntityDaoBase
    {
        public LauncherItemIconStatusEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


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
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox),
                LastUpdatedTimestamp = timestamp,
            };
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateLastUpdatedIconTimestamp(Guid launcherItemId, IconBox iconBox, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox),
                LastUpdatedTimestamp = timestamp,
            };
            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
