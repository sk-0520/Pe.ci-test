using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherToobarSettingEditorElement : ElementBase, ILauncherToolbarId
    {
        public LauncherToobarSettingEditorElement(Guid launcherToolbarId, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherToolbarId = launcherToolbarId;

            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public FontElement? Font { get; private set; }
        public Guid LauncherGroupId { get; set; }
        public AppDesktopToolbarPosition ToolbarPosition { get; set; }
        public LauncherToolbarIconDirection IconDirection { get; set; }
        public IconBox IconBox { get; set; }
        public TimeSpan AutoHideTimeout { get; set; }
        public int TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }
        #endregion

        #region function

        public void Save(DatabaseCommandPack commadPack)
        {
            var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commadPack.Main.Commander, StatementLoader, commadPack.Main.Implementation, LoggerFactory);
            var data = new LauncherToolbarsDisplayData() {
                LauncherToolbarId = LauncherToolbarId,
                LauncherGroupId = LauncherGroupId,
                ToolbarPosition = ToolbarPosition,
                IconDirection = IconDirection,
                IconBox = IconBox,
                AutoHideTimeout = AutoHideTimeout,
                TextWidth = TextWidth,
                IsVisible = IsVisible,
                IsTopmost = IsTopmost,
                IsAutoHide = IsAutoHide,
                IsIconOnly = IsIconOnly,
            };
            launcherToolbarsEntityDao.UpdateDisplayData(data, commadPack.CommonStatus);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LauncherToolbarsDisplayData data;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                data = launcherToolbarsEntityDao.SelectDisplayData(LauncherToolbarId);
            }

            Font = new FontElement(data.FontId, MainDatabaseBarrier, StatementLoader, LoggerFactory);
            Font.Initialize();

            LauncherGroupId = data.LauncherGroupId;

            ToolbarPosition = data.ToolbarPosition;
            IconDirection = data.IconDirection;
            IconBox = data.IconBox;
            AutoHideTimeout = data.AutoHideTimeout;
            TextWidth = data.TextWidth;
            IsVisible = data.IsVisible;
            IsTopmost = data.IsTopmost;
            IsAutoHide = data.IsAutoHide;
            IsIconOnly = data.IsIconOnly;
        }

        #endregion

        #region ILauncherToolbarId

        public Guid LauncherToolbarId { get; }

        #endregion

    }
}
