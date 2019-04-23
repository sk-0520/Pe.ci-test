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
    public class LauncherToolbarsEntityDao : EntityDaoBase
    {
        public LauncherToolbarsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherToolbarId { get; } = "LauncherToolbarId";
            public static string ScreenName { get; } = "ScreenName";
            public static string PositionKind { get; } = "PositionKind";
            public static string IsTopmost { get; } = "IsTopmost";
            public static string IsAutoHide { get; } = "IsAutoHide";
            public static string IsVisible { get; } = "IsVisible";

            #endregion
        }

        #endregion

        #region function


        LauncherToolbarsDisplayData ConvertFromDto(LauncherToolbarsDisplayRowDto dto)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();
            var iconScaleTransfer = new EnumTransfer<IconScale>();
            var iconDirectionTransfer = new EnumTransfer<LauncherToolbarIconDirection>();

            var result = new LauncherToolbarsDisplayData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                LauncherGroupId = dto.LauncherGroupId,
                ToolbarPosition = toolbarPositionTransfer.ToEnum(dto.PositionKind),
                IconDirection = iconDirectionTransfer.ToEnum(dto.Direction),
                IconScale = iconScaleTransfer.ToEnum(dto.IconScale),
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

        public LauncherToolbarsDisplayData SelectDisplayData(Guid launcherToolbarId)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Commander.QuerySingle<LauncherToolbarsDisplayRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public bool InsertNewToolbar(Guid toolbarId, string screenName, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = toolbarId;
            param[Column.ScreenName] = screenName;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateToolbarPosition(Guid launcherToolbarId, AppDesktopToolbarPosition toolbarPosition, IDatabaseCommonStatus commonStatus)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();

            var statement = StatementLoader.LoadStatementByCurrent();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.PositionKind] = toolbarPositionTransfer.ToText(toolbarPosition);

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsTopmost(Guid launcherToolbarId, bool isTopmost, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsTopmost] = isTopmost;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsAutoHide(Guid launcherToolbarId, bool isAutoHide, DatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsAutoHide] = isAutoHide;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsVisible(Guid launcherToolbarId, bool isVisible, DatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsVisible] = isVisible;

            return Commander.Execute(statement, param) == 1;
        }


        #endregion
    }
}
