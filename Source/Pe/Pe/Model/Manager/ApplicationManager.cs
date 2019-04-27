using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Element;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.View.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.Manager;
using ContentTypeTextNet.Pe.Main.Model.Element.Font;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public partial class ApplicationManager : DisposerBase, IOrderManager, INotifyManager
    {
        public ApplicationManager()
        { }

        #region property

        ApplicationLogger ApplicationLogger { get; set; }
        ApplicationDiContainer ApplicationDiContainer { get; set; }

        ILogger Logger { get; set; }

        WindowManager WindowManager { get; set; }
        OrderManagerImpl OrderManager { get; set; }
        NotifyManagerImpl NotifyManager { get; set; }

        ObservableCollection<LauncherGroupElement> LauncherGroupElements { get; } = new ObservableCollection<LauncherGroupElement>();
        ObservableCollection<LauncherToolbarElement> LauncherToolbarElements { get; } = new ObservableCollection<LauncherToolbarElement>();
        ObservableCollection<NoteElement> NoteElements { get; } = new ObservableCollection<NoteElement>();

        HwndSource MessageWindowHandleSource { get; set; }

        #endregion

        #region function

        void ShowStartupView()
        {
            using(var diContainer = ApplicationDiContainer.CreateChildContainer()) {
                diContainer
                    .RegisterLogger(Logger)
                    .RegisterMvvm<Element.Startup.StartupElement, ViewModel.Startup.StartupViewModel, View.Startup.StartupWindow>()
                ;
                var startupModel = diContainer.New<Element.Startup.StartupElement>();
                var view = diContainer.Make<View.Startup.StartupWindow>();

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
            ApplicationDiContainer.Register<INotifyManager, INotifyManager>(this);

        }

        public bool Startup(App app, StartupEventArgs e)
        {
            var initializer = new ApplicationInitializer();
            if(!initializer.Initialize(e.Args)) {
                return false;
            }

            ApplicationLogger = initializer.Logger;
            ApplicationDiContainer = initializer.DiContainer;
            WindowManager = initializer.WindowManager;
            OrderManager = ApplicationDiContainer.Make<OrderManagerImpl>(); //initializer.OrderManager;
            NotifyManager = ApplicationDiContainer.Make<NotifyManagerImpl>();//initializer.NotifyManager;

            MessageWindowHandleSource = new HwndSource(new HwndSourceParameters(nameof(MessageWindowHandleSource)) {
                Width = 0,
                Height = 0,
                WindowStyle = (int)WindowStyle.None,
                //ParentWindow = WindowsUtility.ToIntPtr(HWND.HWND_MESSAGE),
                HwndSourceHook = MessageWindowProc,
            });

            RegisterManagers();

            Logger = ApplicationLogger.Factory.CreateTartget(GetType());
            Logger.Debug("初期化完了");

            if(initializer.IsFirstStartup) {
                // 初期登録の画面を表示
                ShowStartupView();
            }

            return true;
        }


        public ManagerViewModel CreateViewModel()
        {
            var viewModel = new ManagerViewModel(this, Logger.Factory);
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

        public ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> GetLauncherNotifyCollection() {
            var collection = new ActionModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel>(LauncherToolbarElements, Logger.Factory) {
                ToViewModel = m => ApplicationDiContainer.Make<LauncherToolbarNotifyAreaViewModel>(new[] { m })
            };
            return collection;
        }

        public NoteElement CreateNote(Screen dockScreen)
        {
            var idFactory = ApplicationDiContainer.Build<IIdFactory>();
            var noteId = idFactory.CreateNoteId();
            Logger.Information($"new note id: {noteId}", ObjectDumper.GetDumpString(dockScreen));
            var noteElement = CreateNoteElement(noteId, dockScreen, NotePosition.CenterScreen);

            return noteElement;
        }

        public void Execute()
        {
            Logger.Information("がんばる！");

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

        void CloseToolbars()
        {
            var toolbars = WindowManager.GetWindowItems(WindowKind.LauncherToolbar).ToList();
            foreach(var toolbar in toolbars) {
                toolbar.Window.Close();
            }
        }

        void CloseViews()
        {
            CloseToolbars();
        }

        void DisposeToolbarElements()
        {
            foreach(var toolbar in LauncherToolbarElements) {
                toolbar.Dispose();
            }
            LauncherToolbarElements.Clear();
        }

        void DisposeElements()
        {
            DisposeToolbarElements();
        }

        public void Exit()
        {
            Logger.Information("おわる！");

            CloseViews();
            DisposeElements();

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

        public NoteElement CreateNoteElement(Guid noteId, Screen screen, NotePosition notePosition)
        {
            return OrderManager.CreateNoteElement(noteId, screen, notePosition);
        }

        public FontElement CreateFontElement(Guid fontId, ParentUpdater parentUpdater)
        {
            return OrderManager.CreateFontElement(fontId, parentUpdater);
        }

        public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
        {
            var windowItem = OrderManager.CreateLauncherToolbarWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        public WindowItem CreateNoteWindow(NoteElement element)
        {
            var windowItem = OrderManager.CreateNoteWindow(element);

            WindowManager.Register(windowItem);

            return windowItem;
        }

        #endregion

        #region INotifyManager
        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    MessageWindowHandleSource?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
