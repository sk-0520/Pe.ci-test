using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using System.Threading;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using System.IO;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using System.IO.Compression;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using System.Text.Json;
using System.Reflection;
using ContentTypeTextNet.Pe.Main.CrashReport.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    internal partial class ApplicationManager: DisposerBase, IOrderManager
    {
        internal ApplicationManager(ApplicationInitializer initializer)
        {
            Logging = initializer.Logging ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Logging));
            Logger = Logging.Factory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;

            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));
            PlatformThemeLoader = ApplicationDiContainer.Build<PlatformThemeLoader>();
            PlatformThemeLoader.Changed += PlatformThemeLoader_Changed;
            ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(PlatformThemeLoader);

            WindowManager = initializer.WindowManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.WindowManager));
            OrderManager = ApplicationDiContainer.Build<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManagerImpl = initializer.NotifyManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.NotifyManager));
            StatusManagerImpl = initializer.StatusManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.StatusManager));
            ClipboardManager = initializer.ClipboardManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.ClipboardManager));
            UserAgentManager = initializer.UserAgentManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.UserAgentManager));
            ApplicationMutex = initializer.Mutex ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Mutex));

            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManagerImpl);
            ApplicationDiContainer.Register<IStatusManager, StatusManager>(StatusManagerImpl);
            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
            ApplicationDiContainer.Register<IUserAgentManager, UserAgentManager>(UserAgentManager);
            ApplicationDiContainer.Register<IUserAgentFactory, IUserAgentFactory>(UserAgentManager);

            var addonContainer = ApplicationDiContainer.Build<AddonContainer>();
            var themeContainer = ApplicationDiContainer.Build<ThemeContainer>();
            PluginContainer = ApplicationDiContainer.Build<PluginContainer>(addonContainer, themeContainer);

            ApplicationDiContainer.Register<IGeneralTheme, IGeneralTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetGeneralTheme());
            ApplicationDiContainer.Register<ILauncherToolbarTheme, ILauncherToolbarTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherToolbarTheme());
            ApplicationDiContainer.Register<ILauncherGroupTheme, ILauncherGroupTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherGroupTheme());
            ApplicationDiContainer.Register<INoteTheme, INoteTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNoteTheme());
            ApplicationDiContainer.Register<ICommandTheme, ICommandTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetCommandTheme());
            ApplicationDiContainer.Register<INotifyLogTheme, INotifyLogTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNotifyTheme());


            KeyboradHooker = new KeyboradHooker(LoggerFactory);
            MouseHooker = new MouseHooker(LoggerFactory);
            KeyActionChecker = new KeyActionChecker(LoggerFactory);
            KeyActionAssistant = new KeyActionAssistant(LoggerFactory);

            var applicationCommandFinder = ApplicationDiContainer.Build<ApplicationCommandFinder>(CreateApplicationCommandParameters());
            CommandElement = ApplicationDiContainer.Build<CommandElement>(applicationCommandFinder);
            ApplicationUpdateInfo = ApplicationDiContainer.Build<UpdateInfo>();
            NotifyLogElement = ApplicationDiContainer.Build<NotifyLogElement>();
            NotifyLogElement.Initialize();

            var platformConfiguration = ApplicationDiContainer.Get<PlatformConfiguration>();
            LazyScreenElementReset = ApplicationDiContainer.Build<LazyAction>(nameof(LazyScreenElementReset), platformConfiguration.ScreenElementsResetWaitTime);
        }

        #region property

        ApplicationLogging Logging { get; set; }
        ILoggerFactory LoggerFactory => Logging.Factory;
        ApplicationDiContainer ApplicationDiContainer { get; set; }
        bool IsFirstStartup { get; }
        ILogger Logger { get; set; }
        PlatformThemeLoader PlatformThemeLoader { get; }

        WindowManager WindowManager { get; set; }
        OrderManagerImpl OrderManager { get; set; }
        NotifyManager NotifyManagerImpl { get; set; }
        public INotifyManager NotifyManager => NotifyManagerImpl;
        StatusManager StatusManagerImpl { get; set; }
        public IStatusManager StatusManager => StatusManagerImpl;
        ClipboardManager ClipboardManager { get; set; }
        UserAgentManager UserAgentManager { get; set; }

        Mutex ApplicationMutex { get; }

        ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();
        ObservableCollection<StandardInputOutputElement> StandardInputOutputs { get; } = new ObservableCollection<StandardInputOutputElement>();
        CommandElement CommandElement { get; }
        NotifyLogElement NotifyLogElement { get; }
        //FeedbackElement? FeedbackElement { get; set; }
        HwndSource? MessageWindowHandleSource { get; set; }
        //IDispatcherWapper? MessageWindowDispatcherWapper { get; set; }

        KeyboradHooker KeyboradHooker { get; }
        MouseHooker MouseHooker { get; }
        KeyActionChecker KeyActionChecker { get; }
        KeyActionAssistant KeyActionAssistant { get; }

        PluginContainer PluginContainer { get; }

        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public UpdateInfo ApplicationUpdateInfo { get; }

        public bool CanCallNotifyAreaMenu { get; private set; }

        public bool CanSendCrashReport => ApplicationDiContainer.Get<GeneralConfiguration>().CanSendCrashReport;
        public bool UnhandledExceptionHandled => ApplicationDiContainer.Get<GeneralConfiguration>().UnhandledExceptionHandled;

        private bool ResetWaiting { get; set; }
        private LazyAction LazyScreenElementReset { get; }

        #endregion

        #region function

        /// <summary>
        /// すべてここで完結する神の所業。
        /// </summary>
        public void ShowSettingView()
        {
            StopPlatform();
            StopHook();
            UninitializeSystem();

            if(CommandElement.ViewCreated) {
                CommandElement.HideView(true);
            }
            if(NotifyLogElement.ViewCreated) {
                NotifyLogElement.HideView(true);
            }
            using var _silent_ = NotifyLogElement.ToSilent();

            var changing = StatusManagerImpl.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            Logger.LogDebug("遅延書き込み処理停止");
            var lazyWriterPack = ApplicationDiContainer.Get<IDatabaseLazyWriterPack>();
            var lazyWriterItemMap = new Dictionary<IDatabaseLazyWriter, IDisposable>();
            foreach(var lazyWriter in lazyWriterPack.Items) {
                lazyWriter.Flush();
                var pausing = lazyWriter.Pause();
                lazyWriterItemMap.Add(lazyWriter, pausing);
            }

            // 現在DBを編集用として再構築
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var settingDirectory = environmentParameters.TemporarySettingDirectory;
            var directoryCleaner = new DirectoryCleaner(settingDirectory, environmentParameters.Configuration.File.DirectoryRemoveWaitCount, environmentParameters.Configuration.File.DirectoryRemoveWaitTime, LoggerFactory);
            directoryCleaner.Clear(false);

            var settings = new {
                Main = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.MainFile.Name)),
                File = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.FileFile.Name)),
            };
            //var settingDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.SettingFile.Name));
            //var fileDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.FileFile.Name));

            environmentParameters.MainFile.CopyTo(settings.Main.FullName);
            environmentParameters.FileFile.CopyTo(settings.File.FullName);

            // DIを設定処理用に付け替え
            var container = ApplicationDiContainer.Scope();
            var factory = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(settings.Main, true, false),
                new ApplicationDatabaseFactory(settings.File, true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            container
                .Register<IDiContainer, DiContainer>((DiContainer)container) // むりやりぃ
                .Register<ISettingNotifyManager, SettingNotifyManager>(DiLifecycle.Singleton)
                .RegisterDatabase(factory, lazyWriterWaitTimePack, LoggerFactory)
            ;


            var settingElement = container.Build<SettingContainerElement>();
            settingElement.Initialize();
            var windowItem = OrderManager.CreateSettingWindow(settingElement);
            WindowManager.Register(windowItem);
            var dialogResult = windowItem.Window.ShowDialog();

            if(settingElement.IsSubmit) {
                Logger.LogInformation("設定適用のため現在表示要素の破棄");
                CloseViews();
                DisposeElements();

                // 設定用DBを永続用DBと切り替え
                var pack = ApplicationDiContainer.Get<IDatabaseAccessorPack>();
                var stoppings = (new IDatabaseAccessor[] { pack.Main, pack.File })
                    .Select(i => i.StopConnection())
                    .ToList()
                ;

                // バックアップ処理開始
                //string userBackupDirectoryPath;
                //using(var commander = container.Get<IMainDatabaseBarrier>().WaitRead()) {
                //    var appGeneralSettingEntityDao = container.Build<AppGeneralSettingEntityDao>(commander, commander.Implementation);
                //    userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
                //}
                //try {
                //    BackupSettingsCore(
                //        environmentParameters.UserSettingDirectory,
                //        environmentParameters.UserBackupDirectory,
                //        DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                //        environmentParameters.Configuration.Backup.SettingCount,
                //        userBackupDirectoryPath
                //    );
                //} catch(Exception ex) {
                //    Logger.LogError(ex, "バックアップ処理失敗: {0}", ex.Message);
                //}
                BackupSettingsDefault(container);

                var accessorPack = container.Get<IDatabaseAccessorPack>();
                var databaseSetupper = container.Build<DatabaseSetupper>();
                foreach(var accessor in accessorPack.Items) {
                    databaseSetupper.Tune(accessor);
                }

                settings.Main.CopyTo(environmentParameters.MainFile.FullName, true);
                settings.File.CopyTo(environmentParameters.FileFile.FullName, true);

                foreach(var stopping in stoppings) {
                    stopping.Dispose();
                }
                var cultureServiceChanger = ApplicationDiContainer.Build<CultureServiceChanger>(CultureService.Instance);
                cultureServiceChanger.ChangeCulture();

                Logger.LogInformation("設定適用のため各要素生成");
                RebuildHook();
                ExecuteElements();
                if(CommandElement.IsInitialized) {
                    CommandElement.Refresh();
                }
                NotifyLogElement.Refresh();
            } else {
                Logger.LogInformation("設定は保存されなかったため現在要素継続");
            }
            StartHook();
            StartPlatform();
            InitializeSystem();

            Logger.LogDebug("遅延書き込み処理再開");
            foreach(var pair in lazyWriterItemMap) {
                if(settingElement.IsSubmit) {
                    // 確定処理の書き込みが天に召されるのでため込んでいた処理(ないはず)を消す
                    pair.Key.ClearStock();
                }
                pair.Value.Dispose();
            }

            if(changing.Success) {
                changing.SuccessValue?.Dispose();
            }

            settingElement.Dispose();
            container.UnregisterDatabase();
            container.Dispose();
        }

        public void ShowStartupView(bool isFirstSetup)
        {
            var changing = StatusManagerImpl.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModels.Startup.StartupViewModel, Views.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                startupModel.Initialize();
                var view = diContainer.Build<Views.Startup.StartupWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.Startup, startupModel, view));

                view.ShowDialog();

                if(!isFirstSetup) {
                    if(startupModel.IsRegisteredLauncher) {
                        ResetScreenViewElements();
                    }
                }
            }

            changing.SuccessValue?.Dispose();
        }

        public void ShowAboutView()
        {
            var changing = StatusManagerImpl.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.About.AboutElement, ViewModels.About.AboutViewModel, Views.About.AboutWindow>()
                ;
                var model = diContainer.New<Element.About.AboutElement>();
                model.Initialize();

                var view = diContainer.Build<Views.About.AboutWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.About, model, view));

                view.ShowDialog();
            }

            changing.SuccessValue?.Dispose();
        }

        public void ShowHelp()
        {
            try {
                var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
                var systemExecutor = ApplicationDiContainer.Build<SystemExecutor>();
                systemExecutor.ExecuteFile(environmentParameters.HelpFile.FullName);
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
        }

        private void ShowUpdateReleaseNote(UpdateItemData updateItem, bool isCheckOnly)
        {
            void Show()
            {
                var element = ApplicationDiContainer.Build<Element.ReleaseNote.ReleaseNoteElement>(ApplicationUpdateInfo, updateItem, isCheckOnly);
                var view = ApplicationDiContainer.Build<Views.ReleaseNote.ReleaseNoteWindow>();
                view.DataContext = ApplicationDiContainer.Build<ViewModels.ReleaseNote.ReleaseNoteViewModel>(element);
                WindowManager.Register(new WindowItem(WindowKind.Release, element, view));
                view.Show();
            }

            var windowItem = WindowManager.GetWindowItems(WindowKind.Release);
            if(windowItem.Any()) {
                // 再表示
                ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                    var window = windowItem.FirstOrDefault();
                    if(window != null) {
                        window.Window.Activate();
                    } else {
                        Show();
                    }
                }, DispatcherPriority.ApplicationIdle);
                return;
            }

            ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                Show();
            }, DispatcherPriority.ApplicationIdle);
        }

        private void LoadPlugins()
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();

            foreach(var plugin in PluginContainer.GetPlugins()) {
                plugin.Initialize(pluginContextFactory.CreateInitializeContext(plugin.PluginInformations.PluginIdentifiers));
                PluginContainer.AddPlugin(plugin);
            }
        }

        private void ApplyCurrentTheme(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();

            PluginContainer.Theme.SetCurrentTheme(pluginIdentifiers, pluginContextFactory);
        }

        private void RunAddons()
        {
            //TODTO: アドオンを実行していく
        }

        void SetStaticPlatformTheme()
        {
            var themes = new[] { PlatformThemeKind.Dark, PlatformThemeKind.Light };
            foreach(var theme in themes) {
                var themeKey = theme.ToString();
                var colors = PlatformThemeLoader.GetApplicationThemeColors(theme);
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BackgroundColor"] = colors.Background;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ForegroundColor"] = colors.Foreground;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ControlColor"] = colors.Control;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BorderColor"] = colors.Border;

                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BackgroundBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Background));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ForegroundBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Foreground));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ControlBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Control));
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BorderBrush"] = FreezableUtility.GetSafeFreeze(new SolidColorBrush(colors.Border));

            }
        }

        void SetDynamicPlatformTheme()
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().VerifyAccess();

            var colors = PlatformThemeLoader.ApplicationThemeKind switch
            {
                PlatformThemeKind.Dark => (active: "Dark", inactive: "Light"),
                PlatformThemeKind.Light => (active: "Light", inactive: "Dark"),
                _ => throw new NotImplementedException(),
            };

            Application.Current.Resources["PlatformTheme-ThemeColors-BackgroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BackgroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ForegroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ForegroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ControlColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ControlColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors-BorderColor"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BorderColor"];

            Application.Current.Resources["PlatformTheme-ThemeColors2-BackgroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BackgroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ForegroundColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ForegroundColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ControlColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ControlColor"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-BorderColor"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BorderColor"];

            Application.Current.Resources["PlatformTheme-ThemeColors-BackgroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BackgroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ForegroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ForegroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-ControlBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-ControlBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors-BorderBrush"] = Application.Current.Resources["PlatformTheme-" + colors.active + "ThemeColors-BorderBrush"];

            Application.Current.Resources["PlatformTheme-ThemeColors2-BackgroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BackgroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ForegroundBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ForegroundBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-ControlBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-ControlBrush"];
            Application.Current.Resources["PlatformTheme-ThemeColors2-BorderBrush"] = Application.Current.Resources["PlatformTheme-" + colors.inactive + "ThemeColors-BorderBrush"];


            var accent = PlatformThemeLoader.GetAccentColors(PlatformThemeLoader.AccentColor);
            Application.Current.Resources["PlatformTheme-AccentColors-AccentColor"] = accent.Accent;
            Application.Current.Resources["PlatformTheme-AccentColors-BaseColor"] = accent.Base;
            Application.Current.Resources["PlatformTheme-AccentColors-HighlightColor"] = accent.Highlight;
            Application.Current.Resources["PlatformTheme-AccentColors-ActiveColor"] = accent.Active;
            Application.Current.Resources["PlatformTheme-AccentColors-DisableColor"] = accent.Disable;

            var text = PlatformThemeLoader.GetTextColor(accent);
            Application.Current.Resources["PlatformTheme-AccentTextColors-AccentColor"] = text.Accent;
            Application.Current.Resources["PlatformTheme-AccentTextColors-BaseColor"] = text.Base;
            Application.Current.Resources["PlatformTheme-AccentTextColors-HighlightColor"] = text.Highlight;
            Application.Current.Resources["PlatformTheme-AccentTextColors-ActiveColor"] = text.Active;
            Application.Current.Resources["PlatformTheme-AccentTextColors-DisableColor"] = text.Disable;

            void ApplyAccentBrush(string name)
            {
                var color = (Color)Application.Current.Resources[name + "Color"];
                var brush = FreezableUtility.GetSafeFreeze(new SolidColorBrush(color));
                Application.Current.Resources[name + "Brush"] = brush;
            }
            var names = new[] {
                "PlatformTheme-AccentColors-Accent",
                "PlatformTheme-AccentColors-Base",
                "PlatformTheme-AccentColors-Highlight",
                "PlatformTheme-AccentColors-Active",
                "PlatformTheme-AccentColors-Disable",

                "PlatformTheme-AccentTextColors-Accent",
                "PlatformTheme-AccentTextColors-Base",
                "PlatformTheme-AccentTextColors-Highlight",
                "PlatformTheme-AccentTextColors-Active",
                "PlatformTheme-AccentTextColors-Disable",
            };
            foreach(var name in names) {
                ApplyAccentBrush(name);
            }

            if(Logger.IsEnabled(LogLevel.Debug)) {
                Logger.LogDebug("アクセントカラー: #{0:x2}{1:x2}{2:x2}{3:x2}", accent.Accent.A, accent.Accent.R, accent.Accent.G, accent.Accent.B);
            }
        }

        (bool isEnabledTelemetry, string userId) GetTelemetry()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            SettingAppExecuteSettingData setting;
            using(var commander = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(commander, commander.Implementation);
                setting = dao.SelectSettingExecuteSetting();
            }

            if(!setting.IsEnabledTelemetry) {
                Logger.LogInformation("統計情報送信: 無効");
                return (false, string.Empty);
            }

            var userIdManager = ApplicationDiContainer.Build<UserIdManager>();
            if(!userIdManager.IsValidUserId(setting.UserId)) {
                Logger.LogWarning("ユーザーIDが不正: {0}", setting.UserId);
                return (false, string.Empty);
            }

            return (setting.IsEnabledTelemetry, setting.UserId);
        }

        void StartupUsageStatistics()
        {
            var userTracker = new UserTracker(LoggerFactory);
            ApplicationDiContainer.Register<IUserTracker, UserTracker>(userTracker);

            //var configuration = ApplicationDiContainer.Build<Configuration>();
            var setting = GetTelemetry();
            if(setting.isEnabledTelemetry) {
                //AppCenter.Start(
                //    configuration.Api.AppCenter,
                //    typeof(Crashes),
                //    typeof(Analytics)
                //);
                //AppCenter.SetUserId(setting.UserId);
            }
        }

        private void LoggingInformation()
        {
            var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
            infoCollector.Header = string.Empty;
            infoCollector.Indent = string.Empty;

            var s = infoCollector.GetLongInformation();
            Logger.LogInformation("[各種情報]" + Environment.NewLine + s);
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            StartupUsageStatistics();
            LoggingInformation();

            //var initializer = new ApplicationInitializer();
            //if(!initializer.Initialize(e.Args)) {
            //    return false;
            //}
            //ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(PlatformThemeLoader);

            //setting.UserId

            //ApplicationDiContainer.Get<IDispatcherWrapper>().Invoke(() => {
            SetStaticPlatformTheme();
            SetDynamicPlatformTheme();
            //});

            MakeMessageWindow();

            LoadPlugins();
            ApplyCurrentTheme(DefaultTheme.Informations.PluginIdentifiers);

            Logger = LoggerFactory.CreateLogger(GetType());
            Logger.LogDebug("初期化完了");

            if(IsFirstStartup) {
                // 初期登録の画面を表示
                ShowStartupView(true);
            }

            var tuner = ApplicationDiContainer.Build<DatabaseTuner>();
            tuner.Tune();

            return true;
        }

        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, ApplicationDiContainer.Build<IUserTracker>(), LoggerFactory);
            return viewModel;
        }

        IReadOnlyList<LauncherGroupElement> CreateLauncherGroupElements()
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<Guid> launcherGroupIds;
            using(var commander = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<LauncherGroupsEntityDao>(commander, commander.Implementation);
                launcherGroupIds = dao.SelectAllLauncherGroupIds().ToList();
            }

            var result = new List<LauncherGroupElement>(launcherGroupIds.Count);
            foreach(var launcherGroupId in launcherGroupIds) {
                var element = CreateLauncherGroupElement(launcherGroupId);
                result.Add(element);
            }

            return result;
        }

        IReadOnlyList<LauncherToolbarElement> CreateLauncherToolbarElements(ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
        {
            var screens = Screen.AllScreens;
            var result = new List<LauncherToolbarElement>(screens.Length);

            foreach(var screen in screens.OrderByDescending(i => i.Primary)) {
                var element = CreateLauncherToolbarElement(screen, launcherGroups);
                result.Add(element);
            }

            return result;
        }

        IReadOnlyList<NoteElement> CreateNoteElements()
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<Guid> noteIds;
            using(var commander = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<NotesEntityDao>(commander, commander.Implementation);
                noteIds = dao.SelectAllNoteIds().ToList();
            }

            var result = new List<NoteElement>(noteIds.Count);
            foreach(var noteId in noteIds) {
                var element = CreateNoteElement(noteId, default(IScreen), NoteStartupPosition.Setting);
                result.Add(element);
            }

            return result;
        }

        public ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> GetLauncherNotifyCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>(LauncherToolbarElements) {
                ToViewModel = m => ApplicationDiContainer.Build<LauncherToolbarNotifyAreaViewModel>(m)
            };
            return collection;
        }

        public ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> GetNoteCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel>(NoteElements) {
                ToViewModel = m => ApplicationDiContainer.Build<NoteNotifyAreaViewModel>(m)
            };
            return collection;
        }

        public NoteElement CreateNote(IScreen dockScreen, NoteStartupPosition noteStartupPosition)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.LogInformation("new note id: {0}, {1}", noteId, ObjectDumper.GetDumpString(dockScreen));
            var noteElement = CreateNoteElement(noteId, dockScreen, noteStartupPosition);

            NoteElements.Add(noteElement);

            return noteElement;
        }

        public void CompactAllNotes()
        {
            var noteItems = WindowManager.GetWindowItems(WindowKind.Note)
                .Select(i => i.ViewModel)
                .Cast<NoteViewModel>()
                .Where(i => !i.IsLocked)
                .Where(i => i.IsVisible)
                .Where(i => !i.IsCompact)
                .ToList()
            ;
            foreach(var note in noteItems) {
                note.ToggleCompactCommand.ExecuteIfCanExecute(null);
            }
        }

        public void MoveZOrderAllNotes(bool isTop)
        {
            if(isTop) {
                var noteElements = NoteElements
                    .Where(i => i.IsVisible)
                    .Where(i => !i.IsTopmost)
                    .ToList()
                ;
                var dispatcherWrapper = ApplicationDiContainer.Get<IDispatcherWrapper>();
                dispatcherWrapper.Begin(() => {
                    foreach(var noteElement in noteElements) {
                        noteElement.SetTopmost(true);
                    }
                }, DispatcherPriority.SystemIdle).Task.ContinueWith(t => {
                    foreach(var noteElement in noteElements) {
                        noteElement.SetTopmost(false);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            } else {
                var noteItems = WindowManager.GetWindowItems(WindowKind.Note)
                    .Where(i => !i.Window.Topmost)
                    .Where(i => i.Window.IsVisible)
                    .ToList()
                ;
                foreach(var noteItem in noteItems) {
                    var hWnd = HandleUtility.GetWindowHandle(noteItem.Window);
                    WindowsUtility.MoveZoderBttom(hWnd);
                }
            }
        }

        void ExecuteElements()
        {
            var currentActiveWindowHandle = NativeMethods.GetActiveWindow();
            //if(currentActiveWindowHandle == IntPtr.Zero) {
            //    currentActiveWindowHandle = NativeMethods.GetForegroundWindow();
            //}

            // グループ構築
            var launcherGroups = CreateLauncherGroupElements();
            LauncherGroupElements.AddRange(launcherGroups);

            // ツールバーの生成
            var launcherToolbars = CreateLauncherToolbarElements(new ReadOnlyObservableCollection<LauncherGroupElement>(LauncherGroupElements));
            LauncherToolbarElements.AddRange(launcherToolbars);

            // ノートの生成
            var notes = CreateNoteElements();
            NoteElements.AddRange(notes);

            var viewShowStaters = Enumerable.Empty<IViewShowStarter>()
                .Concat(notes)
                .Concat(launcherToolbars)
                .Where(i => i.CanStartShowView)
                .ToList()
            ;
            foreach(var viewShowStater in viewShowStaters) {
                viewShowStater.StartView();
            }

#if DEBUG
            if(IsDevDebug) {
                Logger.LogWarning($"{nameof(IsDevDebug)}が有効");
                return;
            }
#endif
            ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                // ノート生成で最後のノートがアクティブになる対応。設定でも発生するけど起動時に何とかしていって思い
                if(currentActiveWindowHandle != IntPtr.Zero && currentActiveWindowHandle != MessageWindowHandleSource?.Handle) {
                    WindowsUtility.ShowActive(currentActiveWindowHandle);
                }
                MoveZOrderAllNotes(false);
            }, DispatcherPriority.SystemIdle);
        }

        public void Execute()
        {
            Logger.LogInformation("がんばる！");
#if DEBUG
            DebugExecuteBefore();
#endif
            InitializeSystem();
            InitializeHook();

            StartPlatform();

            ExecuteElements();
#if DEBUG
            DebugExecuteAfter();
#endif
        }

        void CloseViewsCore(WindowKind windowKind)
        {
            var windowItems = WindowManager.GetWindowItems(windowKind).ToList();
            foreach(var windowItem in windowItems) {
                if(windowItem.IsOpened) {
                    if(!windowItem.IsClosed) {
                        if(windowItem.Window.IsVisible) {
                            Logger.LogTrace("閉じることのできるウィンドウ: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                            windowItem.Window.Close();
                        } else {
                            Logger.LogTrace("非表示ウィンドウ: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                        }
                    } else {
                        Logger.LogTrace("既に閉じられたウィンドウのためクローズしない: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                    }
                } else {
                    Logger.LogTrace("まだ開かれていないウィンドウのためクローズしない: {0}, {1}", windowItem.WindowKind, windowItem.ViewModel);
                }
            }
        }

        void CloseLauncherToolbarViews() => CloseViewsCore(WindowKind.LauncherToolbar);

        void CloseNoteViews() => CloseViewsCore(WindowKind.Note);

        void CloseLauncherCustomizeViews() => CloseViewsCore(WindowKind.LauncherCustomize);

        void CloseExtendsExecuteViews() => CloseViewsCore(WindowKind.ExtendsExecute);
        void CloseStandardInputOutputViews() => CloseViewsCore(WindowKind.StandardInputOutput);

        void CloseViews()
        {
            CloseStandardInputOutputViews();
            CloseLauncherCustomizeViews();
            CloseExtendsExecuteViews();
            CloseLauncherToolbarViews();
            CloseNoteViews();
        }

        void DisposeElementsCore<TElement>(ICollection<TElement> elements)
            where TElement : ElementBase
        {
            foreach(var element in elements) {
                element.Dispose();
            }
            elements.Clear();
        }

        void DisposeLauncherToolbarElements() => DisposeElementsCore(LauncherToolbarElements);
        void DisposeLauncherGroupElements() => DisposeElementsCore(LauncherGroupElements);
        void DisposeNoteElements() => DisposeElementsCore(NoteElements);

        void DisposeElements()
        {
            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private void DisposeWebView()
        {
            CefSharp.Cef.Shutdown();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreUpdate">アップデートを無視するか。</param>
        public void Exit(bool ignoreUpdate)
        {
            Logger.LogInformation("おわる！");

            BackupSettingsDefault(ApplicationDiContainer);

            if(!ignoreUpdate && ApplicationUpdateInfo.IsReady) {
                Debug.Assert(ApplicationUpdateInfo.Path != null);

                Logger.LogInformation("アップデート処理起動");

                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = ApplicationUpdateInfo.Path.Path;
                process.StartInfo.Arguments = ApplicationUpdateInfo.Path.Option;
                process.StartInfo.WorkingDirectory = ApplicationUpdateInfo.Path.WorkDirectoryPath;

                Logger.LogInformation("path: {0}", process.StartInfo.FileName);
                Logger.LogInformation("args: {0}", process.StartInfo.Arguments);

                try {
                    process.Start();
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }


            StopHook();
            DisposeHook();

            UninitializeSystem();

            CloseViews();
            DisposeElements();
            DisposeWebView();

            Dispose();

            Logger.LogInformation("ばいばい");

            NLog.LogManager.Shutdown();
            Application.Current.Shutdown();
        }

        public void Reboot()
        {
            Logger.LogInformation("再起動開始");

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();

            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
            var executeFiles = environmentExecuteFile.GetPathExecuteFiles();
            var pwsh = environmentExecuteFile.Get("pwsh", executeFiles);
            var powershell = environmentExecuteFile.Get("powershell", executeFiles);

            if(pwsh == null && powershell == null) {
                Logger.LogError("[pwsh] と [powershell] が見つかんないのでもぅﾏﾁﾞ無理");
                return;
            }

            var ps = pwsh?.File.FullName ?? powershell!.File.FullName;

            var psCommands = new List<string>() {
                "-NoProfile",
                "-ExecutionPolicy", "Unrestricted",
                "-File", CommandLine.Escape(environmentParameters.EtcRebootScriptFile.FullName),
                "-LogPath", CommandLine.Escape(environmentParameters.TemporaryRebootLogFile.FullName),
                "-ProcessId", Process.GetCurrentProcess().Id.ToString(),
                "-WaitSeconds", TimeSpan.FromSeconds(10).TotalMilliseconds.ToString(),
                "-ExecuteCommand", CommandLine.Escape(environmentParameters.RootApplication.FullName),
            };
            var currentCommands = Environment.GetCommandLineArgs()
                .Skip(1)
                .Select(i => CommandLine.Escape(i))
                .ToList()
            ;
            if(0 < currentCommands.Count) {
                psCommands.Add("-ExecuteArgument");
                psCommands.Add(CommandLine.Escape(string.Join(" ", currentCommands)));
            }

            var psCommand = string.Join(" ", psCommands);

            try {
                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = ps;
                process.StartInfo.Arguments = psCommand;
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.Start();
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            Exit(true);
        }

        public void ShowCommandView()
        {
            if(!CommandElement.IsInitialized) {
                CommandElement.Initialize();
            }

            CommandElement.StartView();
        }

        public void ShowFeedbackView()
        {
            var items = WindowManager.GetWindowItems(WindowKind.Feedback).ToList();
            if(items.Count != 0) {
                foreach(var item in items) {
                    WindowManager.Flash(item);
                }
                return;
            }

            var feedbackElement = ApplicationDiContainer.Build<FeedbackElement>();
            feedbackElement.Initialize();
            var windowItem = OrderManager.CreateFeedbackWindow(feedbackElement);
            WindowManager.Register(windowItem);
            windowItem.Window.Show();
        }

        void BackupSettings(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string backupFileBaseName, int enabledCount, string userBackupDirectoryPath)
        {
            try {
                // アプリケーション側バックアップ
                var settingBackupper = new SettingBackupper(LoggerFactory);
                settingBackupper.BackupUserSetting(sourceDirectory, targetDirectory, backupFileBaseName, enabledCount);

                // ユーザー設定側バックアップ
                var expandeduserBackupDirectoryPath = Environment.ExpandEnvironmentVariables(userBackupDirectoryPath ?? string.Empty);
                if(!string.IsNullOrWhiteSpace(expandeduserBackupDirectoryPath)) {
                    var dir = new DirectoryInfo(expandeduserBackupDirectoryPath);
                    settingBackupper.BackupUserSettingToCustomDirectory(sourceDirectory, dir);
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "バックアップ失敗");
            }
        }

        void BackupSettingsDefault(IDiContainer diContainer)
        {
            var environmentParameters = diContainer.Get<EnvironmentParameters>();

            // バックアップ処理開始
            string userBackupDirectoryPath;
            using(var commander = diContainer.Get<IMainDatabaseBarrier>().WaitRead()) {
                var appGeneralSettingEntityDao = diContainer.Build<AppGeneralSettingEntityDao>(commander, commander.Implementation);
                userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
            }
            BackupSettings(
                environmentParameters.UserSettingDirectory,
                environmentParameters.UserBackupDirectory,
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                environmentParameters.Configuration.Backup.SettingCount,
                userBackupDirectoryPath
            );
        }

        private void ClearScreenViewElements()
        {
            CloseLauncherToolbarViews();
            CloseNoteViews();

            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private void ResetScreenViewElements()
        {
            ClearScreenViewElements();

            ExecuteElements();
        }

        private void DelayResetScreenViewElements()
        {
            void DelayExecuteElements()
            {
                LazyScreenElementReset.DelayAction(() => {
                    ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(ResetScreenViewElements, DispatcherPriority.SystemIdle);
                    ResetWaiting = false;
                });
            }

            if(!ResetWaiting) {
                ResetWaiting = true;
                ClearScreenViewElements();
                DelayExecuteElements();
            } else {
                DelayExecuteElements();
            }
        }

        public async Task DelayCheckUpdateAsync()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            UpdateKind updateKind;
            using(var commander = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppUpdateSettingEntityDao>(commander, commander.Implementation);
                updateKind = dao.SelectSettingUpdateSetting().UpdateKind;
            }

            if(updateKind == UpdateKind.None) {
                return;
            }

            var updateWait = ApplicationDiContainer.Build<CustomConfiguration>().General.UpdateWait;
            await Task.Delay(updateWait).ConfigureAwait(false);
            var updateCheckKind = updateKind switch
            {
                UpdateKind.Notify => UpdateCheckKind.CheckOnly,
                _ => UpdateCheckKind.Update,
            };
            await ExecuteUpdateAsync(updateCheckKind).ConfigureAwait(false);
        }

        public async Task ExecuteUpdateAsync(UpdateCheckKind updateCheckKind)
        {
            if(ApplicationUpdateInfo.State == UpdateState.None || ApplicationUpdateInfo.State == UpdateState.Error) {
                if(ApplicationUpdateInfo.State == UpdateState.Error) {
                    Logger.LogInformation("エラーありのため再実施");
                }
            } else {
                if(ApplicationUpdateInfo.IsReady) {
                    Logger.LogInformation("アップデート準備完了");
                } else {
                    Logger.LogInformation("アップデート排他制御中");
                }

                ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem!, false);

                return;
            }

            var updateChecker = ApplicationDiContainer.Build<UpdateChecker>();

            ApplicationUpdateInfo.State = UpdateState.Checking;
            {
                var appVersion = await updateChecker.CheckApplicationUpdateAsync().ConfigureAwait(false);
                if(appVersion == null) {
                    Logger.LogInformation("アップデートなし");
                    ApplicationUpdateInfo.State = UpdateState.None;
                    return;
                }
                ApplicationUpdateInfo.UpdateItem = appVersion;
            }

            Logger.LogInformation("アップデートあり: {0}", ApplicationUpdateInfo.UpdateItem.Version);

            // CheckApplicationUpdateAsync で弾いてる
            //if(BuildStatus.Version < ApplicationUpdateInfo.UpdateItem.MinimumVersion) {
            //    Logger.LogWarning("最低バージョン未満であるためバージョンアップ不可: 現在 = {0}, 要求 = {1}", BuildStatus.Version, appVersion.MinimumVersion);
            //    UpdateInfo.State = UpdateState.None;
            //    return;
            //}

            Logger.LogInformation("アップデート可能");

            var updateDownloader = ApplicationDiContainer.Build<UpdateDownloader>();

            if(updateCheckKind != UpdateCheckKind.ForceUpdate) {
                try {
                    ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem, updateCheckKind == UpdateCheckKind.CheckOnly);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    ApplicationUpdateInfo.SetError(ex.Message);
                    return;
                }
            }
            if(updateCheckKind == UpdateCheckKind.CheckOnly) {
                ApplicationUpdateInfo.State = UpdateState.None;
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var downloadFileName = versionConverter.ConvertFileName(BuildStatus.Name, ApplicationUpdateInfo.UpdateItem.Version, ApplicationUpdateInfo.UpdateItem.Platform, ApplicationUpdateInfo.UpdateItem.ArchiveKind);
            var downloadFilePath = Path.Combine(environmentParameters.MachineUpdateArchiveDirectory.FullName, downloadFileName);
            var downloadFile = new FileInfo(downloadFilePath);
            try {
                var skipDownload = false;
                downloadFile.Refresh();
                if(downloadFile.Exists) {
                    ApplicationUpdateInfo.State = UpdateState.Checksumming;
                    skipDownload = await updateDownloader.ChecksumAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                }
                if(skipDownload) {
                    Logger.LogInformation("チェックサムの結果ダウンロード不要");
                    IProgress<double> progress = ApplicationUpdateInfo.DownloadProgress;
                    progress.Report(1);
                } else {
                    downloadFile.Delete(); // ゴミは消しとく
                    ApplicationUpdateInfo.State = UpdateState.Downloading;
                    await updateDownloader.DownloadApplicationArchiveAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.DownloadProgress, ApplicationUpdateInfo.CurrentLogProgress)).ConfigureAwait(false);

                    // ここで更新しないとチェックサムでファイ無し判定を食らう
                    downloadFile.Refresh();

                    ApplicationUpdateInfo.State = UpdateState.Checksumming;
                    var checksumOk = await updateDownloader.ChecksumAsync(ApplicationUpdateInfo.UpdateItem, downloadFile, new UserNotifyProgress(ApplicationUpdateInfo.ChecksumProgress, ApplicationUpdateInfo.CurrentLogProgress));
                    if(!checksumOk) {
                        Logger.LogError("チェックサム異常あり");
                        ApplicationUpdateInfo.SetError(Properties.Resources.String_Download_ChecksumError);
                        return;
                    }
                }
            } catch(Exception ex) {
                ApplicationUpdateInfo.SetError(ex.Message);
                return;
            }

            Logger.LogInformation("アップデートファイル展開");
            ApplicationUpdateInfo.State = UpdateState.Extracting;
            try {
                var directoryCleaner = new DirectoryCleaner(environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.Configuration.File.DirectoryRemoveWaitCount, environmentParameters.Configuration.File.DirectoryRemoveWaitTime, LoggerFactory);
                directoryCleaner.Clear(false);

                var archiveExtractor = ApplicationDiContainer.Build<ArchiveExtractor>();
                archiveExtractor.Extract(downloadFile, environmentParameters.TemporaryApplicationExtractDirectory, ApplicationUpdateInfo.UpdateItem.ArchiveKind, new UserNotifyProgress(ApplicationUpdateInfo.ExtractProgress, ApplicationUpdateInfo.CurrentLogProgress));

                var scriptFactory = ApplicationDiContainer.Build<ApplicationUpdateScriptFactory>();
                var exeutePathParameter = scriptFactory.CreateUpdateExecutePathParameter(environmentParameters.EtcUpdateScriptFile, environmentParameters.TemporaryDirectory, environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.RootDirectory);
                ApplicationUpdateInfo.Path = exeutePathParameter;
                ApplicationUpdateInfo.State = UpdateState.Ready;

                // アップデートアーカイブのローテート
                var fileRotation = new FileRotation();
                fileRotation.ExecuteExtensions(
                    environmentParameters.MachineUpdateArchiveDirectory,
                    new[] { "zip", "7z" },
                    environmentParameters.Configuration.Backup.ArchiveCount,
                    ex => {
                        Logger.LogWarning(ex, ex.Message);
                        return true;
                    }
                );

                // リリースノート再表示
                ShowUpdateReleaseNote(ApplicationUpdateInfo.UpdateItem, false);

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                ApplicationUpdateInfo.SetError(ex.Message);
            }

        }

        internal FileInfo OutputRawCrashReport(Exception exception)
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HHmmss_fff'Z'");
            var fileName = versionConverter.ConvertFileName(timestamp, BuildStatus.Version, BuildStatus.Revision, "dmp");
            var filePath = Path.Combine(environmentParameters.TemporaryCrashReportDirectory.FullName, fileName);

            static Dictionary<string, string?> CreateInfoMap(IEnumerable<PlatformInformationItem> items) => items.ToDictionary(k => k.Key, v => Convert.ToString(v.Value));
            void ExceptionWrapper(Action action)
            {
                try {
                    action();
                } catch(Exception ex) {
                    // 運に任せる
                    Logger.LogError(ex, ex.Message);
                }
            }

            var rawData = new CrashReportRawData() {
                Version = BuildStatus.Version,
                Revision = BuildStatus.Revision,
                Exception = exception.ToString(),
                Timestamp = DateTime.UtcNow,
            };

            ExceptionWrapper(() => {
                rawData.UserId = ApplicationDiContainer.Get<IMainDatabaseBarrier>().ReadData(c => {
                    var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(c, c.Implementation);
                    var userIdManager = new UserIdManager(LoggerFactory);
                    return userIdManager.SafeGetOrCreateUserId(appExecuteSettingEntityDao);
                });
            });

            string TrimFunc(string s) => s.Substring(3);

            var info = new ApplicationInformationCollector(environmentParameters);
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetApplication))] = CreateInfoMap(info.GetApplication()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironmentParameter))] = CreateInfoMap(info.GetEnvironmentParameter()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetCPU))] = CreateInfoMap(info.GetCPU()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetOS))] = CreateInfoMap(info.GetOS()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetRuntimeInformation))] = CreateInfoMap(info.GetRuntimeInformation()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironment))] = CreateInfoMap(info.GetEnvironment()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetEnvironmentVariables))] = CreateInfoMap(info.GetEnvironmentVariables()));
            ExceptionWrapper(() => rawData.Informations[TrimFunc(nameof(info.GetScreen))] = CreateInfoMap(info.GetScreen()));

            // この子はもうこの時点のログで確定
            rawData.LogItems = Logging.GetLogItems().Select(i => LogItem.Create(i)).ToList();

            var file = new FileInfo(filePath);
            using(var stream = file.Create()) {
                var serializer = new CrashReportSerializer();
                serializer.Save(rawData, stream);
            }
