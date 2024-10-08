using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.CrashReport.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.Widget;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting.Factory;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    internal partial class ApplicationManager: DisposerBase, IOrderManager
    {
        internal ApplicationManager(ApplicationInitializer initializer)
        {
            Logging = initializer.Logging ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.Logging));
            Logger = Logging.Factory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;


#if DEBUG
            IsDebugDevelopMode = initializer.IsDebugDevelopMode;
#endif

            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));
            var customConfiguration = ApplicationDiContainer.Get<ApplicationConfiguration>();

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

            NotifyManagerImpl.LauncherGroupItemRegistered += NotifyManagerImpl_LauncherGroupItemRegistered;

            // DIコンテナ登録(マネージャ系)
            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManagerImpl);
            ApplicationDiContainer.Register<IStatusManager, StatusManager>(StatusManagerImpl);
            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
            ApplicationDiContainer.Register<IUserAgentManager, UserAgentManager>(UserAgentManager);
            ApplicationDiContainer.Register<IHttpUserAgentFactory, IHttpUserAgentFactory>(UserAgentManager);

            ApplicationDiContainer.Register<LauncherItemAddonViewSupporterCollection, LauncherItemAddonViewSupporterCollection>(DiLifecycle.Singleton);

            var addonContainer = ApplicationDiContainer.Build<AddonContainer>();
            var themeContainer = ApplicationDiContainer.Build<ThemeContainer>();
            PluginContainer = ApplicationDiContainer.Build<PluginContainer>(addonContainer, themeContainer);
            ApplicationDiContainer.Register<ILauncherItemAddonFinder, ILauncherItemAddonFinder>(DiLifecycle.Transient, () => new LauncherItemAddonFinder(PluginContainer.Addon, LoggerFactory));

            // プラグインコンテナ自体を登録
            ApplicationDiContainer.Register<PluginContainer, PluginContainer>(PluginContainer);

            // テーマIFをDI登録
            ApplicationDiContainer.Register<IGeneralTheme, IGeneralTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetGeneralTheme());
            ApplicationDiContainer.Register<ILauncherToolbarTheme, ILauncherToolbarTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetLauncherToolbarTheme());
            ApplicationDiContainer.Register<INoteTheme, INoteTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNoteTheme());
            ApplicationDiContainer.Register<ICommandTheme, ICommandTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetCommandTheme());
            ApplicationDiContainer.Register<INotifyLogTheme, INotifyLogTheme>(DiLifecycle.Transient, () => PluginContainer.Theme.GetNotifyTheme());
            //// アドオンIFをDI登録
            //ApplicationDiContainer.Register<ICommandFinder, CommandFinderAddonWrapper>(DiLifecycle.Transient, () => PluginContainer.Addon.GetCommandFinder());

            // フルスクリーン検知処理の生成(設定項目が多いので生成後に値設定)
            var fullscreenWatcher = ApplicationDiContainer.Build<FullscreenWatcher>();
            var fullscreen = customConfiguration.Platform.Fullscreen;
            foreach(var item in fullscreen.IgnoreWindowClasses) {
                fullscreenWatcher.IgnoreFullscreenWindowClassNames.Add(item);
            }
            foreach(var item in fullscreen.IgnoreClassAndTexts) {
                fullscreenWatcher.ClassAndTexts.Add(item);
            }
            fullscreenWatcher.TopmostOnly = fullscreen.TopmostOnly;
            fullscreenWatcher.ExcludeNoActive = fullscreen.ExcludeNoActive;
            fullscreenWatcher.ExcludeToolWindow = fullscreen.ExcludeToolWindow;
            ApplicationDiContainer.Register<IFullscreenWatcher, FullscreenWatcher>(fullscreenWatcher);

            KeyboradHooker = new KeyboardHook(LoggerFactory);
            MouseHooker = new MouseHook(LoggerFactory);
            KeyActionChecker = new KeyActionChecker(LoggerFactory);
            KeyActionAssistant = new KeyActionAssistant(LoggerFactory);

            CronScheduler = ApplicationDiContainer.Build<CronScheduler>();

            ApplicationUpdateInfo = ApplicationDiContainer.Build<NewVersionInfo>();

            NotifyLogElement = ApplicationDiContainer.Build<NotifyLogElement>();
            _ = NotifyLogElement.InitializeAsync(CancellationToken.None);

            DelayScreenElementReset = ApplicationDiContainer.Build<DelayAction>(nameof(DelayScreenElementReset), customConfiguration.Platform.ScreenElementsResetWaitTime);

            LowScheduler = new System.Timers.Timer(customConfiguration.Schedule.LowSchedulerTime.TotalMilliseconds);
            LowScheduler.Elapsed += LowScheduler_Elapsed;

            if(!string.IsNullOrWhiteSpace(initializer.TestPluginDirectoryPath)) {
                TestPluginDirectory = new DirectoryInfo(initializer.TestPluginDirectoryPath);
                TestPluginName = initializer.TestPluginName;
            }
        }

        #region property

        private ApplicationLogging Logging { get; set; }
        private ILoggerFactory LoggerFactory => Logging.Factory;
        private ApplicationDiContainer ApplicationDiContainer { get; set; }
        private bool IsFirstStartup { get; }

#if DEBUG
        private bool IsDebugDevelopMode { get; }
