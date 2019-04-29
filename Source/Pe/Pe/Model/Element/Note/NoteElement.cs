using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.CompatibleWindows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Element.Font;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteElement : ElementBase, IViewShowStarter, IViewCloseReceiver, IFlush
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        bool _isCompact;
        bool _isLocked;
        bool _textWrap;
        string _title;
        Screen _dockScreen;

        NoteLayoutKind _layoutKind;
        NoteContentKind _contentKind;

        Color _foregroundColor;
        Color _backegroundColor;

        NoteContentElement _contentElement;

        #endregion

        public NoteElement(Guid noteId, Screen dockScreen, NotePosition notePosition, IOrderManager orderManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
            Position = notePosition;
            OrderManager = orderManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            NoteTheme = noteTheme;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.NoteMainDatabaseLazyWriterWaitTime, this);
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
        public NotePosition Position { get; }
        IOrderManager OrderManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        INoteTheme NoteTheme { get; }
        public FontElement FontElement { get; private set; }

        DatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        bool ViewCreated { get; set; }

        public bool IsTopmost
        {
            get => this._isTopmost;
            private set => SetProperty(ref this._isTopmost, value);
        }
        public bool IsCompact
        {
            get => this._isCompact;
            private set => SetProperty(ref this._isCompact, value);
        }
        public bool IsLocked
        {
            get => this._isLocked;
            private set => SetProperty(ref this._isLocked, value);
        }
        public bool TextWrap
        {
            get => this._textWrap;
            private set => SetProperty(ref this._textWrap, value);
        }
        public string Title
        {
            get => this._title;
            private set => SetProperty(ref this._title, value);
        }

        public NoteLayoutKind LayoutKind
        {
            get => this._layoutKind;
            private set => SetProperty(ref this._layoutKind, value);
        }
        public NoteContentKind ContentKind
        {
            get => this._contentKind;
            private set => SetProperty(ref this._contentKind, value);
        }

        public Color ForegroundColor
        {
            get => this._foregroundColor;
            private set => SetProperty(ref this._foregroundColor, value);
        }
        public Color BackgroundColor
        {
            get => this._backegroundColor;
            private set => SetProperty(ref this._backegroundColor, value);
        }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }

        public NoteContentElement ContentElement
        {
            get => this._contentElement;
            set
            {
                var prev = this._contentElement;
                if(SetProperty(ref this._contentElement, value)) {
                    prev.Dispose();
                }
            }
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


        NoteData CreateNoteData([PixelKind(Px.Device)] Point cursorLocation)
        {
            this._dockScreen = DockScreen ?? Screen.PrimaryScreen;
            if(Position != NotePosition.Setting) {
                this._dockScreen = Screen.FromDevicePoint(cursorLocation);
            }

            var noteData = new NoteData() {
                NoteId = NoteId,
                FontId = Guid.Empty,
                Title = DateTime.Now.ToString(), //TODO: タイトル
                BackgroundColor = Colors.Yellow,
                ForegroundColor = Colors.Black,
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

                var screenOperator = new ScreenOperator(this);
                screenOperator.RegisterDatabase(DockScreen, commander, StatementLoader, commander.Implementation, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            return noteData;
        }

        Screen GetDockScreen(string screenDeviceName)
        {
            IList<NoteScreenData> noteScreens;

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var noteDomainDao = new NoteDomainDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                noteScreens = noteDomainDao.SelectNoteScreens(NoteId).ToList();
            }

            var screens = Screen.AllScreens;
            var screenChecker = new ScreenChecker();
            foreach(var screen in screens) {
                if(noteScreens.Any(i => screenChecker.FindMaybe(screen, i))) {
                    return screen;
                }
            }

            Logger.Warning($"該当ディスプレイ発見できず: ${screenDeviceName}");
            return Screen.PrimaryScreen;
        }

        void LoadNote()
        {
            //あればそれを読み込んでなければ作る
            var noteData = GetNoteData();
            if(noteData == null) {
                NativeMethods.GetCursorPos(out var podPoint);
                var deviceCursorLocation = PodStructUtility.Convert(podPoint);
                noteData = CreateNoteData(deviceCursorLocation);
            }

            DockScreen = GetDockScreen(noteData.ScreenName);

            IsVisible = noteData.IsVisible;
            IsLocked = noteData.IsLocked;
            IsCompact = noteData.IsCompact;
            IsTopmost = noteData.IsTopmost;
            TextWrap = noteData.TextWrap;
            Title = noteData.Title;
            LayoutKind = noteData.LayoutKind;
            ContentKind = noteData.ContentKind;
            ForegroundColor = noteData.ForegroundColor;
            BackgroundColor = noteData.BackgroundColor;

            FontElement = OrderManager.CreateFontElement(noteData.FontId, UpdateFontId);
            ContentElement = OrderManager.CreateNoteContentElement(NoteId, ContentKind);
        }

        void UpdateFontId(FontElement fontElement, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var notesEntityDao = new NotesEntityDao(commander, StatementLoader, implementation, Logger.Factory);
            notesEntityDao.UpdateFontId(NoteId, fontElement.FontId, DatabaseCommonStatus.CreateCurrentAccount());
        }

        public void SwitchCompact()
        {
            IsCompact = !IsCompact;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateCompact(NoteId, IsCompact, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void SwitchTopmost()
        {
            IsTopmost = !IsTopmost;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateTopmost(NoteId, IsTopmost, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void SwitchLock()
        {
            IsLocked = !IsLocked;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateLock(NoteId, IsLocked, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void SwitchTextWrap()
        {
            TextWrap = !TextWrap;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateTextWrap(NoteId, TextWrap, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeTitle(string editingTitle)
        {
            if(Title == editingTitle) {
                Logger.Debug("同一タイトルのため書き込み抑制");
                return;
            }

            Title = editingTitle;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateTitle(NoteId, Title, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        /// <summary>
        /// 座標・サイズの変更。
        /// <para>各種算出済みの値。</para>
        /// </summary>
        /// <param name="location"></param>
        public void ChangeViewArea(ViewAreaChangeTarget viewAreaChangeTargets, Point location, Size size)
        {
            MainDatabaseLazyWriter.Stock(c => {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                var layout = new NoteLayoutData() {
                    NoteId = NoteId,
                    LayoutKind = LayoutKind,
                    X = location.X,
                    Y = location.Y,
                    Width = size.Width,
                    Height = size.Height,
                };
                noteLayoutsEntityDao.UpdatePickupLayout(layout, viewAreaChangeTargets.HasFlag(ViewAreaChangeTarget.Location), viewAreaChangeTargets.HasFlag(ViewAreaChangeTarget.Suze), DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());

        }
        public void ChangeForegroundColor(Color color)
        {
            ForegroundColor = color;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateForegroundColor(NoteId, ForegroundColor, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void ChangeBackgroundColor(Color color)
        {
            BackgroundColor = color;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateBackgroundColor(NoteId, BackgroundColor, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        /// <summary>
        /// 単純にコンテンツ種別変更が可能であるかをチェック。
        /// <para>ほぼ空っぽ状態じゃないと無理。</para>
        /// </summary>
        /// <param name="targetContentKind"></param>
        /// <returns></returns>
        public bool CanChangeContentKind(NoteContentKind targetContentKind)
        {
            // どうでもいいやつ
            if(targetContentKind == ContentKind) {
                return true;
            }

            // 変換後データが存在すればもう無理
            if(ExistsContentKind(targetContentKind)) {
                Logger.Debug($"変換後データあり: {NoteId}, {targetContentKind}");
                return false;
            }

            // 文字列からRTFはOK
            if(ContentKind == NoteContentKind.Plain && targetContentKind == NoteContentKind.RichText) {
                Logger.Debug($"暗黙変換可能: {NoteId}, {targetContentKind}");
                return true;
            }

            // それ以外はもう無理でしょ
            return false;
        }

        public bool ExistsContentKind(NoteContentKind contentKind)
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectExistsContent(NoteId, contentKind);
            }
        }

        public void ConvertContentKind(NoteContentKind fromKind, NoteContentKind toKind)
        {

        }
        public void CreateContentKind(NoteContentKind contentKind)
        {

        }

        public void ChangeContentKind(NoteContentKind contentKind)
        {
            var prevContentKind = ContentKind;
            ContentKind = contentKind;
            ContentElement = OrderManager.CreateNoteContentElement(NoteId, ContentKind);
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                notesEntityDao.UpdateContentKind(NoteId, ContentKind, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public NoteLayoutData GetLayout()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                var layoutData = noteLayoutsEntityDao.SelectLayout(NoteId, LayoutKind);
                return layoutData;
            }
        }

        public void SaveLayout(NoteLayoutData layout)
        {
            if(layout == null) {
                throw new ArgumentNullException(nameof(layout));
            }
            if(layout.NoteId != NoteId) {
                throw new ArgumentException($"{nameof(layout)}.{nameof(layout.NoteId)}");
            }

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                if(noteLayoutsEntityDao.SelectExistsLayout(layout.NoteId, layout.LayoutKind)) {
                    noteLayoutsEntityDao.UpdateLayout(layout, DatabaseCommonStatus.CreateCurrentAccount());
                } else {
                    noteLayoutsEntityDao.InsertLayout(layout, DatabaseCommonStatus.CreateCurrentAccount());
                }
                commander.Commit();
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadNote();
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    MainDatabaseLazyWriter.Dispose();
                    FontElement?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            MainDatabaseLazyWriter.Flush();
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

        public bool ReceiveViewUserClosing()
        {
            return true;
        }
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
