using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.View.Extend;

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

        LauncherToolbarsDisplayData ConvertFromDto(LauncherToolbarsDisplayRowDto dto)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var result = new LauncherToolbarsDisplayData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                LauncherGroupId = dto.LauncherGroupId,
                ToolbarPosition = toolbarPositionTransfer.From(dto.PositionKind),
                IconScale = iconScaleTransfer.From(dto.IconScale),
                FontId = dto.FontId,
                AutoHideTimeout = ToTimespan(dto.AutoHideTimeout),
                TextWidth = dto.TextWidth,
                IsVisible = dto.IsVisible,
                IsTopmost = dto.IsTopmost,
                IsAutoHide = dto.IsAutoHide,
                IsIconOnly = dto.IsIconOnly,
            };

            return result;
        }

        public IEnumerable<LauncherToolbarsScreenData> SelectAllToolbars()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<LauncherToolbarsScreenRowDto>(sql)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public LauncherToolbarsDisplayData SelectDisplayData(Guid launcherToolbarId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Commander.QuerySingle<LauncherToolbarsDisplayRowDto>(sql, param);
            var data = ConvertFromDto(dto);
            return data;
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
