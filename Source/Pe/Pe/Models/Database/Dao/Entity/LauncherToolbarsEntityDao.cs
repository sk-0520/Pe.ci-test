using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
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
            var iconBoxTransfer = new EnumTransfer<IconBox>();
            var iconDirectionTransfer = new EnumTransfer<LauncherToolbarIconDirection>();

            var result = new LauncherToolbarsDisplayData() {
                LauncherToolbarId = dto.LauncherToolbarId,
                LauncherGroupId = dto.LauncherGroupId,
                ToolbarPosition = toolbarPositionTransfer.ToEnum(dto.PositionKind),
                IconDirection = iconDirectionTransfer.ToEnum(dto.Direction),
                IconBox = iconBoxTransfer.ToEnum(dto.IconBox),
                FontId = dto.FontId,
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                AutoHideTimeout = ToTimespan(dto.AutoHideTimeout),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
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
            var statement = LoadStatement();
            var param = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Commander.QuerySingle<LauncherToolbarsDisplayRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public bool InsertNewToolbar(Guid toolbarId, string screenName, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = toolbarId;
            param[Column.ScreenName] = screenName;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateToolbarPosition(Guid launcherToolbarId, AppDesktopToolbarPosition toolbarPosition, IDatabaseCommonStatus commonStatus)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();

            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.PositionKind] = toolbarPositionTransfer.ToString(toolbarPosition);

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsTopmost(Guid launcherToolbarId, bool isTopmost, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsTopmost] = isTopmost;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsAutoHide(Guid launcherToolbarId, bool isAutoHide, DatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsAutoHide] = isAutoHide;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdatIsVisible(Guid launcherToolbarId, bool isVisible, DatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsVisible] = isVisible;

            return Commander.Execute(statement, param) == 1;
        }


        #endregion
    }
}
