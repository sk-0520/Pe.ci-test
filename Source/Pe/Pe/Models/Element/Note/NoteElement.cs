using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteElement : ElementBase, IViewShowStarter, IViewCloseReceiver, IFlushable
    {
        #region variable

        bool _isVisible;
        bool _isTopmost;
        bool _isCompact;
        bool _isLocked;
        bool _textWrap;
        string? _title;
        Screen _dockScreen;

        NoteLayoutKind _layoutKind;
        NoteContentKind _contentKind;

        Color _foregroundColor;
        Color _backegroundColor;

        NoteContentElement? _contentElement;

        #endregion

        public NoteElement(Guid noteId, Screen dockScreen, NotePosition notePosition, IOrderManager orderManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
            Position = notePosition;
            OrderManager = orderManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
            DispatcherWapper = dispatcherWapper;
            NoteTheme = noteTheme;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;
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
        IDispatcherWapper DispatcherWapper { get; }
        INoteTheme NoteTheme { get; }
        public FontElement? FontElement { get; private set; }

        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
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
        public string? Title
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

        public NoteContentElement? ContentElement
        {
            get => this._contentElement;
            private set => SetProperty(ref this._contentElement, value);
        }

        #endregion

        #region function

        NoteData? GetNoteData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NotesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
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
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                ScreenName = DockScreen.DeviceName,
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
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

                var notesEntityDao = new NotesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                notesEntityDao.InsertNewNote(noteData, DatabaseCommonStatus.CreateCurrentAccount());

                /*
                var notesLayoutDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                notesLayoutDao.InsertNewLayout(noteLayout, DatabaseCommonStatus.CreateCurrentAccount());

                var noteContentDao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                noteContentDao.InsertNewContent(noteContent, DatabaseCommonStatus.CreateCurrentAccount());
                */

                var screenOperator = new ScreenOperator(LoggerFactory);
                screenOperator.RegisterDatabase(DockScreen, commander, StatementLoader, commander.Implementation, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }

            return noteData;
        }

        Screen GetDockScreen(string? screenDeviceName)
        {
            IList<NoteScreenData> noteScreens;

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var noteDomainDao = new NoteDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                noteScreens = noteDomainDao.SelectNoteScreens(NoteId).ToList();
            }

            var screens = Screen.AllScreens;
            var screenChecker = new ScreenChecker();
            foreach(var screen in screens) {
                if(noteScreens.Any(i => screenChecker.FindMaybe(screen, i))) {
                    return screen;
                }
            }

            Logger.LogWarning("該当ディスプレイ発見できず: {0}", screenDeviceName);
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
            var oldContentElement = ContentElement;
            ContentElement = OrderManager.CreateNoteContentElement(NoteId, ContentKind);
            oldContentElement?.Dispose();
        }

        void UpdateFontId(FontElement fontElement, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var notesEntityDao = new NotesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            notesEntityDao.UpdateFontId(NoteId, fontElement.FontId, DatabaseCommonStatus.CreateCurrentAccount());
        }

        public void SwitchCompact()
        {
            IsCompact = !IsCompact;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateCompact(NoteId, IsCompact, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void SwitchTopmost()
        {
            IsTopmost = !IsTopmost;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateTopmost(NoteId, IsTopmost, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void SwitchLock()
        {
            IsLocked = !IsLocked;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateLock(NoteId, IsLocked, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void SwitchTextWrap()
        {
            TextWrap = !TextWrap;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateTextWrap(NoteId, TextWrap, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeTitle(string editingTitle)
        {
            if(Title == editingTitle) {
                Logger.LogDebug("同一タイトルのため書き込み抑制");
                return;
            }

            Title = editingTitle;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
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
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
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
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateForegroundColor(NoteId, ForegroundColor, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void ChangeBackgroundColor(Color color)
        {
            BackgroundColor = color;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
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
                Logger.LogDebug("変換後データあり: {0}, {1}", NoteId, targetContentKind);
                return false;
            }

            // 文字列からRTFはOK
            if(ContentKind == NoteContentKind.Plain && targetContentKind == NoteContentKind.RichText) {
                Logger.LogDebug("暗黙変換可能: {0}, {1}", NoteId, targetContentKind);
                return true;
            }

            // それ以外はもう無理でしょ
            return false;
        }

        public bool ExistsContentKind(NoteContentKind contentKind)
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectExistsContent(NoteId);
            }
        }

        bool WriteLinkContent(NoteLinkContentData linkData, string content)
        {
            try {
                var fileInfo = linkData.ToFileInfo();
                using(var stream = fileInfo.Open(System.IO.FileMode.Truncate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read)) {
                    using(var writer = new StreamWriter(stream, linkData.ToEncoding())) {
                        writer.Write(content);
                    }
                }
                return true;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            return false;
        }

        string ConvertContent(NoteContentKind fromKind, string fromRawContent, NoteContentKind toKind, NoteLinkContentData? linkData)
        {
            var noteContentConverter = new NoteContentConverter(LoggerFactory);
            switch(fromKind) {
                case NoteContentKind.Plain:
                    switch(toKind) {
                        case NoteContentKind.RichText:
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                            return DispatcherWapper.Get(() => noteContentConverter.ToRichText(fromRawContent, FontElement.FontData, ForegroundColor));
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

                        case NoteContentKind.Link: {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                                WriteLinkContent(linkData, fromRawContent);
                                return noteContentConverter.ToLinkSettingString(linkData);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                            }

                        case NoteContentKind.Plain:
                        default:
                            throw new NotImplementedException();
                    }

                case NoteContentKind.RichText:
                    switch(toKind) {
                        case NoteContentKind.Plain:
                            return noteContentConverter.ToPlain(fromRawContent);

                        case NoteContentKind.Link: {
                                var content = noteContentConverter.ToPlain(fromRawContent);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                                WriteLinkContent(linkData, content);
                                return noteContentConverter.ToLinkSettingString(linkData);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                            }

                        case NoteContentKind.RichText:
                        default:
                            throw new NotImplementedException();
                    }

                case NoteContentKind.Link: {
                        var linkSetting = noteContentConverter.ToLinkSetting(fromRawContent);
                        try {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                            var linkContent = ContentElement.LoadLinkContent(linkSetting.ToFileInfo(), linkSetting.ToEncoding());
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
                            switch(toKind) {
                                case NoteContentKind.Plain:
                                    return linkContent;

                                case NoteContentKind.RichText:
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                                    return noteContentConverter.ToRichText(linkContent, FontElement.FontData, ForegroundColor);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

                                case NoteContentKind.Link:
                                default:
                                    throw new NotImplementedException();
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                        }
                        return string.Empty;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public void ConvertContentKind(NoteContentKind fromKind, NoteContentKind toKind, NoteLinkContentData? linkData)
        {
            if(fromKind == toKind) {
                throw new ArgumentException($"{nameof(fromKind)} == {nameof(toKind)}");
            }
            if(toKind == NoteContentKind.Link && linkData == null) {
                throw new ArgumentNullException(nameof(linkData));
            }

            using(MainDatabaseLazyWriter.Pause()) {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                var fromRawContent = ContentElement.LoadRawContent();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
                var convertedContent = ConvertContent(fromKind, fromRawContent, toKind, linkData);
                var contentData = new NoteContentData() {
                    NoteId = NoteId,
                    Content = convertedContent,
                };
                using(var commander = MainDatabaseBarrier.WaitWrite()) {
                    var notesEntityDao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    if(notesEntityDao.SelectExistsContent(contentData.NoteId)) {
                        notesEntityDao.UpdateContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                    } else {
                        notesEntityDao.InsertNewContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                    }

                    commander.Commit();
                }
            }
        }

        public void CreateContentKind(NoteContentKind contentKind, NoteLinkContentData? linkData)
        {
            if(contentKind == NoteContentKind.Link && linkData == null) {
                throw new ArgumentNullException(nameof(linkData));
            }

            var noteContentConverter = new NoteContentConverter(LoggerFactory);

            var contentData = new NoteContentData() {
                NoteId = NoteId,
                Content = contentKind == NoteContentKind.Link
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                    ? noteContentConverter.ToLinkSettingString(linkData)
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    : string.Empty
                ,
            };
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var notesEntityDao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                if(notesEntityDao.SelectExistsContent(contentData.NoteId)) {
                    notesEntityDao.UpdateContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                } else {
                    notesEntityDao.InsertNewContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                }

                commander.Commit();
            }
        }

        public void ChangeContentKind(NoteContentKind contentKind)
        {
            var prevContentKind = ContentKind;
            ContentKind = contentKind;
            var oldContentElement = ContentElement;
            ContentElement = OrderManager.CreateNoteContentElement(NoteId, ContentKind);
            oldContentElement?.Dispose();
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateContentKind(NoteId, ContentKind, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void ChangeVisible(bool isVisible)
        {
            IsVisible = isVisible;
            MainDatabaseLazyWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateVisible(NoteId, IsVisible, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public NoteLayoutData GetLayout()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
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
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
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
                Flush();
                if(disposing) {
                    MainDatabaseLazyWriter.Dispose();
                    FontElement?.Dispose();
                    ContentElement?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            FontElement.SafeFlush();
            ContentElement.SafeFlush();
            MainDatabaseLazyWriter.SafeFlush();
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
            ChangeVisible(false);
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
