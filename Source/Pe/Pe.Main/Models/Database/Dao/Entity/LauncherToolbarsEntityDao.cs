using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherToolbarsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherToolbarsDisplayRowDto: CommonDtoBase
        {
            #region property

            public LauncherToolbarId LauncherToolbarId { get; set; }
            public LauncherGroupId LauncherGroupId { get; set; }
            public string PositionKind { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string IconBox { get; set; } = string.Empty;
            public FontId FontId { get; set; }
            public TimeSpan DisplayDelayTime { get; set; }
            public TimeSpan AutoHideTime { get; set; }
            public long TextWidth { get; set; }
            public bool IsVisible { get; set; }
            public bool IsTopmost { get; set; }
            public bool IsAutoHide { get; set; }
            public bool IsIconOnly { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherToolbarId { get; } = "LauncherToolbarId";
            public static string ScreenName { get; } = "ScreenName";
            public static string PositionKind { get; } = "PositionKind";
            public static string FontId { get; } = "FontId";
            public static string IsTopmost { get; } = "IsTopmost";
            public static string IsAutoHide { get; } = "IsAutoHide";
            public static string IsVisible { get; } = "IsVisible";

            #endregion
        }

        #endregion

        public LauncherToolbarsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherToolbarsDisplayData ConvertFromDto(LauncherToolbarsDisplayRowDto dto)
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
                DisplayDelayTime = dto.DisplayDelayTime,
                AutoHideTime = dto.AutoHideTime,
                TextWidth = ToInt(dto.TextWidth),
                IsVisible = dto.IsVisible,
                IsTopmost = dto.IsTopmost,
                IsAutoHide = dto.IsAutoHide,
                IsIconOnly = dto.IsIconOnly,
            };

            return result;
        }

        private LauncherToolbarsDisplayRowDto ConvertFromData(LauncherToolbarsDisplayData data, IDatabaseCommonStatus commonStatus)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();
            var iconBoxTransfer = new EnumTransfer<IconBox>();
            var iconDirectionTransfer = new EnumTransfer<LauncherToolbarIconDirection>();

            var result = new LauncherToolbarsDisplayRowDto() {
                LauncherToolbarId = data.LauncherToolbarId,
                LauncherGroupId = data.LauncherGroupId,
                PositionKind = toolbarPositionTransfer.ToString(data.ToolbarPosition),
                Direction = iconDirectionTransfer.ToString(data.IconDirection),
                IconBox = iconBoxTransfer.ToString(data.IconBox),
                FontId = data.FontId,
                DisplayDelayTime = data.DisplayDelayTime,
                AutoHideTime = data.AutoHideTime,
                TextWidth = data.TextWidth,
                IsVisible = data.IsVisible,
                IsTopmost = data.IsTopmost,
                IsAutoHide = data.IsAutoHide,
                IsIconOnly = data.IsIconOnly,
            };
            commonStatus.WriteCommonTo(result);
            return result;
        }

        public IEnumerable<LauncherToolbarId> SelectAllLauncherToolbarIds()
        {
            var statement = LoadStatement();
            return Context.Query<LauncherToolbarId>(statement);
        }

        public LauncherToolbarsDisplayData SelectDisplayData(LauncherToolbarId launcherToolbarId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Context.QueryFirst<LauncherToolbarsDisplayRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertNewToolbar(LauncherToolbarId toolbarId, FontId fontId, string? screenName, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = toolbarId;
            param[Column.FontId] = fontId;
            param[Column.ScreenName] = screenName ?? string.Empty;

            Context.InsertSingle(statement, param);
        }

        public bool UpdateToolbarPosition(LauncherToolbarId launcherToolbarId, AppDesktopToolbarPosition toolbarPosition, IDatabaseCommonStatus commonStatus)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();

            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.PositionKind] = toolbarPositionTransfer.ToString(toolbarPosition);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateIsTopmost(LauncherToolbarId launcherToolbarId, bool isTopmost, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsTopmost] = isTopmost;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateIsAutoHide(LauncherToolbarId launcherToolbarId, bool isAutoHide, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsAutoHide] = isAutoHide;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateIsVisible(LauncherToolbarId launcherToolbarId, bool isVisible, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsVisible] = isVisible;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public void UpdateDisplayData(LauncherToolbarsDisplayData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);

            Context.UpdateByKey(statement, dto);
        }
        #endregion
    }
}
