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
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherToolbarsDao : ApplicationDatabaseObjectBase
    {
        public LauncherToolbarsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        LauncherToolbarsScreenData ConvertFromDto(LauncherToolbarsScreenRowDto dto)
        {
            var data = new LauncherToolbarsScreenData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                ScreenName = dto.ScreenName,
                X = dto.ScreenX,
                Y = dto.ScreenY,
                Height = dto.ScreenHeight,
                Width = dto.ScreenWidth,
            };

            return data;
        }

        public IEnumerable<LauncherToolbarsScreenData> SelectAllToolbars()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<LauncherToolbarsScreenRowDto>(sql)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public LauncherToolbarsDisplayData SelectDisplayData(Guid launcherToolbar)
        {
            throw new NotImplementedException();
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