#endif
        private ILogger Logger { get; set; }
        private PlatformThemeLoader PlatformThemeLoader { get; }

        private WindowManager WindowManager { get; set; }
        private OrderManagerImpl OrderManager { get; set; }
        private NotifyManager NotifyManagerImpl { get; set; }
        public INotifyManager NotifyManager => NotifyManagerImpl;
        private StatusManager StatusManagerImpl { get; set; }
        public IStatusManager StatusManager => StatusManagerImpl;
        private ClipboardManager ClipboardManager { get; set; }
        private UserAgentManager UserAgentManager { get; set; }

        private Mutex ApplicationMutex { get; }

        private ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        private ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        private ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();
        private ObservableCollection<StandardInputOutputElement> StandardInputOutputs { get; } = new ObservableCollection<StandardInputOutputElement>();
        private ObservableCollection<LauncherItemExtensionElement> LauncherItemExtensions { get; } = new ObservableCollection<LauncherItemExtensionElement>();
        private CommandElement? CommandElement { get; set; }
        private NotifyLogElement NotifyLogElement { get; }
        //FeedbackElement? FeedbackElement { get; set; }
        private HwndSource? MessageWindowHandleSource { get; set; }
        //IDispatcherWapper? MessageWindowDispatcherWapper { get; set; }

        private KeyboardHook KeyboradHooker { get; }
        private MouseHook MouseHooker { get; }
        private KeyActionChecker KeyActionChecker { get; }
        private KeyActionAssistant KeyActionAssistant { get; }

        PluginContainer PluginContainer { get; }

        private UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        public NewVersionInfo ApplicationUpdateInfo { get; }
        /// <summary>
        /// プラグインの新規バージョンが存在するか。
        /// </summary>
        public bool ExistsPluginChanges { get; private set; }

        public bool CanSendCrashReport => ApplicationDiContainer.Get<GeneralConfiguration>().CanSendCrashReport;
        public bool UnhandledExceptionHandled => ApplicationDiContainer.Get<GeneralConfiguration>().UnhandledExceptionHandled;

        private bool ResetWaiting { get; set; }
        private DelayAction DelayScreenElementReset { get; }

        private DirectoryInfo? TestPluginDirectory { get; }
        private string TestPluginName { get; } = string.Empty;

        private ObservableCollection<WidgetElement> Widgets { get; } = new ObservableCollection<WidgetElement>();

        #endregion

        #region function

        /// <summary>
        /// すべてここで完結する神の所業。
        /// </summary>
        public async Task ShowSettingViewAsync(CancellationToken cancellationToken)
        {
            SaveWidgets();
            using var viewPausing = PauseAllViews();

            Logger.LogDebug("遅延書き込み処理停止");
            var delayWriterPack = ApplicationDiContainer.Get<IDatabaseDelayWriterPack>();
            var delayWriterItemMap = new Dictionary<IDatabaseDelayWriter, IDisposable>();
            foreach(var delayWriter in delayWriterPack.Items) {
                delayWriter.Flush();
                var pausing = delayWriter.Pause();
                delayWriterItemMap.Add(delayWriter, pausing);
            }

            // 現在DBを編集用として再構築
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var settingDirectory = environmentParameters.TemporarySettingDirectory;
            var directoryCleaner = new DirectoryCleaner(settingDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
            directoryCleaner.Clear(false);

            var settings = new {
                Main = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.MainFile.Name)),
                File = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.LargeFile.Name)),
            };
            //var settingDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.SettingFile.Name));
            //var fileDatabaseFile = new FileInfo(Path.Combine(settingDirectory.FullName, environmentParameters.FileFile.Name));

            environmentParameters.MainFile.CopyTo(settings.Main.FullName);
            environmentParameters.LargeFile.CopyTo(settings.File.FullName);

            // DIを設定処理用に付け替え
            var container = ApplicationDiContainer.Scope();
            var factory = new ApplicationDatabaseFactoryPack(
                new ApplicationDatabaseFactory(settings.Main, true, false),
                new ApplicationDatabaseFactory(settings.File, true, false),
                new ApplicationDatabaseFactory(true, false)
            );
            var delayWriterWaitTimePack = new DelayWriterWaitTimePack(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            container
                .Register<IDiContainer, DiContainer>((DiContainer)container) // むりやりぃ
                .Register<ISettingNotifyManager, SettingNotifyManager>(DiLifecycle.Singleton)
                .RegisterDatabase(factory, delayWriterWaitTimePack, LoggerFactory)
            ;

            var workingDatabasePack = ApplicationDiContainer.Build<IDatabaseAccessorPack>();
            var settingDatabasePack = container.Build<IDatabaseAccessorPack>();
            PersistenceHelper.Copy(workingDatabasePack.Temporary, settingDatabasePack.Temporary);

            var settingElement = new SettingContainerElement(container, Logging.PauseReceiveLog, container.Build<ISettingElementFactory>(), container.Build<ILoggerFactory>());
            await settingElement.InitializeAsync(cancellationToken);
            var windowItem = OrderManager.CreateSettingWindow(settingElement);
            WindowManager.Register(windowItem);
            var dialogResult = windowItem.Window.ShowDialog();

            static void EndPreferences(SettingContainerElement settingElement, ILogger logger)
            {
                foreach(var element in settingElement.PluginsSettingEditor.PluginItems) {
                    if(element.SupportedPreferences && element.StartedPreferences) {
                        logger.LogTrace("プラグイン処理設定完了: {0}({1})", element.PluginState.PluginName, element.PluginState.PluginId);
                        element.EndPreferences();
                    }
                }
            }

            if(settingElement.IsSubmit) {
                Logger.LogInformation("設定適用のため現在表示要素の破棄");
                CloseViews(false);
                DisposeElements();

                // 設定用DBを永続用DBと切り替え
                var pack = ApplicationDiContainer.Get<IDatabaseAccessorPack>();
                var stoppings = (new IDatabaseAccessor[] { pack.Main, pack.Large })
                    .Select(i => i.PauseConnection())
                    .ToList()
                ;

                BackupSettingsDefault(container);

                var accessorPack = container.Get<IDatabaseAccessorPack>();
                var databaseSetupper = container.Build<DatabaseSetupper>();
                foreach(var accessor in accessorPack.Items) {
                    databaseSetupper.Adjust(accessor);
                }

                settings.Main.CopyTo(environmentParameters.MainFile.FullName, true);
                settings.File.CopyTo(environmentParameters.LargeFile.FullName, true);

                foreach(var stopping in stoppings) {
                    stopping.Dispose();
                }
                var cultureServiceChanger = ApplicationDiContainer.Build<CultureServiceChanger>(CultureService.Instance);
                cultureServiceChanger.ChangeCulture();

                PersistenceHelper.Copy(settingDatabasePack.Temporary, workingDatabasePack.Temporary);

                Logger.LogInformation("設定適用のため各要素生成");

                EndPreferences(settingElement, Logger);

                // 更新したランチャーアイテムの更新通知
                foreach(var launcherItemId in settingElement.AllLauncherItems.Where(i => !i.IsLazyLoad).Select(i => i.LauncherItemId)) {
                    OrderManager.RefreshLauncherItemElement(launcherItemId);
                }

                ApplyThemeSetting();
                RebuildHook();
                RebuildSchedulerSetting();
                ResetNotifyArea();
                await ExecuteElementsAsync(cancellationToken);

                if(CommandElement != null) {
                    if(CommandElement.IsInitialized) {
                        await CommandElement.RefreshAsync(cancellationToken);
                    }
                }
                await NotifyLogElement.RefreshAsync(cancellationToken);
                NotifyManager.SendSettingChanged();

                ExistsPluginChanges = CheckPluginChanges();
            } else {
                Logger.LogInformation("設定は保存されなかったため現在要素継続");
                EndPreferences(settingElement, Logger);
            }

            Logger.LogDebug("遅延書き込み処理再開");
            foreach(var pair in delayWriterItemMap) {
                if(settingElement.IsSubmit) {
                    // 確定処理の書き込みが天に召されるのでため込んでいた処理(ないはず)を消す
                    pair.Key.ClearStock();
                }
                pair.Value.Dispose();
            }

            settingElement.Dispose();
            container.UnregisterDatabase();
            container.Dispose();
        }

        /// <summary>
        /// 一時的に表示系と表示処理に起因する処理を停止する。
        /// </summary>
        /// <remarks>
        /// <para>モーダルダイアログ表示の際に使用する。</para>
        /// </remarks>
        /// <returns></returns>
        private IDisposable PauseAllViews()
        {
            StopPlatform();
            StopScheduler();
            StopHook();
            FinalizeSystem();

            KeyActionChecker.Reset();
            NotifyManagerImpl.ClearAllLogs();
            KeyboardNotifyLogId = NotifyLogId.Empty;

            if(CommandElement != null) {
                if(CommandElement.ViewCreated) {
                    CommandElement.HideView(true);
                }
            }
            if(NotifyLogElement.ViewCreated) {
                NotifyLogElement.HideView(true);
            }

            var silent = NotifyLogElement.ToSilent();
            var changing = StatusManagerImpl.ChangeLimitedBoolean(StatusProperty.CanCallNotifyAreaMenu, false);

            if(BackgroundAddon != null) {
                if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                    var context = new BackgroundAddonProxyRunPauseContext(true);
                    BackgroundAddon.RunPause(context);
                }
            }

            return new ActionDisposer(d => {
                StartHook();
                StartScheduler();
                StartBackground();
                StartPlatform();
                InitializeSystem();

                if(BackgroundAddon != null) {
                    if(BackgroundAddon.IsSupported(Bridge.Plugin.Addon.BackgroundKind.KeyboardHook)) {
                        var context = new BackgroundAddonProxyRunPauseContext(false);
                        BackgroundAddon.RunPause(context);
                    }
                }

                if(changing.Success) {
                    changing.SuccessValue?.Dispose();
                }
                silent.Dispose();
            });
        }

        public async Task ShowStartupViewAsync(bool isFirstSetup, CancellationToken cancellationToken)
        {
            using var viewPausing = isFirstSetup
                ? new ActionDisposer(d => { Logger.LogInformation("初回スタートアップ終了"); })
                : PauseAllViews()
            ;

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModels.Startup.StartupViewModel, Views.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                await startupModel.InitializeAsync(cancellationToken);
                var view = diContainer.Build<Views.Startup.StartupWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.Startup, startupModel, view));

                view.ShowDialog();

                if(!isFirstSetup) {
                    if(startupModel.IsRegisteredLauncher) {
                        await ResetScreenViewElementsAsync(cancellationToken);
                    }
                }
            }
        }

