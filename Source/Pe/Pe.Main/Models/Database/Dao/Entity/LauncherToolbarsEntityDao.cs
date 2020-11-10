using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherToolbarsEntityDao : EntityDaoBase
    {
        #region define

        private class LauncherToolbarsDisplayRowDto: CommonDtoBase
        {
            #region property

            public Guid LauncherToolbarId { get; set; }
            public Guid LauncherGroupId { get; set; }
            public string PositionKind { get; set; } = string.Empty;
            public string Direction { get; set; } = string.Empty;
            public string IconBox { get; set; } = string.Empty;
            public Guid FontId { get; set; }
            public TimeSpan AutoHideTime { get; set; }
            public long TextWidth { get; set; }
            public bool IsVisible { get; set; }
            public bool IsTopmost { get; set; }
            public bool IsAutoHide { get; set; }
            public bool IsIconOnly { get; set; }

            #endregion
        }

        #endregion

        public LauncherToolbarsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
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
                AutoHideTime = dto.AutoHideTime,
                TextWidth = ToInt(dto.TextWidth),
                IsVisible = dto.IsVisible,
                IsTopmost = dto.IsTopmost,
                IsAutoHide = dto.IsAutoHide,
                IsIconOnly = dto.IsIconOnly,
            };

            return result;
        }

        LauncherToolbarsDisplayRowDto ConvertFromData(LauncherToolbarsDisplayData data, IDatabaseCommonStatus commonStatus)
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
                AutoHideTime = data.AutoHideTime,
                TextWidth = data.TextWidth,
                IsVisible = data.IsVisible,
                IsTopmost = data.IsTopmost,
                IsAutoHide = data.IsAutoHide,
                IsIconOnly = data.IsIconOnly,
            };
            commonStatus.WriteCommon(result);
            return result;
        }

        public IEnumerable<Guid> SelectAllLauncherToolbarIds()
        {
            var statement = LoadStatement();
            return Context.Query<Guid>(statement);
        }

        public LauncherToolbarsDisplayData SelectDisplayData(Guid launcherToolbarId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherToolbarId = launcherToolbarId,
            };
            var dto = Context.QueryFirst<LauncherToolbarsDisplayRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public string SelectScreenName(Guid launcherToolbarId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherToolbarId = launcherToolbarId,
            };
            return Context.QueryFirst<string>(statement, parameter);
        }

        public bool InsertNewToolbar(Guid toolbarId, Guid fontId, string? screenName, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = toolbarId;
            param[Column.FontId] = fontId;
            param[Column.ScreenName] = screenName ?? string.Empty;

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateToolbarPosition(Guid launcherToolbarId, AppDesktopToolbarPosition toolbarPosition, IDatabaseCommonStatus commonStatus)
        {
            var toolbarPositionTransfer = new EnumTransfer<AppDesktopToolbarPosition>();

            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.PositionKind] = toolbarPositionTransfer.ToString(toolbarPosition);

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdatIsTopmost(Guid launcherToolbarId, bool isTopmost, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsTopmost] = isTopmost;

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdatIsAutoHide(Guid launcherToolbarId, bool isAutoHide, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsAutoHide] = isAutoHide;

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdatIsVisible(Guid launcherToolbarId, bool isVisible, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();

            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherToolbarId] = launcherToolbarId;
            param[Column.IsVisible] = isVisible;

            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateDisplayData(LauncherToolbarsDisplayData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            return Context.Execute(statement, dto) == 1;
        }
        #endregion
    }
}
