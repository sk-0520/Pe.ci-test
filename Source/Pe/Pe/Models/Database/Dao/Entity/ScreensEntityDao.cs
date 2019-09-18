using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class ScreensEntityDao : EntityDaoBase
    {
        public ScreensEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
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
