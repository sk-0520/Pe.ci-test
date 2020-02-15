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
using ContentTypeTextNet.Pe.Main.Models.UsageStatistics;
using System.IO.Compression;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public partial class ApplicationManager : DisposerBase, IOrderManager
    {
        public ApplicationManager(ApplicationInitializer initializer)
        {
            LoggerFactory = initializer.LoggerFactory ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.LoggerFactory));
            Logger = LoggerFactory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;
            PlatformThemeLoader = new PlatformThemeLoader(LoggerFactory);
            PlatformThemeLoader.Changed += PlatformThemeLoader_Changed;

            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));
            WindowManager = initializer.WindowManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.WindowManager));
            OrderManager = ApplicationDiContainer.Make<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManager = initializer.NotifyManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.NotifyManager));
            StatusManager = initializer.StatusManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.StatusManager));
            ClipboardManager = initializer.ClipboardManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.ClipboardManager));
            UserAgentManager = initializer.UserAgentManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.UserAgentManager));

            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManager);
            ApplicationDiContainer.Register<IStatusManager, StatusManager>(StatusManager);
            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
            ApplicationDiContainer.Register<IUserAgentManager, UserAgentManager>(UserAgentManager);

            KeyboradHooker = new KeyboradHooker(LoggerFactory);
            MouseHooker = new MouseHooker(LoggerFactory);
            KeyActionChecker = new KeyActionChecker(LoggerFactory);
            KeyActionAssistant = new KeyActionAssistant(LoggerFactory);

            CommandElement = ApplicationDiContainer.Build<CommandElement>();
            UpdateInfo = ApplicationDiContainer.Build<UpdateInfo>();
        }

        #region property

        ILoggerFactory LoggerFactory { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        bool IsFirstStartup { get; }
        ILogger Logger { get; set; }
        PlatformThemeLoader PlatformThemeLoader { get; }

        WindowManager WindowManager { get; set; }
        OrderManagerImpl OrderManager { get; set; }
        NotifyManager NotifyManager { get; set; }
        StatusManager StatusManager { get; set; }
        ClipboardManager ClipboardManager { get; set; }
        UserAgentManager UserAgentManager { get; set; }

        ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();
        ObservableCollection<StandardInputOutputElement> StandardInputOutputs { get; } = new ObservableCollection<StandardInputOutputElement>();
        CommandElement CommandElement { get; }
        HwndSource? MessageWindowHandleSource { get; set; }
        //IDispatcherWapper? MessageWindowDispatcherWapper { get; set; }

        KeyboradHooker KeyboradHooker { get; }
        MouseHooker MouseHooker { get; }
        KeyActionChecker KeyActionChecker { get; }
        KeyActionAssistant KeyActionAssistant { get; }

        PluginContainer? PluginContainer { get; set; }

        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public UpdateInfo UpdateInfo { get; }

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

            var changing = StatusManager.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

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
                new ApplicationDatabaseFactory(settings.Main),
                new ApplicationDatabaseFactory(settings.File),
                new ApplicationDatabaseFactory()
            );
            var lazyWriterWaitTimePack = new LazyWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));

            container
                .Register<IDiContainer, DiContainer>((DiContainer)container) // むりやりぃ
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
                string userBackupDirectoryPath;
                using(var commander = container.Get<IMainDatabaseBarrier>().WaitRead()) {
                    var appGeneralSettingEntityDao = container.Build<AppGeneralSettingEntityDao>(commander, commander.Implementation);
                    userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
                }
                try {
                    BackupSettings(
                        environmentParameters.UserSettingDirectory,
                        environmentParameters.UserBackupDirectory,
                        DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                        environmentParameters.Configuration.Backup.SettingCount,
                        userBackupDirectoryPath
                    );
                } catch(Exception ex) {
                    Logger.LogError(ex, "バックアップ処理失敗: {0}", ex.Message);
                }

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
                var cultureServiceChanger = ApplicationDiContainer.Build<CultureServiceChanger>(CultureService.Current);
                cultureServiceChanger.ChangeCulture();

                Logger.LogInformation("設定適用のため各要素生成");
                RebuildHook();
                ExecuteElements();
                CommandElement.Refresh();
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

        public void ShowStartupView()
        {
            var changing = StatusManager.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModels.Startup.StartupViewModel, Views.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                var view = diContainer.Make<Views.Startup.StartupWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.Startup, view));

                view.ShowDialog();
            }

            changing.SuccessValue?.Dispose();
        }

        public void ShowAboutView()
        {
            var changing = StatusManager.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.About.AboutElement, ViewModels.About.AboutViewModel, Views.About.AboutWindow>()
                ;
                var model = diContainer.New<Element.About.AboutElement>();
                model.Initialize();

                var view = diContainer.Make<Views.About.AboutWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.About, view));

                view.ShowDialog();
            }

            changing.SuccessValue?.Dispose();
        }

        private void ShowUpdateReleaseNote(UpdateItemData updateItem)
        {
            var windowItem = WindowManager.GetWindowItems(WindowKind.Release);
            if(windowItem.Any()) {
                // 再表示
                ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                    windowItem.First().Window.Activate();
                }, DispatcherPriority.ApplicationIdle);
                return;
            }

            ApplicationDiContainer.Build<IDispatcherWrapper>().Begin(() => {
                var element = ApplicationDiContainer.Build<Element.ReleaseNote.ReleaseNoteElement>(UpdateInfo, updateItem);
                var view = ApplicationDiContainer.Build<Views.ReleaseNote.ReleaseNoteWindow>();
                view.DataContext = ApplicationDiContainer.Build<ViewModels.ReleaseNote.ReleaseNoteViewModel>(element);
                WindowManager.Register(new WindowItem(WindowKind.Release, view));
                view.Show();
            }, DispatcherPriority.ApplicationIdle);
        }

        private void RegisterPlugins()
        {
            Debug.Assert(ApplicationDiContainer != null);
            var addonContainer = ApplicationDiContainer.Build<AddonContainer>();
            var themeContainer = ApplicationDiContainer.Build<ThemeContainer>();
            PluginContainer = ApplicationDiContainer.Build<PluginContainer>(addonContainer, themeContainer);

            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
            foreach(var plugin in PluginContainer.GetPlugins()) {
                plugin.Initialize(pluginContextFactory.CreateInitializeContext(plugin.PluginId));
                PluginContainer.AddPlugin(plugin);
            }
            PluginContainer.Theme.SetCurrentTheme(DefaultTheme.Id, pluginContextFactory);

            ApplicationDiContainer.Register<IGeneralTheme, IGeneralTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetGeneralTheme());
            ApplicationDiContainer.Register<ILauncherToolbarTheme, ILauncherToolbarTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherToolbarTheme());
            ApplicationDiContainer.Register<ILauncherGroupTheme, ILauncherGroupTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherGroupTheme());
            ApplicationDiContainer.Register<INoteTheme, INoteTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNoteTheme());
            ApplicationDiContainer.Register<ICommandTheme, ICommandTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetCommandTheme());

        }

        void SetStaticPlatformTheme()
        {
            var themes = new[] { PlatformThemeKind.Dark, PlatformThemeKind.Light };
            foreach(var theme in themes) {
                var themeKey = theme.ToString();
                var colors = PlatformThemeLoader.GetApplicationThemeColors(PlatformThemeKind.Dark);
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BackgroundColor"] = colors.Background;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ForegroundColor"] = colors.Foreground;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-ControlColor"] = colors.Control;
                Application.Current.Resources["PlatformTheme-" + themeKey + "ThemeColors-BorderColor"] = colors.Border;
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
        }

        (bool sendUsageStatistics, string userId) GetUsageStatistics()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            SettingAppExecuteSettingData setting;
            using(var commander = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(commander, commander.Implementation);
                setting = dao.SelectSettingExecuteSetting();
            }

            if(!setting.SendUsageStatistics) {
                Logger.LogInformation("統計情報送信: 無効");
                return (false, string.Empty);
            }

            var userIdManager = ApplicationDiContainer.Build<UserIdManager>();
            if(!userIdManager.IsValidUserId(setting.UserId)) {
                Logger.LogWarning("ユーザーIDが不正: {0}", setting.UserId);
                return (false, string.Empty);
            }

            return (setting.SendUsageStatistics, setting.UserId);
        }

        void StartupUsageStatistics()
        {
            var userTracker = new UserTracker(LoggerFactory);
            ApplicationDiContainer.Register<IUserTracker, UserTracker>(userTracker);

            //var configuration = ApplicationDiContainer.Build<Configuration>();
            var setting = GetUsageStatistics();
            if(setting.sendUsageStatistics) {
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
            Logger.LogInformation("[Logging]" + Environment.NewLine + s);
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            StartupUsageStatistics();
            LoggingInformation();

            //var initializer = new ApplicationInitializer();
            //if(!initializer.Initialize(e.Args)) {
            //    return false;
            //}
            ApplicationDiContainer.Register<IPlatformTheme, PlatformThemeLoader>(PlatformThemeLoader);

            //setting.UserId

            //ApplicationDiContainer.Get<IDispatcherWrapper>().Invoke(() => {
            SetStaticPlatformTheme();
            SetDynamicPlatformTheme();
            //});

            MakeMessageWindow();
            RegisterPlugins();


            Logger = LoggerFactory.CreateLogger(GetType());
            Logger.LogDebug("初期化完了");

            if(IsFirstStartup) {
                // 初期登録の画面を表示
                ShowStartupView();
            }

            var tuner = ApplicationDiContainer.Build<DatabaseTuner>();
            tuner.Tune();

            return true;
        }

        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, LoggerFactory);
            return viewModel;
        }

        IReadOnlyList<LauncherGroupElement> CreateLauncherGroupElements()
        {
            var barrier = ApplicationDiContainer.Make<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Make<IDatabaseStatementLoader>();

            IList<Guid> launcherGroupIds;
            using(var commander = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Make<LauncherGroupsEntityDao>(new object[] { commander, commander.Implementation });
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
            var barrier = ApplicationDiContainer.Make<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Make<IDatabaseStatementLoader>();

            IList<Guid> noteIds;
            using(var commander = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Make<NotesEntityDao>(new object[] { commander, commander.Implementation });
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
                ToViewModel = m => ApplicationDiContainer.Make<LauncherToolbarNotifyAreaViewModel>(new[] { m })
            };
            return collection;
        }

        public ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> GetNoteCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel>(NoteElements) {
                ToViewModel = m => ApplicationDiContainer.Make<NoteNotifyAreaViewModel>(new[] { m })
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
            if(currentActiveWindowHandle == IntPtr.Zero) {
                currentActiveWindowHandle = NativeMethods.GetForegroundWindow();
            }

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
                .Concat(launcherToolbars)
                .Concat(notes)
                .Where(i => i.CanStartShowView)
                .ToList()
            ;
            foreach(var viewShowStater in viewShowStaters) {
                viewShowStater.StartView();
            }

#if DEBUG
            if(IsDevDebug) {
                return;
            }
#endif
            // ノート生成で最後のノートがアクティブになる対応。設定でも発生するけど起動時に何とかしていって思い
            if(currentActiveWindowHandle != IntPtr.Zero) {
                ApplicationDiContainer.Get<IDispatcherWrapper>().Begin(() => {
                    NativeMethods.SetActiveWindow(currentActiveWindowHandle);
                    NativeMethods.SetForegroundWindow(currentActiveWindowHandle);
                    MoveZOrderAllNotes(false);
                }, DispatcherPriority.SystemIdle);
            }
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
            StartHook();

            ExecuteElements();
#if DEBUG
            DebugExecuteAfter();
#endif
        }

        void CloseViewsCore(WindowKind windowKind)
        {
            var windowItems = WindowManager.GetWindowItems(windowKind).ToList();
            foreach(var windowItem in windowItems) {
                windowItem.Window.Close();
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

        public void Exit(bool ignoreUpdate)
        {
            Logger.LogInformation("おわる！");

            if(!ignoreUpdate && UpdateInfo.IsReady) {
                Debug.Assert(UpdateInfo.Path != null);

                Logger.LogInformation("アップデート処理起動");

                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = UpdateInfo.Path.Path;
                process.StartInfo.Arguments = UpdateInfo.Path.Option;
                process.StartInfo.WorkingDirectory = UpdateInfo.Path.WorkDirectoryPath;

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

        public void ShowCommandView()
        {
            if(!CommandElement.IsInitialized) {
                CommandElement.Initialize();
            }

            CommandElement.StartView();
        }


        void BackupSettings(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string backupFileBaseName, int enabledCount, string userBackupDirectoryPath)
        {
            // アプリケーション側バックアップ
            var settingBackupper = new SettingBackupper(LoggerFactory);
            settingBackupper.BackupUserSetting(sourceDirectory, targetDirectory, backupFileBaseName, enabledCount);

            // ユーザー設定側バックアップ
            var expandeduserBackupDirectoryPath = Environment.ExpandEnvironmentVariables(userBackupDirectoryPath ?? string.Empty);
            if(!string.IsNullOrWhiteSpace(expandeduserBackupDirectoryPath)) {
                var dir = new DirectoryInfo(expandeduserBackupDirectoryPath);
                settingBackupper.BackupUserSettingToCustomDirectory(sourceDirectory, dir);
            }
        }

        void BackupSettingsDefault()
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();

            // バックアップ処理開始
            string userBackupDirectoryPath;
            using(var commander = ApplicationDiContainer.Get<IMainDatabaseBarrier>().WaitRead()) {
                var appGeneralSettingEntityDao = ApplicationDiContainer.Build<AppGeneralSettingEntityDao>(commander, commander.Implementation);
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

        private void ResetScreenViewElements()
        {
            CloseLauncherToolbarViews();
            CloseNoteViews();

            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();

            ExecuteElements();
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
            if(UpdateInfo.State != UpdateState.None) {
                if(UpdateInfo.IsReady) {
                    Logger.LogInformation("アップデート準備完了");
                } else {
                    Logger.LogInformation("アップデート排他制御中");
                }
                return;
            }

            var updateChecker = ApplicationDiContainer.Build<UpdateChecker>();

            UpdateInfo.State = UpdateState.Checking;
            var appVersion = await updateChecker.CheckApplicationUpdateAsync().ConfigureAwait(false);
            if(appVersion == null) {
                Logger.LogInformation("アップデートなし");
                UpdateInfo.State = UpdateState.None;
                return;
            }

            Logger.LogInformation("アップデートあり: {0}", appVersion.Version);

            // CheckApplicationUpdateAsync で弾いてる
            //if(BuildStatus.Version < appVersion.MinimumVersion) {
            //    Logger.LogWarning("最低バージョン未満であるためバージョンアップ不可: 現在 = {0}, 要求 = {1}", BuildStatus.Version, appVersion.MinimumVersion);
            //    UpdateInfo.State = UpdateState.None;
            //    return;
            //}

            Logger.LogInformation("アップデート可能");

            var updateDownloader = ApplicationDiContainer.Build<UpdateDownloader>();

            if(updateCheckKind != UpdateCheckKind.ForceUpdate) {
                try {
                    //var releaseNoteItem = await updateDownloader.DownloadReleaseNoteAsync(appVersion);
                    ShowUpdateReleaseNote(appVersion);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    UpdateInfo.State = UpdateState.None;
                    return;
                }
            }
            if(updateCheckKind == UpdateCheckKind.CheckOnly) {
                UpdateInfo.State = UpdateState.None;
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var donwloadFilePath = Path.Combine(environmentParameters.MachineUpdateArchiveDirectory.FullName, appVersion.Version.ToString() + ".zip");
            var donwloadFile = new FileInfo(donwloadFilePath);
            UpdateInfo.State = UpdateState.Downloading;
            try {
                await updateDownloader.DownloadApplicationArchiveAsync(appVersion, donwloadFile).ConfigureAwait(false);
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                UpdateInfo.State = UpdateState.None;
                return;
            }

            Logger.LogInformation("アップデートファイル展開");
            UpdateInfo.State = UpdateState.Extracting;
            try {
                var directoryCleaner = new DirectoryCleaner(environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.Configuration.File.DirectoryRemoveWaitCount, environmentParameters.Configuration.File.DirectoryRemoveWaitTime, LoggerFactory);
                directoryCleaner.Clear(false);

                var archiveExtractor = ApplicationDiContainer.Build<ArchiveExtractor>();
                archiveExtractor.Extract(donwloadFile, environmentParameters.TemporaryApplicationExtractDirectory);

                var scriptFactory = ApplicationDiContainer.Build<ApplicationUpdateScriptFactory>();
                var exeutePathParameter = scriptFactory.CreateUpdateExecutePathParameter(environmentParameters.EtcUpdateScriptFile, environmentParameters.TemporaryDirectory, environmentParameters.TemporaryApplicationExtractDirectory, environmentParameters.RootDirectory);
                UpdateInfo.Path = exeutePathParameter;
                UpdateInfo.State = UpdateState.Ready;

            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                UpdateInfo.State = UpdateState.None;
            }

        }

        #endregion

        #region IOrderManager

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

        public StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process, IScreen screen)
        {
            var element = OrderManager.CreateStandardInputOutputElement(id, process, screen);
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

                    CloseViews();
                    DisposeElements();
                    DisposeWebView();

                    //MessageWindowDispatcherWapper?.Begin(() => {
                    //    MessageWindowHandleSource?.Dispose();
                    //    Dispatcher.CurrentDispatcher.InvokeShutdown();
                    //});
                    MessageWindowHandleSource?.Dispose();

                    NotifyManager.Dispose();
                    OrderManager.Dispose();

                    WindowManager.Dispose();
                    StatusManager.Dispose();
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
