using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class GeneralSettingEditorElementBase: ElementBase
    {
        protected GeneralSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        #endregion

        #region function

        public void Save(IDatabaseContextsPack contextsPack)
        {
            SaveImpl(contextsPack);
        }

        protected abstract void SaveImpl(IDatabaseContextsPack contextsPack);

        #endregion
    }

    public class AppExecuteSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppExecuteSettingEditorElement(EnvironmentParameters environmentParameters, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        public string UserId { get; set; } = string.Empty;
        public bool IsEnabledTelemetry { get; set; }


        #endregion

        #region function

        public void OpenPrivacyPolicyDocument()
        {
            try {
                // パラメータを渡せないのでしゃあない
                var systemExecutor = new SystemExecutor();
                systemExecutor.ExecuteFile(EnvironmentParameters.HelpFile.FullName);
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
        }

        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppExecuteSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
            }

            UserId = setting.UserId;
            IsEnabledTelemetry = setting.IsEnabledTelemetry;

            var userIdManager = new UserIdManager(LoggerFactory);
            if(!userIdManager.IsValidUserId(UserId)) {
                UserId = userIdManager.CreateFromEnvironment();
            }
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppExecuteSettingData() {
                IsEnabledTelemetry = IsEnabledTelemetry,
                UserId = UserId,
            };
            appExecuteSettingEntityDao.UpdateSettingExecuteSetting(data, contextsPack.CommonStatus);
        }

        #endregion
    }


    public class AppGeneralSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppGeneralSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IReadOnlyList<IPlugin> themePlugins, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            ThemePlugins = themePlugins;
        }

        #region property

        public IReadOnlyList<IPlugin> ThemePlugins { get; }

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
        public string UserBackupDirectoryPath { get; set; } = string.Empty;
        public Guid ThemePluginId { get; set; }

        public bool IsRegisterStartup { get; set; }
        public bool DelayStartup { get; set; }
        public TimeSpan StartupWaitTime { get; set; }
        public string StartupArgument { get; set; } = string.Empty;


        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppGeneralSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appGeneralSettingEntityDao.SelectSettingGeneralSetting();
            }

            if(string.IsNullOrWhiteSpace(setting.Language)) {
                CultureInfo = CultureInfo.InvariantCulture;
            } else {
                CultureInfo = CultureInfo.GetCultureInfo(setting.Language);
            }

            UserBackupDirectoryPath = setting.UserBackupDirectoryPath;
            ThemePluginId = setting.ThemePluginId;

            // スタートアップ取得
            var startupRegister = new StartupRegister(LoggerFactory);
            var startupParameterResult = startupRegister.GetStartupParameter();
            if(startupParameterResult.Success) {
                var startupParameter = startupParameterResult.SuccessValue!;
                IsRegisterStartup = true;
                DelayStartup = startupParameter.DelayStartup;
                StartupWaitTime = startupParameter.StartupWaitTime;
                StartupArgument = startupParameter.Argument;
            } else {
                IsRegisterStartup = false;
                DelayStartup = false;
                StartupWaitTime = TimeSpan.FromSeconds(3);
                StartupArgument = string.Empty;
            }
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppGeneralSettingData() {
                Language = CultureInfo.Name,
                UserBackupDirectoryPath = UserBackupDirectoryPath,
                ThemePluginId = ThemePluginId,
            };
            appGeneralSettingEntityDao.UpdateSettingGeneralSetting(data, contextsPack.CommonStatus);

            var startupRegister = new StartupRegister(LoggerFactory);
            if(IsRegisterStartup) {
                var startupParameter = new StartupParameter() {
                    DelayStartup = DelayStartup,
                    StartupWaitTime = StartupWaitTime,
                    Argument = StartupArgument,
                };
                startupRegister.Register(startupParameter);
            } else {
                startupRegister.Unregister();
            }
        }

        #endregion
    }


    public class AppUpdateSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppUpdateSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        public UpdateKind UpdateKind { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppUpdateSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appUpdateSettingEntityDao.SelectSettingUpdateSetting();
            }

            UpdateKind = setting.UpdateKind;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppUpdateSettingData() {
                UpdateKind = UpdateKind,
            };
            appUpdateSettingEntityDao.UpdateSettingUpdateSetting(data, contextsPack.CommonStatus);
        }

        #endregion
    }

    public class AppNotifyLogSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppNotifyLogSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        public bool IsVisible { get; set; }
        public NotifyLogPosition Position { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            var setting = MainDatabaseBarrier.ReadData(c => {
                var dao = new AppNotifyLogSettingEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return dao.SelectSettingNotifyLogSetting();
            });

            IsVisible = setting.IsVisible;
            Position = setting.Position;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var appNotifyLogSettingEntityDao = new AppNotifyLogSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppNotifyLogSettingData() {
                IsVisible = IsVisible,
                Position = Position,
            };
            appNotifyLogSettingEntityDao.UpdateSettingNotifyLogSetting(data, contextsPack.CommonStatus);
        }

        #endregion
    }

    public class AppLauncherToolbarSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppLauncherToolbarSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        public LauncherToolbarContentDropMode ContentDropMode { get; set; }
        public LauncherGroupPosition GroupMenuPosition { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            var setting = MainDatabaseBarrier.ReadData(c => {
                var appLauncherToolbarSettingEntityDao = new AppLauncherToolbarSettingEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return appLauncherToolbarSettingEntityDao.SelectSettingLauncherToolbarSetting();
            });

            ContentDropMode = setting.ContentDropMode;
            GroupMenuPosition = setting.GroupMenuPosition;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            var appLauncherToolbarSettingEntityDao = new AppLauncherToolbarSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new AppLauncherToolbarSettingData() {
                ContentDropMode = ContentDropMode,
                GroupMenuPosition = GroupMenuPosition,
            };

            appLauncherToolbarSettingEntityDao.UpdateSettingLauncherToolbarSetting(data, contextsPack.CommonStatus);
        }

        #endregion
    }


    public class AppCommandSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppCommandSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        //public Guid FontId { get; set; }
        public FontElement? Font { get; private set; }
        public IconBox IconBox { get; set; }
        public double Width { get; set; }
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppCommandSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appCommandSettingEntityDao = new AppCommandSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appCommandSettingEntityDao.SelectSettingCommandSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            Font.Initialize();

            IconBox = setting.IconBox;
            Width = setting.Width;
            HideWaitTime = setting.HideWaitTime;
            FindTag = setting.FindTag;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            Debug.Assert(Font != null);

            var appCommandSettingEntityDao = new AppCommandSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppCommandSettingData() {
                FontId = Font.FontId,
                IconBox = IconBox,
                Width = Width,
                HideWaitTime = HideWaitTime,
                FindTag = FindTag,
            };
            appCommandSettingEntityDao.UpdateSettingCommandSetting(data, contextsPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, contextsPack.CommonStatus);

        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }



    public class AppNoteSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppNoteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        public FontElement? Font { get; private set; }
        public NoteCreateTitleKind TitleKind { get; set; }
        public NoteLayoutKind LayoutKind { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsTopmost { get; set; }
        public NoteCaptionPosition CaptionPosition { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppNoteSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appNoteSettingEntityDao = new AppNoteSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appNoteSettingEntityDao.SelectSettingNoteSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            Font.Initialize();

            TitleKind = setting.TitleKind;
            LayoutKind = setting.LayoutKind;
            ForegroundColor = setting.ForegroundColor;
            BackgroundColor = setting.BackgroundColor;
            IsTopmost = setting.IsTopmost;
            CaptionPosition = setting.CaptionPosition;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            Debug.Assert(Font != null);

            var appNoteSettingEntityDao = new AppNoteSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppNoteSettingData() {
                FontId = Font.FontId,
                TitleKind = TitleKind,
                LayoutKind = LayoutKind,
                ForegroundColor = ForegroundColor,
                BackgroundColor = BackgroundColor,
                IsTopmost = IsTopmost,
                CaptionPosition = CaptionPosition,
            };
            appNoteSettingEntityDao.UpdateSettingNoteSetting(data, contextsPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, contextsPack.CommonStatus);

        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }



    public class AppStandardInputOutputSettingEditorElement: GeneralSettingEditorElementBase
    {
        public AppStandardInputOutputSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        #region property

        public FontElement? Font { get; private set; }
        public Color OutputForegroundColor { get; set; }
        public Color OutputBackgroundColor { get; set; }
        public Color ErrorForegroundColor { get; set; }
        public Color ErrorBackgroundColor { get; set; }
        public bool IsTopmost { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppStandardInputOutputSettingData setting;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = appStandardInputOutputSettingEntityDao.SelectSettingStandardInputOutputSetting();
            }

            Font = new FontElement(setting.FontId, MainDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            Font.Initialize();

            OutputForegroundColor = setting.OutputForegroundColor;
            OutputBackgroundColor = setting.OutputBackgroundColor;
            ErrorForegroundColor = setting.ErrorForegroundColor;
            ErrorBackgroundColor = setting.ErrorBackgroundColor;
            IsTopmost = setting.IsTopmost;
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            Debug.Assert(Font != null);

            var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppStandardInputOutputSettingData() {
                FontId = Font.FontId,
                OutputForegroundColor = OutputForegroundColor,
                OutputBackgroundColor = OutputBackgroundColor,
                ErrorForegroundColor = ErrorForegroundColor,
                ErrorBackgroundColor = ErrorBackgroundColor,
                IsTopmost = IsTopmost,
            };
            appStandardInputOutputSettingEntityDao.UpdateSettingStandardInputOutputSetting(data, contextsPack.CommonStatus);

            var fontsEntityDao = new FontsEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);
            fontsEntityDao.UpdateFont(Font.FontId, Font.FontData, contextsPack.CommonStatus);

        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }



}
