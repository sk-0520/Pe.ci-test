using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.Model.Note;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteElement : ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        Screen _dockScreen;

        #endregion

        public NoteElement(Guid noteId, Screen dockScreen, IOrderManager orderManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
            OrderManager = orderManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            NoteTheme = noteTheme;
        }

        #region property

        public Guid NoteId { get; }

        /// <summary>
        /// DB から取得して設定したりそれでも保存しなかったりするまさに変数。
        /// </summary>
        public Screen DockScreen
        {
            get => this._dockScreen;
            private set => SetProperty(ref this._dockScreen, value);
        }
        IOrderManager OrderManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        INoteTheme NoteTheme { get; }

        bool ViewCreated { get; set; }

        public bool IsTopmost
        {
            get => this._isTopmost;
            private set => SetProperty(ref this._isTopmost, value);
        }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }
        #endregion

        #region function

        NoteData GetNoteData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NotesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectNote(NoteId);
            }
        }

        NoteData CreateNoteData()
        {
            DockScreen = DockScreen ?? Screen.PrimaryScreen;

            var noteData = new NoteData() {
                NoteId = NoteId,
                FontId = Guid.Empty,
                Title = DateTime.Now.ToString(), //TODO: タイトル
                BackgroundColor = Colors.Yellow,
                ForegdoundColor = Colors.Black,
                ScreenName = DockScreen.DeviceName,
                IsCompact = false,
                IsLocked = false,
                IsTopmost = false,
                IsVisible = true,
                LayoutKind = NoteLayoutKind.Absolute,
                TextWrap = true,
                ContentKind = NoteContentKind.Plain,
            };

            /*
            var noteLayout = new NoteLayoutData() {
                NoteId = noteData.NoteId,
                LayoutKind = noteData.LayoutKind,
            };
            if(noteLayout.LayoutKind == NoteLayoutKind.Absolute) {
                noteLayout.Width = 200;
                noteLayout.Height = 160;
                noteLayout.X = (DockScreen.DeviceBounds.Width / 2) - (noteLayout.Width / 2);
                noteLayout.Y = (DockScreen.DeviceBounds.Height / 2) - (noteLayout.Height / 2);
            } else {
                Debug.Assert(noteLayout.LayoutKind == NoteLayoutKind.Relative);
                throw new NotImplementedException();
            }

            var noteContent = new NoteContentData() {
                NoteId = noteData.NoteId,
                ContentKind = noteData.ContentKind,
                Content = string.Empty,
            };
            */

            using(var commander = MainDatabaseBarrier.WaitWrite()) {

                var notesEntityDao = new NotesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                notesEntityDao.InsertNewNote(noteData, DatabaseCommonStatus.CreateCurrentAccount());

                /*
                var notesLayoutDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                notesLayoutDao.InsertNewLayout(noteLayout, DatabaseCommonStatus.CreateCurrentAccount());

                var noteContentDao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                noteContentDao.InsertNewContent(noteContent, DatabaseCommonStatus.CreateCurrentAccount());
                */

                var screenEntityDao = new ScreensEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                if(!screenEntityDao.SelectExistsScreen(DockScreen.DeviceName)) {
                    screenEntityDao.InsertScreen(DockScreen, DatabaseCommonStatus.CreateCurrentAccount());
                }

                commander.Commit();
            }

            return noteData;
        }

        void LoadNote()
        {
            //あればそれを読み込んでなければ作る
            var noteData = GetNoteData();
            if(noteData == null) {
                noteData = CreateNoteData();
            }

            IsVisible = true;
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadNote();
        }

        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return IsVisible;
            }
        }

        public void StartView()
        {
            var windowItem = OrderManager.CreateNoteWindow(this);
            ViewCreated = true;
        }

        #endregion

        #region IWindowCloseReceiver

        public bool ReceiveViewClosing()
        {
            return true;
        }

        public void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }
}
