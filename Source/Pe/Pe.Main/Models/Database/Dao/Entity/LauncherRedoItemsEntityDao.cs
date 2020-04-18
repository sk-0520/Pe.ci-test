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
        public bool IsEnabled { get; set; }
        public TimeSpan WaitTime { get; set; }
        public string SuccessExitCodeRange { get; set; } = string.Empty;

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

        public bool SelectExistsLauncherRedoItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.QueryFirst<bool>(statement, parameter);
        }

        #endregion
    }
}
