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

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public partial class ApplicationManager : DisposerBase, IOrderManager
    {
        public ApplicationManager(ApplicationInitializer initializer)
        {
            LoggerFactory = initializer.LoggerFactory ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.LoggerFactory));
            Logger = LoggerFactory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;
            ApplicationDiContainer = initializer.DiContainer ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.DiContainer));
            WindowManager = initializer.WindowManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.WindowManager));
            OrderManager = ApplicationDiContainer!.Make<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManager = initializer.NotifyManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.NotifyManager));
            StatusManager = initializer.StatusManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.StatusManager));
            ClipboardManager = initializer.ClipboardManager ?? throw new ArgumentNullException(nameof(initializer) + "." + nameof(initializer.ClipboardManager));

            KeyboradHooker = new KeyboradHooker(LoggerFactory);
            MouseHooker = new MouseHooker(LoggerFactory);
            KeyActionChecker = new KeyActionChecker(LoggerFactory);
            KeyActionAssistant = new KeyActionAssistant(LoggerFactory);

        }

        #region property

        ILoggerFactory LoggerFactory { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        bool IsFirstStartup { get; }
        ILogger Logger { get; set; }

        WindowManager WindowManager { get; set; }
        OrderManagerImpl OrderManager { get; set; }
        NotifyManager NotifyManager { get; set; }
        StatusManager StatusManager { get; set; }
        ClipboardManager ClipboardManager { get; set; }

        ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();
        ObservableCollection<StandardInputOutputElement> StandardInputOutputs { get; } = new ObservableCollection<StandardInputOutputElement>();
        SettingContainerElement? SettingElement { get; set; }
        HwndSource? MessageWindowHandleSource { get; set; }
        //IDispatcherWapper? MessageWindowDispatcherWapper { get; set; }

        KeyboradHooker KeyboradHooker { get; }
        MouseHooker MouseHooker { get; }
        KeyActionChecker KeyActionChecker { get; }
        KeyActionAssistant KeyActionAssistant { get; }

        #endregion

        #region function

        void ShowStartupView()
        {
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
        }

        void RegisterManagers()
        {
            Debug.Assert(ApplicationDiContainer != null);

            ApplicationDiContainer.Register<IWindowManager, WindowManager>(WindowManager);
            ApplicationDiContainer.Register<IOrderManager, IOrderManager>(this);
            ApplicationDiContainer.Register<INotifyManager, NotifyManager>(NotifyManager);
            ApplicationDiContainer.Register<IStatusManager, StatusManager>(StatusManager);
            ApplicationDiContainer.Register<IClipboardManager, ClipboardManager>(ClipboardManager);
        }

        public bool Startup(App app, StartupEventArgs e)
        {
            //var initializer = new ApplicationInitializer();
            //if(!initializer.Initialize(e.Args)) {
            //    return false;
            //}
            MakeMessageWindow();
            RegisterManagers();

            Logger = LoggerFactory.CreateLogger(GetType());
            Logger.LogDebug("初期化完了");

            if(IsFirstStartup) {
                // 初期登録の画面を表示
                ShowStartupView();
            }

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
                var element = CreateNoteElement(noteId, default(Screen), NoteStartupPosition.Setting);
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

        public NoteElement CreateNote(Screen dockScreen)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.LogInformation("new note id: {0}, {1}", noteId, ObjectDumper.GetDumpString(dockScreen));
            var noteElement = CreateNoteElement(noteId, dockScreen, NoteStartupPosition.CenterScreen);

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
                note.SwitchCompactCommand.ExecuteIfCanExecute(null);
            }
        }

        public void MoveZOrderAllNotes(bool isTop)
        {
            var noteItems = WindowManager.GetWindowItems(WindowKind.Note)
                .Where(i => !i.Window.Topmost)
                .Where(i => i.Window.IsVisible)
                .ToList()
            ;
            foreach(var noteItem in noteItems) {
                var hWnd = HandleUtility.GetWindowHandle(noteItem.Window);
                if(isTop) {
                    WindowsUtility.ShowNoActiveForeground(hWnd);
                } else {
                    WindowsUtility.MoveZoderBttom(hWnd);
                }
            }
        }

        void ExecuteElements()
        {
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
        }

        public void Execute()
        {
            Logger.LogInformation("がんばる！");
#if DEBUG
            DebugExecuteBefore();
#endif
            InitializeSystem();
            InitializeHook();

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

        public void Exit()
        {
            Logger.LogInformation("おわる！");

            StopHook();
            DisposeHook();

            UninitializeSystem();

            CloseViews();
            DisposeElements();

            Dispose();

            Logger.LogInformation("ばいばい");
            Application.Current.Shutdown();
        }

        /// <summary>
        /// すべてここで完結する神の所業。
        /// </summary>
        public void ShowSettingView()
        {
            if(SettingElement != null) {
                Logger.LogWarning("せっていちゅう");
                return;
            }

            StopHook();
            UninitializeSystem();

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
            var settingDirectory = environmentParameters.SettingTemporaryDirectory;
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

            SettingElement = container.Build<SettingContainerElement>();
            SettingElement.Closed += Element_Closed;
            SettingElement.Initialize();
            SettingElement.StartView();

            void Element_Closed(object? sender, System.EventArgs e)
            {
                Debug.Assert(SettingElement == sender);
                Debug.Assert(SettingElement != null);

                SettingElement.Closed -= Element_Closed;

                if(SettingElement.IsSubmit) {
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

                    Logger.LogInformation("設定適用のため各要素生成");
                    RebuildHook();
                    ExecuteElements();
                } else {
                    Logger.LogInformation("設定は保存されなかったため現在要素継続");
                }
                StartHook();
                InitializeSystem();

                Logger.LogDebug("遅延書き込み処理再開");
                foreach(var pair in lazyWriterItemMap) {
                    if(SettingElement.IsSubmit) {
                        // 確定処理の書き込みが天に召されるのでため込んでいた処理(ないはず)を消す
                        pair.Key.ClearStock();
                    }
                    pair.Value.Dispose();
                }

                if(changing.Success) {
                    changing.SuccessValue?.Dispose();
                }

                SettingElement.Dispose();
                SettingElement = null;
                container.UnregisterDatabase();
                container.Dispose();
            }
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

        private void ResetViewElements()
        {
            CloseViews();
            DisposeElements();

            ExecuteElements();
        }

        #endregion

        #region IOrderManager

        public LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId)
        {
            return OrderManager.CreateLauncherGroupElement(launcherGroupId);
        }
        public LauncherToolbarElement CreateLauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
        {
            return OrderManager.CreateLauncherToolbarElement(dockScreen, launcherGroups);
        }

        public LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId)
        {
            return OrderManager.GetOrCreateLauncherItemElement(launcherItemId);
        }

        public LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, Screen screen, LauncherIconElement iconElement)
        {
            return OrderManager.CreateCustomizeLauncherItemContainerElement(launcherItemId, screen, iconElement);
        }

        public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen)
        {
            return OrderManager.CreateExtendsExecuteElement(captionName, launcherFileData, launcherEnvironmentVariables, screen);
        }

        public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, Screen screen)
        {
            return OrderManager.CreateLauncherExtendsExecuteElement(launcherItemId, screen);
        }


        public NoteElement CreateNoteElement(Guid noteId, Screen? screen, NoteStartupPosition startupPosition)
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

        public StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process, Screen screen)
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
                    CloseViews();
                    DisposeElements();

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
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
