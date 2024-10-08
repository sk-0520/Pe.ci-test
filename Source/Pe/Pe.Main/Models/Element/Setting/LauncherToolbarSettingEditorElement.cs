using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherToolbarSettingEditorElement: ElementBase, ILauncherToolbarId
    {
        public LauncherToolbarSettingEditorElement(LauncherToolbarId launcherToolbarId, ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherToolbarId = launcherToolbarId;
            AllLauncherGroups = allLauncherGroups;

            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property
        private ObservableCollection<LauncherGroupSettingEditorElement> AllLauncherGroups { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        public FontElement? Font { get; private set; }
        public LauncherGroupId LauncherGroupId { get; set; }
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        public LauncherToolbarIconDirection IconDirection { get; set; }
        public IconBox IconBox { get; set; }
        public TimeSpan DisplayDelayTime { get; set; }
        public TimeSpan AutoHideTime { get; set; }
        public int TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        public IScreen? Screen { get; set; }
        public string ScreenName { get; set; } = string.Empty;

        #endregion

        #region function

        public void Save(IDatabaseContextsPack commandPack)
        {
            Debug.Assert(Font != null);

            var fontsEntityDao = new FontsEntityDao(commandPack.Main.Context, DatabaseStatementLoader, commandPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, commandPack.CommonStatus);

            var defaultLauncherGroupId = LauncherGroupId;
            if(defaultLauncherGroupId != LauncherGroupId.Empty) {
                if(!AllLauncherGroups.Any(i => i.LauncherGroupId == defaultLauncherGroupId)) {
                    Logger.LogTrace("存在しないランチャーグループIDのため補正: {0}", defaultLauncherGroupId);
                    defaultLauncherGroupId = LauncherGroupId.Empty;
                }
            }

            var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commandPack.Main.Context, DatabaseStatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new LauncherToolbarsDisplayData() {
                LauncherToolbarId = LauncherToolbarId,
                FontId = Font.FontId,
                LauncherGroupId = defaultLauncherGroupId,
                ToolbarPosition = ToolbarPosition,
                IconDirection = IconDirection,
                IconBox = IconBox,
                DisplayDelayTime = DisplayDelayTime,
                AutoHideTime = AutoHideTime,
                TextWidth = TextWidth,
                IsVisible = IsVisible,
                IsTopmost = IsTopmost,
                IsAutoHide = IsAutoHide,
                IsIconOnly = IsIconOnly,
            };
            launcherToolbarsEntityDao.UpdateDisplayData(data, commandPack.CommonStatus);
        }

        #endregion

        #region ElementBase

        protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            LauncherToolbarsDisplayData data;
            LauncherToolbarsScreenData screenToolbar;

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                data = launcherToolbarsEntityDao.SelectDisplayData(LauncherToolbarId);

                var launcherToolbarDomainDao = new LauncherToolbarDomainDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                screenToolbar = launcherToolbarDomainDao.SelectScreenToolbar(LauncherToolbarId);
            }

            ScreenName = screenToolbar.ScreenName;

            Font = new FontElement(data.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            await Font.InitializeAsync(cancellationToken);

            LauncherGroupId = data.LauncherGroupId;

            ToolbarPosition = data.ToolbarPosition;
            IconDirection = data.IconDirection;
            IconBox = data.IconBox;
            DisplayDelayTime = data.DisplayDelayTime;
            AutoHideTime = data.AutoHideTime;
            TextWidth = data.TextWidth;
            IsVisible = data.IsVisible;
            IsTopmost = data.IsTopmost;
            IsAutoHide = data.IsAutoHide;
            IsIconOnly = data.IsIconOnly;

            var screens = ContentTypeTextNet.Pe.Core.Compatibility.Forms.Screen.AllScreens;
            var screenChecker = new ScreenChecker();
            foreach(var screen in screens) {
                if(screenChecker.FindMaybe(screen, screenToolbar)) {
                    Screen = screen;
                    break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                    Font = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherToolbarId

        public LauncherToolbarId LauncherToolbarId { get; }

        #endregion
    }
}
