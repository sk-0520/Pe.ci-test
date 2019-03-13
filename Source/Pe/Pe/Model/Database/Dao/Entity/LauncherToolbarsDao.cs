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
    public class LauncherToolbarsDao : ApplicationDatabaseObjectBase
    {
        public LauncherToolbarsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        public IEnumerable<LauncherToolbarsScreenRowDto> SelectAllToolbars()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<LauncherToolbarsScreenRowDto>(sql);
        }

        public bool InsertNewToolbar(Guid toolbarId, Screen screen)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = new LauncherToolbarsScreenRowDto() {
                LauncherToolbarId = toolbarId,
                ScreenName = screen.DeviceName,
                ScreenX = (long)screen.DeviceBounds.X,
                ScreenY = (long)screen.DeviceBounds.Y,
                ScreenWidth = (long)screen.DeviceBounds.Width,
                ScreenHeight = (long)screen.DeviceBounds.Height,
            };

            var status = DatabaseCommonStatus.CreateUser();
            status.WriteCommon(dto);

            return Commander.Execute(sql, dto) == 1;
        }

        #endregion
    }
}