#if DEBUG
        private async Task StartDebugDevelopModeAsync(CancellationToken cancellationToken)
        {
            var importProgramsElement = ApplicationDiContainer.Build<Element.Startup.ImportProgramsElement>();
            await importProgramsElement.LoadProgramsAsync(cancellationToken);
            await importProgramsElement.ImportAsync(cancellationToken);

            var mainBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();

            var commandKeyActionData = new KeyActionData() {
                KeyActionId = idFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.Command,
                KeyActionContent = string.Empty,
                Comment = "debug-dev-mode",
            };
            var commandKeyMappings = new[] {
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.Any,
                    Shift = ModifierKey.Any,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Space,
                }
            };

            var hideToolbarKeyActionData = new KeyActionData() {
                KeyActionId = idFactory.CreateKeyActionId(),
                KeyActionKind = KeyActionKind.LauncherToolbar,
                KeyActionContent = KeyActionContentLauncherToolbar.AutoHiddenToHide.ToString(),
                Comment = "debug-dev-mode",
            };
            var hideToolbarKeyMappings = new[] {
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.None,
                    Shift = ModifierKey.None,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Escape,
                },
                new KeyMappingData() {
                    Alt = ModifierKey.None,
                    Control = ModifierKey.None,
                    Shift = ModifierKey.None,
                    Super = ModifierKey.None,
                    Key = System.Windows.Input.Key.Escape,
                }
            };


            var pressedOptionConverter = new PressedOptionConverter();

            using(var context = mainBarrier.WaitWrite()) {
                var status = new DatabaseCommonStatus() {
                    Account = "🍶",
                    ProgramName = "🍻",
                    ProgramVersion = BuildStatus.Version,
                };
                var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
                var keyActionsEntityDao = ApplicationDiContainer.Build<KeyActionsEntityDao>(context, context.Implementation);
                var keyOptionsEntityDao = ApplicationDiContainer.Build<KeyOptionsEntityDao>(context, context.Implementation);
                var keyMappingsEntityDao = ApplicationDiContainer.Build<KeyMappingsEntityDao>(context, context.Implementation);

                var userIdManager = ApplicationDiContainer.Build<UserIdManager>();
                appExecuteSettingEntityDao.UpdateExecuteSettingAcceptInput(userIdManager.CreateFromRandom(), true, status);

                keyActionsEntityDao.InsertKeyAction(commandKeyActionData, status);
                keyOptionsEntityDao.InsertOption(commandKeyActionData.KeyActionId, KeyActionPressOption.ThroughSystem.ToString(), false.ToString(), status);
                foreach(var item in commandKeyMappings.Counting()) {
                    keyMappingsEntityDao.InsertMapping(commandKeyActionData.KeyActionId, item.Value, item.Number, status);
                }

                keyActionsEntityDao.InsertKeyAction(hideToolbarKeyActionData, status);
                keyOptionsEntityDao.InsertOption(hideToolbarKeyActionData.KeyActionId, KeyActionPressOption.ThroughSystem.ToString(), true.ToString(), status);
                foreach(var item in hideToolbarKeyMappings.Counting()) {
                    keyMappingsEntityDao.InsertMapping(hideToolbarKeyActionData.KeyActionId, item.Value, item.Number, status);
                }


                context.Commit();
            }
        }