#if DEBUG
            using(var stream = file.Open(FileMode.Open)) {
                var serializer = new CrashReportSerializer();
                var data = serializer.Load<CrashReportRawData>(new KeepStream(stream));
                var diffStream = new MemoryStream();
                serializer.Save(data, new KeepStream(diffStream));
                stream.Position = 0;
                diffStream.Position = 0;
                Debug.Assert(stream.Length == diffStream.Length);
                var s = new byte[stream.Length];
                var d = new byte[diffStream.Length];
                stream.Read(s);
                diffStream.Read(d);
                Debug.Assert(s.SequenceEqual(d));
            }
#endif
            return file;
        }

        internal void ExecuteCrashReport(FileInfo rawReport)
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var saveReportFilePath = Path.Combine(environmentParameters.MachineCrashReportDirectory.FullName, Path.ChangeExtension(rawReport.Name, "json"));

            var currentCommands = Environment.GetCommandLineArgs()
                .Skip(1)
                .Select(i => CommandLine.Escape(i))
                .ToList()
            ;

            var autoSend = ApplicationDiContainer.Get<IMainDatabaseBarrier>().ReadData(c => {
                var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(c, c.Implementation);
                var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
                return setting.IsEnabledTelemetry;
            });

            var args = new List<string> {
                "--run-mode", "crash-report",
                "--language", System.Globalization.CultureInfo.CurrentCulture.Name,
                "--post-uri", CommandLine.Escape(environmentParameters.Configuration.Api.CrashReportUri.OriginalString),
                "--src-uri", CommandLine.Escape(environmentParameters.Configuration.Api.CrashReportSourceUri.OriginalString),
                "--report-raw-file", CommandLine.Escape(rawReport.FullName),
                "--report-save-file", CommandLine.Escape(saveReportFilePath),
                "--execute-command", CommandLine.Escape(environmentParameters.RootApplication.FullName),
                "--execute-argument", CommandLine.Escape(string.Join(" ", currentCommands)),
            };
            if(autoSend) {
                args.Add("--auto-send");
            }
            args.AddRange(currentCommands);

            var arg = string.Join(' ', args);

            var systemExecutor = new SystemExecutor();
            var commandPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe");
            Logger.LogInformation("path: {0}", commandPath);
            Logger.LogInformation("args: {0}", arg);
            systemExecutor.ExecuteFile(commandPath, arg);
        }

        internal void StartupEnd()
        {
            StartHook();
            RunAddons();
            DelayCheckUpdateAsync().ConfigureAwait(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called")]
        private void GarbageCollection(bool full)
        {
            var old = GC.GetTotalMemory(false);
            if(full) {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            } else {
                GC.Collect(0);
                GC.Collect(1);
            }
            var now = GC.GetTotalMemory(false);
            var sizeConverter = ApplicationDiContainer.Build<Core.Models.SizeConverter>();
            Logger.LogInformation(
                "GC(FULL:{0}): {1}({2}) -> {3}({4}), 差分: {5}({6})",
                full,
                sizeConverter.ConvertHumanLikeByte(old), old,
                sizeConverter.ConvertHumanLikeByte(now), now,
                sizeConverter.ConvertHumanLikeByte(old - now), old - now
            );
        }

        #endregion

        #region IOrderManager

        public void AddRedoItem(RedoExecutor redoExecutor) => OrderManager.AddRedoItem(redoExecutor);

        public void StartUpdate(UpdateTarget target, UpdateProcess process)
        {
            Debug.Assert(target == UpdateTarget.Application);

            switch(process) {
                case UpdateProcess.Download:
                    ExecuteUpdateAsync(UpdateCheckKind.Update).ConfigureAwait(false);
                    break;

                case UpdateProcess.Update:
                    Exit(false);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId)
        {
            return OrderManager.CreateLauncherGroupElement(launcherGroupId);
        }
        public LauncherToolbarElement CreateLauncherToolbarElement(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
        {
            return OrderManager.CreateLauncherToolbarElement(dockScreen, launcherGroups);
        }

        public LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId)
        {
            return OrderManager.GetOrCreateLauncherItemElement(launcherItemId);
        }
        public void RefreshLauncherItemElement(Guid launcherItemId) => OrderManager.RefreshLauncherItemElement(launcherItemId);

        public LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, IScreen screen, LauncherIconElement iconElement)
        {
            return OrderManager.CreateCustomizeLauncherItemContainerElement(launcherItemId, screen, iconElement);
        }

        public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen)
        {
            return OrderManager.CreateExtendsExecuteElement(captionName, launcherFileData, launcherEnvironmentVariables, screen);
        }

        public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, IScreen screen)
        {
            return OrderManager.CreateLauncherExtendsExecuteElement(launcherItemId, screen);
        }


        public NoteElement CreateNoteElement(Guid noteId, IScreen? screen, NoteStartupPosition startupPosition)
        {
            return OrderManager.CreateNoteElement(noteId, screen, startupPosition);
        }
        public bool RemoveNoteElement(Guid noteId)
        {
            var targetElement = NoteElements.FirstOrDefault(i => i.NoteId == noteId);
            if(targetElement == null) {
                Logger.LogWarning("ノート削除: 対象不明 {0}", noteId);
                return false;
            }

            var entitiesRemover = ApplicationDiContainer.Build<EntitiesRemover>();
            entitiesRemover.Items.Add(new NoteRemover(noteId, LoggerFactory));

            try {
                var reuslt = entitiesRemover.Execute();
                if(reuslt.Sum(i => i.Items.Count) == 0) {
                    Logger.LogWarning("ノート削除に失敗: 対象データ不明: {0}", noteId);
                    return false;
                }
                NoteElements.Remove(targetElement);
                targetElement.Dispose();
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, "ノート削除に失敗: {0} {1}", ex.Message, noteId);
            }

            return false;
        }

        public NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind)
        {
            return OrderManager.CreateNoteContentElement(noteId, contentKind);
        }

        public SavingFontElement CreateFontElement(DefaultFontKind defaultFontKind, Guid fontId, ParentUpdater parentUpdater)
        {
            return OrderManager.CreateFontElement(defaultFontKind, fontId, parentUpdater);
        }

        /// <inheritdoc cref="IOrderManager.CreateStandardInputOutputElement(string, Process, IScreen)"/>
        public StandardInputOutputElement CreateStandardInputOutputElement(string caption, Process process, IScreen screen)
        {
            var element = OrderManager.CreateStandardInputOutputElement(caption, process, screen);
            StandardInputOutputs.Add(element);
            return element;
        }

        public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
        {
            var windowItem = OrderManager.CreateLauncherToolbarWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element)
        {
            var windowItem = OrderManager.CreateCustomizeLauncherItemWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element)
        {
            var windowItem = OrderManager.CreateExtendsExecuteWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateNoteWindow(NoteElement element)
        {
            var windowItem = OrderManager.CreateNoteWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateCommandWindow(CommandElement element)
        {
            var windowItem = OrderManager.CreateCommandWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }


        public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
        {
            var windowItem = OrderManager.CreateStandardInputOutputWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateNotifyLogWindow(NotifyLogElement element)
        {
            var windowItem = OrderManager.CreateNotifyLogWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }


        public WindowItem CreateSettingWindow(SettingContainerElement element)
        {
            var windowItem = OrderManager.CreateSettingWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }




        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    HeartBeatSender?.Dispose();

                    ApplicationMutex.ReleaseMutex();
                    ApplicationMutex.Dispose();

                    LazyScreenElementReset.Dispose();

                    CloseViews();
                    DisposeElements();
                    DisposeWebView();

                    //MessageWindowDispatcherWapper?.Begin(() => {
                    //    MessageWindowHandleSource?.Dispose();
                    //    Dispatcher.CurrentDispatcher.InvokeShutdown();
                    //});
                    MessageWindowHandleSource?.Dispose();

                    NotifyManagerImpl.Dispose();
                    OrderManager.Dispose();

                    WindowManager.Dispose();
                    StatusManagerImpl.Dispose();
                    ClipboardManager.Dispose();
                    UserAgentManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void PlatformThemeLoader_Changed(object? sender, EventArgs e)
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                SetDynamicPlatformTheme();
            }, DispatcherPriority.ApplicationIdle);
        }



    }
}
