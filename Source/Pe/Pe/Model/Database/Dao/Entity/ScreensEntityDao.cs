using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class ScreensEntityDao : EntityDaoBase
    {
        public ScreensEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public bool SelectExistsScreen(string screenName)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                ScreenName = screenName,
            };
            return Commander.QuerySingle<bool>(sql, param);
        }

        public bool InsertScreen(Screen screen, IDatabaseCommonStatus commonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = new ScreensRowDto() {
                ScreenName = screen.DeviceName,
                ScreenX = (long)screen.DeviceBounds.X,
                ScreenY = (long)screen.DeviceBounds.Y,
                ScreenWidth = (long)screen.DeviceBounds.Width,
                ScreenHeight = (long)screen.DeviceBounds.Height,
            };
            commonStatus.WriteCommon(dto);

            return Commander.Execute(sql, dto) == 1;
        }

        #endregion
    }
}