#endif

        public async Task ShowAboutViewAsync(CancellationToken cancellationToken)
        {
            using var viewPausing = PauseAllViews();

            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterMvvm<Element.About.AboutElement, ViewModels.About.AboutViewModel, Views.About.AboutWindow>()
                ;
                var model = diContainer.New<Element.About.AboutElement>();
                await model.InitializeAsync(cancellationToken);

                var view = diContainer.Build<Views.About.AboutWindow>();

                var windowManager = diContainer.Get<IWindowManager>();
                windowManager.Register(new WindowItem(WindowKind.About, model, view));

                view.ShowDialog();
            }
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

        private async Task ShowNewVersionReleaseNoteCoreAsync(NewVersionItemData updateItem, bool isCheckOnly, CancellationToken cancellationToken)
        {
            var element = ApplicationDiContainer.Build<Element.ReleaseNote.ReleaseNoteElement>(ApplicationUpdateInfo, updateItem, isCheckOnly);
            await element.InitializeAsync(cancellationToken);
            var view = ApplicationDiContainer.Build<Views.ReleaseNote.ReleaseNoteWindow>();
            view.DataContext = ApplicationDiContainer.Build<ViewModels.ReleaseNote.ReleaseNoteViewModel>(element);
            WindowManager.Register(new WindowItem(WindowKind.Release, element, view));
            view.Show();
        }

        private async Task ShowNewVersionReleaseNoteAsync(NewVersionItemData updateItem, bool isCheckOnly, CancellationToken cancellationToken)
        {
            var windowItem = WindowManager.GetWindowItems(WindowKind.Release);
            if(windowItem.Any()) {
                // 再表示
                await ApplicationDiContainer.Build<IDispatcherWrapper>().BeginAsync(async () => {
                    var window = windowItem.FirstOrDefault();
                    if(window != null) {
                        window.Window.Activate();
                    } else {
                        await ShowNewVersionReleaseNoteCoreAsync(updateItem, isCheckOnly, cancellationToken);
                    }
                }, DispatcherPriority.ApplicationIdle);
                return;
            }

            await ApplicationDiContainer.Build<IDispatcherWrapper>().BeginAsync(async () => {
                await ShowNewVersionReleaseNoteCoreAsync(updateItem, isCheckOnly, cancellationToken);
            }, DispatcherPriority.ApplicationIdle);
        }

        private void InstallLatestPlugins()
        {
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();

            var directoryMover = ApplicationDiContainer.Build<DirectoryMover>();

            var dirs = environmentParameters.MachinePluginInstallDirectory.EnumerateDirectories();
            foreach(var dir in dirs) {
                var destDirPath = Path.Combine(environmentParameters.MachinePluginModuleDirectory.FullName, dir.Name);
                var destDir = new DirectoryInfo(destDirPath);
                Logger.LogInformation("新規プラグイン: {0}", destDirPath);
                directoryMover.Move(dir, destDir);
            }

            var directoryCleaner = new DirectoryCleaner(environmentParameters.MachinePluginInstallDirectory, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitCount, environmentParameters.ApplicationConfiguration.File.DirectoryRemoveWaitTime, LoggerFactory);
            directoryCleaner.Clear(false);
        }

        /// <summary>
        /// 全プラグインの読み込み。
        /// </summary>
        /// <remarks>
        /// <para>管理者権限実行時は読み込まない。</para>
        /// </remarks>
        private void LoadPlugins()
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();

            // プラグイン情報取得
            var pluginStateItems = ApplicationDiContainer.Build<IMainDatabaseBarrier>().ReadData(c => {
                var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(c, c.Implementation);
                return pluginsEntityDao.SelectPluginStateData().ToList();
            });

            // アンインストール対象を消しちゃう
            var uninstallPlugins = pluginStateItems.Where(i => i.State == PluginState.Uninstall);
            var uninstalledPlugins = new List<PluginStateData>();
            foreach(var uninstallPlugin in uninstallPlugins) {
                // なんかが失敗したときに後続を続けたいので毎度ロールバックする
                using var pack = PersistenceHelper.WaitWritePack(
                    ApplicationDiContainer.Build<IMainDatabaseBarrier>(),
                    ApplicationDiContainer.Build<ILargeDatabaseBarrier>(),
                    ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>(),
                    DatabaseCommonStatus.CreateCurrentAccount()
                );
                try {
                    var uninstaller = ApplicationDiContainer.Build<PluginUninstaller>(pack, environmentParameters.MachinePluginModuleDirectory);
                    uninstaller.Uninstall(uninstallPlugin);
                    pack.Commit();
                    uninstalledPlugins.Add(uninstallPlugin);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }

            var enabledPluginLoadStateItems = new List<PluginLoadStateData>();

            var userRole = new UserRole();
            if(userRole.IsRunningAdministrator()) {
                Logger.LogWarning("管理権限実行中のためプラグイン読み込み処理を無視");
            } else {
                var enabledPlugins = pluginStateItems.Except(uninstalledPlugins).ToArray();

                // プラグインディレクトリからプラグインDLL列挙
                var pluginFiles = PluginContainer.GetPluginFiles(environmentParameters.MachinePluginModuleDirectory, environmentParameters.ApplicationConfiguration.Plugin.IgnoreBaseFileNames, environmentParameters.ApplicationConfiguration.Plugin.Extensions);

                FileInfo? testPluginFile = null;
                if(TestPluginDirectory != null) {
                    var pluginName = string.IsNullOrWhiteSpace(TestPluginName) ? TestPluginDirectory.Name : TestPluginName;
                    testPluginFile = PluginContainer.GetPluginFile(TestPluginDirectory, pluginName, environmentParameters.ApplicationConfiguration.Plugin.Extensions);
                }

                // プラグインを読み込み、プラグイン情報と突合して使用可能・不可を検証
                var pluginLoadStateItems = new List<PluginLoadStateData>();
                var pluginConstructorContext = ApplicationDiContainer.Build<PluginConstructorContext>();
                foreach(var pluginFile in pluginFiles) {
                    var loadStateData = PluginContainer.LoadPlugin(pluginFile, enabledPlugins, BuildStatus.Version, pluginConstructorContext, Logging.PauseReceiveLog);
                    pluginLoadStateItems.Add(loadStateData);
                }

                PluginLoadStateData? testPluginLoadState = null;
                if(testPluginFile != null) {
                    testPluginLoadState = PluginContainer.LoadPlugin(testPluginFile, enabledPlugins, BuildStatus.Version, pluginConstructorContext, Logging.PauseReceiveLog);
                    pluginLoadStateItems.Add(testPluginLoadState);
                }

                // 戻ってきた突合情報を反映
                using(var context = ApplicationDiContainer.Build<IMainDatabaseBarrier>().WaitWrite()) {
                    var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                    var pluginVersionChecksEntityDao = ApplicationDiContainer.Build<PluginVersionChecksEntityDao>(context, context.Implementation);
                    foreach(var pluginLoadStateItem in pluginLoadStateItems) {
                        // プラグインIDすら取得できなかったぶっこわれアセンブリは無視
                        if(pluginLoadStateItem.PluginId == PluginId.Empty && pluginLoadStateItem.LoadState == PluginState.IllegalAssembly) {
                            Logger.LogWarning("プラグイン {0} はもろもろおかしい", pluginLoadStateItem.PluginName);
                            if(pluginLoadStateItem == testPluginLoadState) {
#if DEBUG
                                if(Debugger.IsAttached) {
                                    Debugger.Break();
                                }
#endif
                                Logger.LogWarning("テスト用プラグインはおかしいためデータ登録処理スキップ: {0}, {1}", testPluginFile!.FullName, pluginLoadStateItem.LoadState);
                            }
                            continue;
                        }

                        var pluginStateData = new PluginStateData() {
                            PluginId = pluginLoadStateItem.PluginId,
                            PluginName = pluginLoadStateItem.PluginName,
                            State = pluginLoadStateItem.LoadState
                        };

                        if(pluginLoadStateItem == testPluginLoadState) {
                            if(pluginStateData.State == PluginState.Disable) {
#if DEBUG
                                if(Debugger.IsAttached) {
                                    Debugger.Break();
                                }
#endif
                                Logger.LogWarning("テスト用プラグインは読み込み失敗したためデータ登録処理スキップ: {0}, {1}", testPluginFile!.FullName, pluginLoadStateItem.LoadState);
                                continue;
                            }
                        }

                        if(pluginsEntityDao.SelectExistsPlugin(pluginLoadStateItem.PluginId)) {
                            pluginsEntityDao.UpdatePluginStateData(pluginStateData, DatabaseCommonStatus.CreateCurrentAccount());
                        } else {
                            pluginsEntityDao.InsertPluginStateData(pluginStateData, DatabaseCommonStatus.CreateCurrentAccount());
                        }

                        // 読み込みOKの際に更新URLの再設定
                        if(pluginLoadStateItem.Plugin != null) {
                            pluginVersionChecksEntityDao.DeletePluginVersionChecks(pluginLoadStateItem.PluginId);
                            foreach(var countUrl in pluginLoadStateItem.Plugin.PluginInformation.PluginVersions.CheckUrls.Counting()) {
                                pluginVersionChecksEntityDao.InsertPluginVersionCheckUrl(pluginLoadStateItem.PluginId, countUrl.Number * PluginUtility.CheckVersionStep, countUrl.Value, DatabaseCommonStatus.CreateCurrentAccount());
                            }
                        }
                    }

                    context.Commit();
                }

                enabledPluginLoadStateItems = pluginLoadStateItems
                    .Where(i => i.LoadState == PluginState.Enable)
                    .ToList()
                ;
                var disabledPluginLoadStateItems = pluginLoadStateItems
                    .Except(enabledPluginLoadStateItems)
                    .Where(i => i.LoadContext is not null)
                    .ToList()
                ;
                if(0 < disabledPluginLoadStateItems.Count) {
                    var disabledItems = disabledPluginLoadStateItems
                        .Select(a => (
                            item: a,
                            loadContext: new WeakReference<PluginAssemblyLoadContext>(a.FreeLoadContext())
                        ))
                        .ToArray()
                    ;

                    // アンロード対象の解放待ち
                    foreach(var counter in new Counter(10)) {
                        if(counter.IsFirst || counter.IsLast || (counter.CurrentCount == counter.MaxCount / 2)) {
                            GarbageCollection(true);
                        } else {
                            GarbageCollection(false);
                        }

                        var unloadedItems = new List<PluginLoadStateData>();
                        foreach(var disabledItem in disabledItems) {
                            if(disabledItem.loadContext.TryGetTarget(out _)) {
                                Logger.LogInformation("[{0}/{1}] アンロード待ち: {2}, {3}", counter.CurrentCount, counter.MaxCount, disabledItem.item.PluginName, disabledItem.item.PluginId);
                            } else {
                                Logger.LogInformation("[{0}/{1}] アンロード完了: {2}, {3}", counter.CurrentCount, counter.MaxCount, disabledItem.item.PluginName, disabledItem.item.PluginId);
                                unloadedItems.Add(disabledItem.item);
                            }
                        }
                        foreach(var removeItem in unloadedItems) {
                            disabledPluginLoadStateItems.Remove(removeItem);
                        }
                        if(disabledPluginLoadStateItems.Count == 0) {
                            break;
                        }
                    }
                    // NOTE: disabledPluginLoadStateItem.FreeLoadContext により再実施はもう出来なくなった
                    //if(0 < disabledPluginLoadStateItems.Count) {
                    //    GarbageCollection(true);
                    //    foreach(var disabledPluginLoadStateItem in disabledPluginLoadStateItems) {
                    //        if(disabledPluginLoadStateItem.WeakLoadContext!.TryGetTarget(out _)) {
                    //            Logger.LogWarning("[LAST] アンロード待機超過: {0}, {1}", disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                    //        } else {
                    //            Logger.LogInformation("[LAST] アンロード完了: {0}, {1}", disabledPluginLoadStateItem.PluginName, disabledPluginLoadStateItem.PluginId);
                    //        }
                    //    }
                    //}

                    if(disabledPluginLoadStateItems.Count == 0) {
                        Logger.LogInformation("不要プラグイン解放完了");
                    } else {
                        Logger.LogWarning("不要プラグイン解放不完全: {0}", disabledPluginLoadStateItems.Count);
                    }
                }
            }

            var applicationPluginTypes = new List<Type>() {
                typeof(DefaultTheme),
            };
            var applicationPlugins = new List<IPlugin>(applicationPluginTypes.Count);
            foreach(var type in applicationPluginTypes) {
                using var context = ApplicationDiContainer.Build<PluginConstructorContext>();
                var appPlugin = (IPlugin)ApplicationDiContainer.New(type, new object[] { context });
                applicationPlugins.Add(appPlugin);
            }

            var initializedPluginItems = new List<(IPlugin plugin, PluginAssemblyLoadContext? loadContext)>(enabledPluginLoadStateItems.Count + applicationPlugins.Count);

            var databaseBarrierPack = ApplicationDiContainer.Build<IDatabaseBarrierPack>();

            // Pe専用プラグイン
            foreach(var plugin in applicationPlugins) {
                using(var readerPack = databaseBarrierPack.WaitRead()) {
                    using var context = pluginContextFactory.CreateInitializeContext(plugin.PluginInformation, readerPack);
                    plugin.Initialize(context);
                }
                initializedPluginItems.Add((plugin, null));
            }

            // 通常プラグイン
            foreach(var pluginLoadStateItem in enabledPluginLoadStateItems) {
                Debug.Assert(pluginLoadStateItem.Plugin != null);

                Logger.LogInformation("プラグイン初期化処理: {0}, {1}", pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                var plugin = pluginLoadStateItem.Plugin;
                try {
                    using(var readerPack = databaseBarrierPack.WaitRead()) {
                        using var context = pluginContextFactory.CreateInitializeContext(plugin.PluginInformation, readerPack);
                        plugin.Initialize(context);
                    }
                    Debug.Assert(pluginLoadStateItem.LoadContext is not null);
                    initializedPluginItems.Add((plugin, pluginLoadStateItem.LoadContext));
                } catch(Exception ex) {
                    Logger.LogError(ex, "プラグイン初期化失敗: {0}, {1}, {2}", ex.Message, pluginLoadStateItem.PluginName, pluginLoadStateItem.PluginId);
                    pluginLoadStateItem.LoadContext?.Unload();
                }
            }

            foreach(var pluginItems in initializedPluginItems) {
                Logger.LogInformation("初期化完了プラグイン: {0}, {1}, {2}", pluginItems.plugin.PluginInformation.PluginIdentifiers.PluginName, pluginItems.plugin.PluginInformation.PluginVersions.PluginVersion, pluginItems.plugin.PluginInformation.PluginIdentifiers.PluginId);
                PluginContainer.AddPlugin(pluginItems.plugin, pluginItems.loadContext);
            }

            // プラグイン情報を更新
            if(0 < initializedPluginItems.Count) {
                using(var context = ApplicationDiContainer.Build<IMainDatabaseBarrier>().WaitWrite()) {
                    var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                    foreach(var initializedPluginItem in initializedPluginItems) {
                        pluginsEntityDao.UpdatePluginRunningState(
                            initializedPluginItem.plugin.PluginInformation.PluginIdentifiers.PluginId,
                            initializedPluginItem.plugin.PluginInformation.PluginVersions.PluginVersion,
                            BuildStatus.Version,
                            DatabaseCommonStatus.CreateCurrentAccount()
                        );
                    }

                    context.Commit();
                }
            }
        }

        private void UnloadPlugins()
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
            var plugins = PluginContainer.Plugins.Where(i => i.IsInitialized).ToList();
            var themePlugins = plugins.Where(i => i.IsLoaded(PluginKind.Theme)).Select(i => new { Plugin = i, Kind = PluginKind.Theme });
            var addonPlugins = plugins.Where(i => i.IsLoaded(PluginKind.Addon)).Select(i => new { Plugin = i, Kind = PluginKind.Addon });

            using(var writer = pluginContextFactory.BarrierWrite()) {
                foreach(var item in addonPlugins.Concat(themePlugins)) {
                    using var context = pluginContextFactory.CreateUnloadContext(item.Plugin.PluginInformation, writer);
                    try {
                        item.Plugin.Unload(item.Kind, context);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "{0}({1}) {2}", item.Plugin.PluginInformation.PluginIdentifiers.PluginName, item.Plugin.PluginInformation.PluginIdentifiers.PluginId, ex.Message);
                    }
                }

                foreach(var plugin in plugins) {
                    using var context = pluginContextFactory.CreateFinalizeContext(plugin.PluginInformation, writer);
                    try {
                        plugin.Finalize(context);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "{0}({1}) {2}", plugin.PluginInformation.PluginIdentifiers.PluginName, plugin.PluginInformation.PluginIdentifiers.PluginId, ex.Message);
                    }
                }

                pluginContextFactory.Save();
            }

        }

        private void ApplyThemeSetting()
        {
            var themePluginId = ApplicationDiContainer.Build<IMainDatabaseBarrier>().ReadData(c => {
                var appGeneralSettingEntityDao = ApplicationDiContainer.Build<AppGeneralSettingEntityDao>(c, c.Implementation);
                try {
                    return appGeneralSettingEntityDao.SelectThemePluginId();
                } catch(Exception ex) {
                    Logger.LogWarning(ex, "テーマプラグインID取得失敗のため標準テーマを使用");
                    return DefaultTheme.Information.PluginIdentifiers.PluginId;
                }
            });
            ApplyCurrentTheme(themePluginId);
        }

        private void ApplyCurrentTheme(PluginId themePluginId)
        {
            var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();

            PluginContainer.Theme.SetCurrentTheme(themePluginId, pluginContextFactory);
        }

        private void SetStaticPlatformTheme()
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

        private void SetDynamicPlatformTheme()
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().VerifyAccess();

            var colors = PlatformThemeLoader.ApplicationThemeKind switch {
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

        private (bool isEnabledTelemetry, string userId) GetTelemetry()
        {
            var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            SettingAppExecuteSettingData setting;
            using(var context = mainDatabaseBarrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(context, context.Implementation);
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

        private void StartupUsageStatistics()
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
            Logger.LogInformation("[各種情報]{NewLine}{Information}", Environment.NewLine, s);
        }

        public async Task<bool> StartupAsync(App app, StartupEventArgs e, CancellationToken cancellationToken)
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

            InstallLatestPlugins();
            LoadPlugins();

            ApplyThemeSetting();

            Logger = LoggerFactory.CreateLogger(GetType());
            Logger.LogDebug("初期化完了");

            if(IsFirstStartup) {
                // 初期登録の画面を表示
#if DEBUG
                if(IsDebugDevelopMode) {
                    await StartDebugDevelopModeAsync(cancellationToken);
                } else {
                    await ShowStartupViewAsync(true, cancellationToken);
                }
#else
                await ShowStartupViewAsync(true, cancellationToken);
#endif
            }

            var tuner = ApplicationDiContainer.Build<DatabaseAdjuster>();
            tuner.Adjust();

            return true;
        }

        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, ApplicationDiContainer.Build<IKeyGestureGuide>(), ApplicationDiContainer.Build<IUserTracker>(), LoggerFactory);
            return viewModel;
        }

        private async Task<IReadOnlyList<LauncherGroupElement>> CreateLauncherGroupElementsAsync(CancellationToken cancellationToken)
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<LauncherGroupId> launcherGroupIds;
            using(var context = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<LauncherGroupsEntityDao>(context, context.Implementation);
                launcherGroupIds = dao.SelectAllLauncherGroupIds().ToList();
            }

            var result = new List<LauncherGroupElement>(launcherGroupIds.Count);
            foreach(var launcherGroupId in launcherGroupIds) {
                var element = await CreateLauncherGroupElementAsync(launcherGroupId, cancellationToken);
                result.Add(element);
            }

            return result;
        }

        private async Task<IReadOnlyList<LauncherToolbarElement>> CreateLauncherToolbarElementsAsync(ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, CancellationToken cancellationToken)
        {
            var screens = Screen.AllScreens;
            var result = new List<LauncherToolbarElement>(screens.Length);

            foreach(var screen in screens.OrderByDescending(i => i.Primary)) {
                var element = await CreateLauncherToolbarElementAsync(screen, launcherGroups, cancellationToken);
                result.Add(element);
            }

            return result;
        }

        private async Task<IReadOnlyList<NoteElement>> CreateNoteElementsAsync(CancellationToken cancellationToken)
        {
            var barrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();

            IList<NoteId> noteIds;
            using(var context = barrier.WaitRead()) {
                var dao = ApplicationDiContainer.Build<NotesEntityDao>(context, context.Implementation);
                noteIds = dao.SelectAllNoteIds().ToList();
            }

            var result = new List<NoteElement>(noteIds.Count);
            foreach(var noteId in noteIds) {
                var element = await CreateNoteElementAsync(noteId, default(IScreen), NoteStartupPosition.Setting, cancellationToken);
                result.Add(element);
            }

            return result;
        }

        public ModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> GetLauncherNotifyCollection()
        {
            var collection = new ModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>(LauncherToolbarElements, new ModelViewModelObservableCollectionOptions<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>() {
                ToViewModel = m => ApplicationDiContainer.Build<LauncherToolbarNotifyAreaViewModel>(m)
            });
            return collection;
        }

        public ModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel> GetNoteCollection()
        {
            var collection = new ModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel>(NoteElements, new ModelViewModelObservableCollectionOptions<NoteElement, NoteNotifyAreaViewModel>() {
                ToViewModel = m => ApplicationDiContainer.Build<NoteNotifyAreaViewModel>(m)
            });
            return collection;
        }

        public ModelViewModelObservableCollectionManager<WidgetElement, WidgetNotifyAreaViewModel> GetWidgetCollection()
        {
            var collection = new ModelViewModelObservableCollectionManager<WidgetElement, WidgetNotifyAreaViewModel>(Widgets, new ModelViewModelObservableCollectionOptions<WidgetElement, WidgetNotifyAreaViewModel>() {
                ToViewModel = m => ApplicationDiContainer.Build<WidgetNotifyAreaViewModel>(m),
            });
            return collection;
        }

        public async Task<NoteElement> CreateNoteAsync(IScreen dockScreen, NoteStartupPosition noteStartupPosition, CancellationToken cancellationToken)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.LogInformation("new note id: {0}, {1}", noteId, ObjectDumper.GetDumpString(dockScreen));
            var noteElement = await CreateNoteElementAsync(noteId, dockScreen, noteStartupPosition, cancellationToken);

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
                dispatcherWrapper.BeginAsync(() => {
                    foreach(var noteElement in noteElements) {
                        noteElement.SetTopmost(true);
                    }
                }, DispatcherPriority.SystemIdle).ContinueWith(t => {
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
                    WindowsUtility.MoveZoderBottom(hWnd);
                }
            }
        }

        private async Task ExecuteElementsAsync(CancellationToken cancellationToken)
        {
            var currentActiveWindowHandle = NativeMethods.GetActiveWindow();
            //if(currentActiveWindowHandle == IntPtr.Zero) {
            //    currentActiveWindowHandle = NativeMethods.GetForegroundWindow();
            //}

            // グループ構築
            var launcherGroups = await CreateLauncherGroupElementsAsync(cancellationToken);
            LauncherGroupElements.AddRange(launcherGroups);

            // ツールバーの生成
            var launcherToolbars = await CreateLauncherToolbarElementsAsync(new ReadOnlyObservableCollection<LauncherGroupElement>(LauncherGroupElements), cancellationToken);
            LauncherToolbarElements.AddRange(launcherToolbars);

            // ノートの生成
            var notes = await CreateNoteElementsAsync(cancellationToken);
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

            await ExecuteWidgetsAsync(cancellationToken);

#if DEBUG
            if(IsDevDebug) {
                Logger.LogWarning("{IsDevDebug}が有効", nameof(IsDevDebug));
                return;
            }
#endif
            await ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
                // ノート生成で最後のノートがアクティブになる対応。設定でも発生するけど起動時に何とかしていって思い
                if(currentActiveWindowHandle != IntPtr.Zero && currentActiveWindowHandle != MessageWindowHandleSource?.Handle) {
                    WindowsUtility.ShowActive(currentActiveWindowHandle);
                }
                MoveZOrderAllNotes(false);
            }, DispatcherPriority.SystemIdle);
        }

        private async Task ExecuteWidgetsAsync(CancellationToken cancellationToken)
        {
            //TODO: 表示・非表示状態を読み込んだりの諸々が必要
            if(Widgets.Count == 0) {
                //var pluginContextFactory = ApplicationDiContainer.Build<PluginContextFactory>();
                var widgetAddonContextFactory = ApplicationDiContainer.Build<WidgetAddonContextFactory>();
                var mainDatabaseBarrier = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
                var mainDatabaseDelayWriter = ApplicationDiContainer.Build<IMainDatabaseDelayWriter>();
                var databaseStatementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();
                var cultureService = ApplicationDiContainer.Build<ICultureService>();
                var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
                var dispatcherWrapper = ApplicationDiContainer.Build<IDispatcherWrapper>();

                foreach(var widget in PluginContainer.Addon.GetWidgets()) {
                    var element = new WidgetElement(widget, widget.Addon, widgetAddonContextFactory, mainDatabaseBarrier, mainDatabaseDelayWriter, databaseStatementLoader, cultureService, WindowManager, NotifyManager, environmentParameters, dispatcherWrapper, LoggerFactory);
                    await element.InitializeAsync(cancellationToken);
                    Widgets.Add(element);
                }
            }

            var showWidgets = new List<WidgetElement>(Widgets.Count);
            using(var context = ApplicationDiContainer.Build<IMainDatabaseBarrier>().WaitRead()) {
                var pluginWidgetSettingsEntityDao = ApplicationDiContainer.Build<PluginWidgetSettingsEntityDao>(context, context.Implementation);
                foreach(var element in Widgets) {
                    if(pluginWidgetSettingsEntityDao.SelectExistsPluginWidgetSetting(element.PluginId)) {
                        var setting = pluginWidgetSettingsEntityDao.SelectPluginWidgetSetting(element.PluginId);
                        if(setting.IsVisible) {
                            showWidgets.Add(element);
                        }
                    }
                }
            }

            // ViewModel渡す設計は💩で、しかもダミーってのがまた💩
            foreach(var element in showWidgets) {
                var viewModel = ApplicationDiContainer.Build<TemporaryWidgetViewModel>(element);
                element.ShowView(viewModel);
            }
        }

        private void SaveWidgets()
        {
            foreach(var widget in Widgets.Where(i => i.ViewCreated)) {
                widget.SaveStatus(true);
            }
        }

        private void CloseWidgets()
        {
            foreach(var widget in Widgets.Where(i => i.ViewCreated)) {
                widget.HideView();
            }
        }

        private void CloseLauncherItemExtensions()
        {
            foreach(var launcherItemExtension in LauncherItemExtensions.Where(i => i.HasView)) {
                launcherItemExtension.CloseView();
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("がんばる！");
#if DEBUG
            await DebugExecuteBeforeAsync();
#endif
            InitializeSystem();
            InitializeHook();
            InitializeScheduler();

            StartPlatform();

            await ExecuteElementsAsync(cancellationToken);

#if DEBUG
            await DebugExecuteAfterAsync();
#endif
        }

        private void CloseViewsCore(WindowKind windowKind)
        {
            var windowItems = WindowManager.GetWindowItems(windowKind).ToList();
            foreach(var windowItem in windowItems) {
                try {
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
                } catch(System.ComponentModel.Win32Exception ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }
        }

        private void CloseLauncherToolbarViews() => CloseViewsCore(WindowKind.LauncherToolbar);

        private void CloseNoteViews() => CloseViewsCore(WindowKind.Note);

        private void CloseLauncherCustomizeViews() => CloseViewsCore(WindowKind.LauncherCustomize);

        private void CloseExtendsExecuteViews() => CloseViewsCore(WindowKind.ExtendsExecute);
        void CloseStandardInputOutputViews() => CloseViewsCore(WindowKind.StandardInputOutput);

        private void CloseViews(bool saveWidgets)
        {
            CloseStandardInputOutputViews();
            CloseLauncherCustomizeViews();
            CloseExtendsExecuteViews();
            CloseLauncherToolbarViews();
            CloseNoteViews();

            if(saveWidgets) {
                SaveWidgets();
            }
            CloseWidgets();
            CloseLauncherItemExtensions();
        }

        private void DisposeElementsCore<TElement>(ICollection<TElement> elements)
            where TElement : ElementBase
        {
            foreach(var element in elements) {
                element.Dispose();
            }
            elements.Clear();
        }

        private void DisposeLauncherToolbarElements() => DisposeElementsCore(LauncherToolbarElements);
        private void DisposeLauncherGroupElements() => DisposeElementsCore(LauncherGroupElements);
        private void DisposeNoteElements() => DisposeElementsCore(NoteElements);

        private void DisposeElements()
        {
            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private void DisposeWebView()
        {
            //CefSharp.Cef.Shutdown();
        }

        /// <summary>
        /// 最新バージョンプラグインを構築。
        /// </summary>
        private void PrepareLatestPlugins()
        {
            var temporaryBarrier = ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>();
            IList<PluginInstallData> installDataItems;
            using(var context = temporaryBarrier.WaitRead()) {
                var installPluginsEntityDao = ApplicationDiContainer.Build<InstallPluginsEntityDao>(context, context.Implementation);
                installDataItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            if(!installDataItems.Any()) {
                return;
            }

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var pluginMap = PluginContainer.Plugins.ToDictionary(i => i.PluginInformation.PluginIdentifiers.PluginId, i => i);
            var directoryMover = ApplicationDiContainer.Build<DirectoryMover>();
            foreach(var installDataItem in installDataItems) {
                if(installDataItem.PluginInstallMode == PluginInstallMode.New) {
                    // 新規の場合プラグインIDからほわーっとディレクトリを準備
                    var destDirPath = Path.Combine(environmentParameters.MachinePluginInstallDirectory.FullName, PluginUtility.ConvertDirectoryName(installDataItem.PluginId));
                    var srcDir = new DirectoryInfo(installDataItem.PluginDirectoryPath);
                    var destDir = new DirectoryInfo(destDirPath);
                    Logger.LogInformation("インストール対象: 新規プラグイン: {0}, {1} -> {2}", installDataItem.PluginId, srcDir.FullName, destDir.FullName);
                    directoryMover.Move(srcDir, destDir);
                } else {
                    Debug.Assert(installDataItem.PluginInstallMode == PluginInstallMode.Update);
                    // 更新の場合、元プラグインのディレクトリ名をあれこれ調整してほわー
                    if(!pluginMap.TryGetValue(installDataItem.PluginId, out var plugin)) {
                        Logger.LogWarning("更新プラグインインストール処理の無視: {0}", installDataItem.PluginId);
                        continue;
                    }

                    var pluginDirPath = Path.GetDirectoryName(plugin.GetType().Assembly.Location)!;
                    var srcDir = new DirectoryInfo(installDataItem.PluginDirectoryPath);
                    var destDirPath = Path.Combine(environmentParameters.MachinePluginInstallDirectory.FullName, PluginUtility.ConvertDirectoryName(installDataItem.PluginId));
                    var destDir = new DirectoryInfo(destDirPath);
                    Logger.LogInformation("インストール対象: 更新プラグイン: {0}, {1} -> {2}", installDataItem.PluginId, srcDir.FullName, destDir.FullName);
                    directoryMover.Move(srcDir, destDir);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreApplicationUpdate">アプリケーションアップデートを無視するか。</param>
        public void Exit(bool ignoreApplicationUpdate)
        {
            Logger.LogInformation("おわる！");

            if(BackgroundAddon != null) {
                var backgroundAddonProxyRunShutdownContext = new BackgroundAddonProxyRunShutdownContext();
                BackgroundAddon.RunShutdown(backgroundAddonProxyRunShutdownContext);
            }

            PrepareLatestPlugins();

            UnloadPlugins();

            BackupSettingsDefault(ApplicationDiContainer);

            if(!ignoreApplicationUpdate && ApplicationUpdateInfo.IsReady) {
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

            StopScheduler();

            FinalizeSystem();

            CloseViews(true);
            DisposeElements();
            DisposeWebView();

            Dispose();

            Logger.LogInformation("ばいばい");

            NLog.LogManager.Shutdown();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// TODO: <see cref="ApplicationUpdateScriptFactory.CreateUpdateExecutePathParameter"/> と重複
        /// </summary>
        public void Reboot()
        {
            Logger.LogInformation("再起動開始");

            var environmentParameters = ApplicationDiContainer.Build<EnvironmentParameters>();
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);

            var powerShellArguments = new PowerShellArguments();
            var psResult = powerShellArguments.GetPowerShellFromCommandName(environmentExecuteFile);
            if(!psResult.Success) {
                Logger.LogError("PowerShell が見つかんないのでもぅﾏﾁﾞ無理");
                return;
            }
            var ps = psResult.SuccessValue!;

            var psCommands = powerShellArguments.CreateParameters(true, new[] {
                KeyValuePair.Create("-File", environmentParameters.EtcRebootScriptFile.FullName),
                KeyValuePair.Create("-LogPath", environmentParameters.TemporaryRebootLogFile.FullName),
                KeyValuePair.Create("-ProcessId", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture)),
                KeyValuePair.Create("-WaitSeconds", TimeSpan.FromSeconds(10).TotalMilliseconds.ToString(CultureInfo.InvariantCulture)),
                KeyValuePair.Create("-ExecuteCommand", environmentParameters.RootApplication.FullName)
            });
            psCommands.AddRange(powerShellArguments.ConvertOptions());

            var psCommand = string.Join(" ", psCommands);

            Logger.LogInformation("reboot path: {0}", ps);
            Logger.LogInformation("reboot args: {0}", psCommand);

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

        public async Task ShowCommandViewAsync(CancellationToken cancellationToken)
        {
            if(CommandElement == null) {
                CommandElement = ApplicationDiContainer.Build<CommandElement>();
                await CommandElement.InitializeAsync(cancellationToken);

                var commandFinders = new ICommandFinder[] {
                    ApplicationDiContainer.Build<LauncherItemCommandFinder>(),
                    ApplicationDiContainer.Build<ApplicationCommandFinder>(CreateApplicationCommandParameters()),
                    PluginContainer.Addon.GetCommandFinder(),
                };

                foreach(var commandFinder in commandFinders) {
                    CommandElement.AddCommandFinder(commandFinder);
                }
                await CommandElement.RefreshAsync(cancellationToken);
            }

            CommandElement.StartView();
        }

        public async Task ShowFeedbackViewAsync(CancellationToken cancellationToken)
        {
            var items = WindowManager.GetWindowItems(WindowKind.Feedback).ToList();
            if(items.Count != 0) {
                foreach(var item in items) {
                    WindowManager.Flash(item);
                }
                return;
            }

            var feedbackElement = ApplicationDiContainer.Build<FeedbackElement>();
            await feedbackElement.InitializeAsync(cancellationToken);
            var windowItem = OrderManager.CreateFeedbackWindow(feedbackElement);
            WindowManager.Register(windowItem);
            windowItem.Window.Show();
        }

        /// <summary>
        /// 設定データをバックアップ。
        /// </summary>
        /// <param name="sourceDirectory">バックアップ対象ディレクトリ。</param>
        /// <param name="targetDirectory">アーカイブファイル配置ディレクトリ。</param>
        /// <param name="backupFileWithoutExtensionName">バックアップファイルの拡張子抜きファイル名。</param>
        /// <param name="enabledCount">ローテート処理で残すファイル数。</param>
        /// <param name="userBackupDirectoryPath">ユーザー設定バックアップディレクトリパス。</param>
        private void BackupSettings(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, string backupFileWithoutExtensionName, int enabledCount, string userBackupDirectoryPath)
        {
            try {
                // アプリケーション側バックアップ
                var settingBackupper = new SettingBackupper(LoggerFactory);
                settingBackupper.BackupUserSetting(sourceDirectory, targetDirectory, backupFileWithoutExtensionName, enabledCount);

                // ユーザー設定側バックアップ
                var expandedUserBackupDirectoryPath = Environment.ExpandEnvironmentVariables(userBackupDirectoryPath ?? string.Empty);
                if(!string.IsNullOrWhiteSpace(expandedUserBackupDirectoryPath)) {
                    var dir = new DirectoryInfo(expandedUserBackupDirectoryPath);
                    settingBackupper.BackupUserSettingToCustomDirectory(sourceDirectory, dir);
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "バックアップ失敗");
            }
        }

        private void BackupSettingsDefault(IDiContainer diContainer)
        {
            var environmentParameters = diContainer.Get<EnvironmentParameters>();

            // バックアップ処理開始
            string userBackupDirectoryPath;
            using(var context = diContainer.Get<IMainDatabaseBarrier>().WaitRead()) {
                var appGeneralSettingEntityDao = diContainer.Build<AppGeneralSettingEntityDao>(context, context.Implementation);
                userBackupDirectoryPath = appGeneralSettingEntityDao.SelectUserBackupDirectoryPath();
            }
            var versionConverter = new VersionConverter();
            BackupSettings(
                environmentParameters.UserSettingDirectory,
                environmentParameters.UserBackupDirectory,
                DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss", CultureInfo.InvariantCulture) + "_ver." + versionConverter.ConvertDisplayVersion(BuildStatus.Version, "-"),
                environmentParameters.ApplicationConfiguration.Backup.SettingCount,
                userBackupDirectoryPath
            );
        }

        private void ClearScreenViewElements()
        {
            CloseLauncherToolbarViews();
            CloseNoteViews();
            SaveWidgets();
            CloseWidgets();
            //CloseLauncherItemExtensions(); // とりあえずこれは消さない

            DisposeLauncherToolbarElements();
            DisposeLauncherGroupElements();
            DisposeNoteElements();
        }

        private async Task ResetScreenViewElementsAsync(CancellationToken cancellationToken)
        {
            ClearScreenViewElements();

            ResetNotifyArea();

            await ExecuteElementsAsync(cancellationToken);
        }

        private void ResetNotifyArea()
        {
            var notifyIcon = (Hardcodet.Wpf.TaskbarNotification.TaskbarIcon)Application.Current.FindResource("root");
            var viewModel = notifyIcon.DataContext;
            Logger.LogDebug("通知領域再設定開始");
            notifyIcon.DataContext = null;
            ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
                notifyIcon.DataContext = viewModel;
                Logger.LogDebug("通知領域再設定終了");
            }, DispatcherPriority.SystemIdle);

        }

        private void DelayResetScreenViewElements()
        {
            void DelayExecuteElements()
            {
                DelayScreenElementReset.Callback(() => {
                    ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(async () => await ResetScreenViewElementsAsync(CancellationToken.None), DispatcherPriority.SystemIdle);
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

        internal FileInfo OutputRawCrashReport(Exception exception)
        {
            var environmentParameters = ApplicationDiContainer.Get<EnvironmentParameters>();
            var versionConverter = new VersionConverter();
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HHmmss_fff'Z'", CultureInfo.InvariantCulture);
            var fileName = versionConverter.ConvertFileName(timestamp, BuildStatus.Version, BuildStatus.Revision, "dmp");
            var filePath = Path.Combine(environmentParameters.TemporaryCrashReportDirectory.FullName, fileName);

            static Dictionary<string, string?> CreateInfoMap(IEnumerable<PlatformInformationItem> items) => items.ToDictionary(k => k.Key, v => Convert.ToString(v.Value, CultureInfo.InvariantCulture));
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
                Build = BuildStatus.BuildType.ToString(),
                Exception = exception.ToString(),
            };

            ExceptionWrapper(() => {
                rawData.UserId = ApplicationDiContainer.Get<IMainDatabaseBarrier>().ReadData(c => {
                    var appExecuteSettingEntityDao = ApplicationDiContainer.Build<AppExecuteSettingEntityDao>(c, c.Implementation);
                    var userIdManager = new UserIdManager(LoggerFactory);
                    return userIdManager.SafeGetOrCreateUserId(appExecuteSettingEntityDao);
                });
            });

            static string TrimFunc(string s) => s.Substring(3);

            var info = new ApplicationInformationCollector(environmentParameters);
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetApplication))] = CreateInfoMap(info.GetApplication()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetEnvironmentParameter))] = CreateInfoMap(info.GetEnvironmentParameter()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetCPU))] = CreateInfoMap(info.GetCPU()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetOS))] = CreateInfoMap(info.GetOS()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetRuntimeInformation))] = CreateInfoMap(info.GetRuntimeInformation()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetEnvironment))] = CreateInfoMap(info.GetEnvironment()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetEnvironmentVariables))] = CreateInfoMap(info.GetEnvironmentVariables()));
            ExceptionWrapper(() => rawData.InformationMap[TrimFunc(nameof(info.GetScreen))] = CreateInfoMap(info.GetScreen()));

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
                var diffStream = new MemoryReleaseStream();
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
                "--post-uri", CommandLine.Escape(environmentParameters.ApplicationConfiguration.Api.CrashReportUri.OriginalString),
                "--src-uri", CommandLine.Escape(environmentParameters.ApplicationConfiguration.Api.CrashReportSourceUri.OriginalString),
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
            var commandPath = ApplicationBoot.CommandPath;
            Logger.LogInformation("path: {0}", commandPath);
            Logger.LogInformation("args: {0}", arg);
            systemExecutor.ExecuteFile(commandPath, arg);
        }

        internal void CompleteStartup()
        {
#if DEBUG
            if(!IsDevDebug) {
#endif
                StartHook();
                StartScheduler();
                StartBackground();
#if DEBUG
            }
#endif
            CheckNewVersionsAsync(true, CancellationToken.None).ConfigureAwait(false);
#if DEBUG
            DebugBoot();
            DebugCompleteStartup();
#endif
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called")]
        private void GarbageCollection(bool full)
        {
            var old = GC.GetTotalMemory(false);
            var startTimestamp = DateTime.UtcNow;

            if(full) {
                var currentMode = System.Runtime.GCSettings.LargeObjectHeapCompactionMode;
                Logger.LogTrace("LargeObjectHeapCompactionMode: {0}", currentMode);
                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                try {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                } finally {
                    System.Runtime.GCSettings.LargeObjectHeapCompactionMode = currentMode;
                }
            } else {
                GC.Collect(0);
                GC.Collect(1);
            }
            var endTimestamp = DateTime.UtcNow;
            var now = GC.GetTotalMemory(false);
            var sizeConverter = ApplicationDiContainer.Build<Library.Base.SizeConverter>();
            Logger.LogInformation(
                "GC(FULL:{0}): {1}({2}) -> {3}({4}), 差分: {5}({6}), 所要時間: {7}",
                full,
                sizeConverter.ConvertHumanReadableByte(old), old,
                sizeConverter.ConvertHumanReadableByte(now), now,
                sizeConverter.ConvertHumanReadableByte(old - now), old - now,
                endTimestamp - startTimestamp
            );
        }

        /// <summary>
        /// プラグイン(DLL周り)に変更があったか。
        /// </summary>
        /// <returns>真: 変更があった。</returns>
        private bool CheckPluginChanges()
        {
            var main = ApplicationDiContainer.Build<IMainDatabaseBarrier>();
            var temp = ApplicationDiContainer.Build<ITemporaryDatabaseBarrier>();

            var existsUninstall = false;
            using(var context = main.WaitRead()) {
                var pluginsEntityDao = ApplicationDiContainer.Build<PluginsEntityDao>(context, context.Implementation);
                existsUninstall = pluginsEntityDao.SelectExistsPluginByState(PluginState.Uninstall);
            }

            var existsInstall = false;
            using(var context = temp.WaitRead()) {
                var installPluginsEntityDao = ApplicationDiContainer.Build<InstallPluginsEntityDao>(context, context.Implementation);
                existsInstall = installPluginsEntityDao.SelectExistsInstallPlugin();
            }

            return existsUninstall || existsInstall;
        }

        #endregion

        #region IOrderManager

        public void AddRedoItem(RedoExecutor redoExecutor) => OrderManager.AddRedoItem(redoExecutor);

        public void StartUpdate(UpdateTarget target, UpdateProcess process)
        {
            Debug.Assert(target == UpdateTarget.Application);

            switch(process) {
                case UpdateProcess.Download:
                    CheckApplicationNewVersionAsync(UpdateCheckKind.Update, CancellationToken.None).ConfigureAwait(false);
                    break;

                case UpdateProcess.Update:
                    Exit(false);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public Task<LauncherGroupElement> CreateLauncherGroupElementAsync(LauncherGroupId launcherGroupId, CancellationToken cancellationToken)
        {
            return OrderManager.CreateLauncherGroupElementAsync(launcherGroupId, cancellationToken);
        }
        public Task<LauncherToolbarElement> CreateLauncherToolbarElementAsync(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, CancellationToken cancellationToken)
        {
            return OrderManager.CreateLauncherToolbarElementAsync(dockScreen, launcherGroups, cancellationToken);
        }

        public LauncherItemElement GetOrCreateLauncherItemElement(LauncherItemId launcherItemId)
        {
            return OrderManager.GetOrCreateLauncherItemElement(launcherItemId);
        }
        public void RefreshLauncherItemElement(LauncherItemId launcherItemId) => OrderManager.RefreshLauncherItemElement(launcherItemId);

        public Task<LauncherItemCustomizeContainerElement> CreateCustomizeLauncherItemContainerElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken)
        {
            return OrderManager.CreateCustomizeLauncherItemContainerElementAsync(launcherItemId, screen, cancellationToken);
        }

        public Task<ExtendsExecuteElement> CreateExtendsExecuteElementAsync(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen, CancellationToken cancellationToken)
        {
            return OrderManager.CreateExtendsExecuteElementAsync(captionName, launcherFileData, launcherEnvironmentVariables, screen, cancellationToken);
        }

        public Task<LauncherExtendsExecuteElement> CreateLauncherExtendsExecuteElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken)
        {
            return OrderManager.CreateLauncherExtendsExecuteElementAsync(launcherItemId, screen, cancellationToken);
        }


        public Task<NoteElement> CreateNoteElementAsync(NoteId noteId, IScreen? screen, NoteStartupPosition startupPosition, CancellationToken cancellationToken)
        {
            return OrderManager.CreateNoteElementAsync(noteId, screen, startupPosition, cancellationToken);
        }
        public bool RemoveNoteElement(NoteId noteId)
        {
            var targetElement = NoteElements.FirstOrDefault(i => i.NoteId == noteId);
            if(targetElement == null) {
                Logger.LogWarning("ノート削除: 対象不明 {0}", noteId);
                return false;
            }

            var appBarrierPack = ApplicationDiContainer.Build<IDatabaseBarrierPack>();
            var statementLoader = ApplicationDiContainer.Build<IDatabaseStatementLoader>();
            using(var appContext = appBarrierPack.WaitWrite()) {
                var noteEntityEraser = new NoteEntityEraser(noteId, appContext, statementLoader, LoggerFactory);
                try {
                    noteEntityEraser.Execute();
                    NoteElements.Remove(targetElement);
                    targetElement.Dispose();
                    appBarrierPack.Save();
                    return true;
                } catch(Exception ex) {
                    Logger.LogError(ex, "ノート削除に失敗: {0} {1}", ex.Message, noteId);
                }
            }

            return false;
        }

        public Task<NoteContentElement> CreateNoteContentElementAsync(NoteId noteId, NoteContentKind contentKind, CancellationToken cancellationToken)
        {
            return OrderManager.CreateNoteContentElementAsync(noteId, contentKind, cancellationToken);
        }

        public Task<SavingFontElement> CreateFontElementAsync(DefaultFontKind defaultFontKind, FontId fontId, ParentUpdater parentUpdater, CancellationToken cancellationToken)
        {
            return OrderManager.CreateFontElementAsync(defaultFontKind, fontId, parentUpdater, cancellationToken);
        }

        /// <inheritdoc cref="IOrderManager.CreateStandardInputOutputElementAsync(string, Process, IScreen)"/>
        public async Task<StandardInputOutputElement> CreateStandardInputOutputElementAsync(string caption, Process process, IScreen screen, CancellationToken cancellationToken)
        {
            var element = await OrderManager.CreateStandardInputOutputElementAsync(caption, process, screen, cancellationToken);
            StandardInputOutputs.Add(element);
            return element;
        }

        /// <inheritdoc cref="IOrderManager.CreateLauncherItemExtensionElementAsync(IPluginInformation, LauncherItemId)"/>
        public async Task<LauncherItemExtensionElement> CreateLauncherItemExtensionElementAsync(IPluginInformation pluginInformation, LauncherItemId launcherItemId, CancellationToken cancellationToken)
        {
            var element = await OrderManager.CreateLauncherItemExtensionElementAsync(pluginInformation, launcherItemId, cancellationToken);
            LauncherItemExtensions.Add(element);
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

                    DelayScreenElementReset.Dispose();

                    CloseViews(false);
                    DisposeElements();
                    DisposeWebView();

                    CronScheduler.Dispose();

                    //MessageWindowDispatcherWapper?.BeginAsync(() => {
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void NotifyManagerImpl_LauncherGroupItemRegistered(object? sender, LauncherGroupItemRegisteredEventArgs e)
        {
            var launcherGroupElement = await OrderManager.CreateLauncherGroupElementAsync(e.LauncherGroupId, CancellationToken.None);
            LauncherGroupElements.Add(launcherGroupElement);
        }

        private void PlatformThemeLoader_Changed(object? sender, EventArgs e)
        {
            ApplicationDiContainer.Get<IDispatcherWrapper>().BeginAsync(() => {
                SetDynamicPlatformTheme();
            }, DispatcherPriority.ApplicationIdle);
        }
    }
}
