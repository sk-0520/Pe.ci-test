using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Model.Database;
using ContentTypeTextNet.Pe.Main.Model.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class ScreensEntityDao : EntityDaoBase
    {
        public ScreensEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        public ScreensEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILogger logger) : base(commander, statementLoader, implementation, logger)
        { }

        #region function

        public bool SelectExistsScreen(string screenName)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                ScreenName = screenName,
            };
            return Commander.QuerySingle<bool>(statement, param);
        }

        public bool InsertScreen(Screen screen, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var dto = new ScreensRowDto() {
                ScreenName = screen.DeviceName,
                ScreenX = (long)screen.DeviceBounds.X,
                ScreenY = (long)screen.DeviceBounds.Y,
                ScreenWidth = (long)screen.DeviceBounds.Width,
                ScreenHeight = (long)screen.DeviceBounds.Height,
            };
            commonStatus.WriteCommon(dto);

            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
