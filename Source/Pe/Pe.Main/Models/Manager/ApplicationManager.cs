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

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public partial class ApplicationManager : DisposerBase, IOrderManager
    {
        public ApplicationManager(ApplicationInitializer initializer)
        {
#pragma warning disable CS8601 // Null 参照割り当ての可能性があります。
            LoggerFactory = initializer.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            IsFirstStartup = initializer.IsFirstStartup;
            ApplicationDiContainer = initializer.DiContainer;
            WindowManager = initializer.WindowManager;
            OrderManager = ApplicationDiContainer!.Make<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManager = initializer.NotifyManager;
            StatusManager = initializer.StatusManager;
            ClipboardManager = initializer.ClipboardManager;
#pragma warning restore CS8601 // Null 参照割り当ての可能性があります。
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

        HwndSource? MessageWindowHandleSource { get; set; }

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

            MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                Width = 0,
                Height = 0,
                WindowStyle = (int)WindowStyle.None,
                //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                HwndSourceHook = MessageWindowProc,
            });

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
                var element = CreateNoteElement(noteId, default(Screen), NotePosition.Setting);
                result.Add(element);
            }

            return result;
        }

        public ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> GetLauncherNotifyCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>(LauncherToolbarElements, LoggerFactory) {
                ToViewModel = m => ApplicationDiContainer.Make<LauncherToolbarNotifyAreaViewModel>(new[] { m })
            };
            return collection;
        }

        public ModelViewModelObservableCollectionManagerBase<NoteElement, NoteNotifyAreaViewModel> GetNoteCollection()
        {
            var collection = new ActionModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel>(NoteElements, LoggerFactory) {
                ToViewModel = m => ApplicationDiContainer.Make<NoteNotifyAreaViewModel>(new[] { m })
            };
            return collection;
        }

        public NoteElement CreateNote(Screen dockScreen)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.LogInformation("new note id: {0}, {1}", noteId, ObjectDumper.GetDumpString(dockScreen));
            var noteElement = CreateNoteElement(noteId, dockScreen, NotePosition.CenterScreen);

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

        public void Execute()
        {
            Logger.LogInformation("がんばる！");
#if DEBUG
            DebugExecuteBefore();
#endif

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

            CloseViews();

            Dispose();

            Application.Current.Shutdown();
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

        public LauncherItemCustomizeElement CreateCustomizeLauncherItemElement(Guid launcherItemId, LauncherIconElement iconElement, Screen screen)
        {
            return OrderManager.CreateCustomizeLauncherItemElement(launcherItemId, iconElement, screen);
        }

        public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen)
        {
            return OrderManager.CreateExtendsExecuteElement(captionName, launcherFileData, launcherEnvironmentVariables, screen);
        }

        public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, Screen screen)
        {
            return OrderManager.CreateLauncherExtendsExecuteElement(launcherItemId, screen);
        }


        public NoteElement CreateNoteElement(Guid noteId, Screen? screen, NotePosition notePosition)
        {
            return OrderManager.CreateNoteElement(noteId, screen, notePosition);
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

        public FontElement CreateFontElement(Guid fontId, ParentUpdater parentUpdater)
        {
            return OrderManager.CreateFontElement(fontId, parentUpdater);
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

        public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeElement element)
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


        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    CloseViews();
                    DisposeElements();

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
