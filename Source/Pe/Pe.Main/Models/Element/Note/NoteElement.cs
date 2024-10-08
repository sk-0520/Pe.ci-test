using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteElement: ElementBase, IViewShowStarter, IViewCloseReceiver, IFlushable
    {
        #region variable

        private bool _isVisible;
        private bool _isTopmost;
        private bool _isCompact;
        private bool _isLocked;
        private bool _textWrap;
        private bool _isLink;
        private string _title = string.Empty;
        private IScreen? _dockScreen;
        private NoteHiddenMode _hiddenMode;
        private NoteCaptionPosition _captionPosition;

        private NoteLayoutKind _layoutKind;
        private NoteContentKind _contentKind;

        private Color _foregroundColor;
        private Color _backgroundColor;

        private NoteContentElement? _contentElement;

        private bool _isVisibleBlind;
        private bool _hiddenCompact;

        #endregion

        public NoteElement(NoteId noteId, IScreen? dockScreen, NoteStartupPosition startupPosition, IOrderManager orderManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IMainDatabaseDelayWriter mainDatabaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, NoteConfiguration noteConfiguration, IDispatcherWrapper dispatcherWrapper, INoteTheme noteTheme, IIdFactory idFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            this._dockScreen = dockScreen; // プロパティは静かに暮らしたい
            StartupPosition = startupPosition;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            NoteConfiguration = noteConfiguration;
            DispatcherWrapper = dispatcherWrapper;
            NoteTheme = noteTheme;
            IdFactory = idFactory;

            MainDatabaseDelayWriter = mainDatabaseDelayWriter;
        }

        #region property

        public NoteId NoteId { get; }

        private NoteConfiguration NoteConfiguration { get; }
        private System.Timers.Timer? HideWaitTimer { get; set; }
        private IIdFactory IdFactory { get; }

        /// <summary>
        /// DB から取得して設定したりそれでも保存しなかったりするまさに変数。
        /// </summary>
        public IScreen DockScreen
        {
            get => this._dockScreen ?? Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen"); //TODO: [NOTE]決定的に間違ってる気がする
            private set => SetProperty(ref this._dockScreen, value);
        }
        public NoteStartupPosition StartupPosition { get; private set; }
        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private INoteTheme NoteTheme { get; }
        public SavingFontElement? FontElement { get; private set; }

        private IMainDatabaseDelayWriter MainDatabaseDelayWriter { get; }
        private UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        /// <summary>
        /// 添付ファイル。
        /// </summary>
        public ObservableCollection<NoteFileElement> Files { get; } = new();

        private bool ViewCreated { get; set; }

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

        public bool IsLink
        {
            get => this._isLink;
            private set => SetProperty(ref this._isLink, value);
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
            get => this._backgroundColor;
            private set => SetProperty(ref this._backgroundColor, value);
        }

        /// <summary>
        /// 表示されているか。
        /// </summary>
        public bool IsVisible
        {
            get => this._isVisible;
            set => SetProperty(ref this._isVisible, value);
        }

        public bool IsVisibleBlind
        {
            get => this._isVisibleBlind;
            private set => SetProperty(ref this._isVisibleBlind, value);
        }
        internal bool HiddenCompact
        {
            get => this._hiddenCompact;
            private set => SetProperty(ref this._hiddenCompact, value);
        }
        public NoteHiddenMode HiddenMode
        {
            get => this._hiddenMode;
            set => SetProperty(ref this._hiddenMode, value);
        }

        public NoteCaptionPosition CaptionPosition
        {
            get => this._captionPosition;
            private set => SetProperty(ref this._captionPosition, value);
        }

        public NoteContentElement? ContentElement
        {
            get => this._contentElement;
            private set => SetProperty(ref this._contentElement, value);
        }

        private NotifyLogId RestoreVisibleNotifyLogId { get; set; }

        #endregion

        #region function

        private NoteData? GetNoteData()
        {
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new NotesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectNote(NoteId);
            }
        }


        private NoteData CreateNoteData([PixelKind(Px.Device)] Point cursorLocation)
        {
            ThrowIfDisposed();

            this._dockScreen = this._dockScreen ?? Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null");
            if(StartupPosition != NoteStartupPosition.Setting) {
                this._dockScreen = Screen.FromDevicePoint(cursorLocation);
            }

            var noteData = new NoteData() {
                NoteId = NoteId,
                //FontId = Guid.Empty,
                //Title = DateTime.Now.ToString(), //TODO: タイトル
                //BackgroundColor = Colors.Yellow,
                //ForegroundColor = Colors.Black,
                ScreenName = this._dockScreen.DeviceName,
                IsCompact = false,
                IsLocked = false,
                //IsTopmost = false,
                IsVisible = true,
                //LayoutKind = NoteLayoutKind.Absolute,
                TextWrap = true,
                ContentKind = NoteContentKind.Plain,
                HiddenMode = NoteHiddenMode.None,
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

            using(var context = MainDatabaseBarrier.WaitWrite()) {

                var notesEntityDao = new NotesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                notesEntityDao.InsertNewNote(noteData, DatabaseCommonStatus.CreateCurrentAccount());

                /*
                var notesLayoutDao = new NoteLayoutsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                notesLayoutDao.InsertNewLayout(noteLayout, DatabaseCommonStatus.CreateCurrentAccount());

                var noteContentDao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                noteContentDao.InsertNewContent(noteContent, DatabaseCommonStatus.CreateCurrentAccount());
                */

                ScreenUtility.RegisterDatabase(DockScreen, context, DatabaseStatementLoader, context.Implementation, DatabaseCommonStatus.CreateCurrentAccount(), LoggerFactory);

                noteData = notesEntityDao.SelectNote(NoteId)!;

                context.Commit();
            }

            return noteData;
        }

        private IScreen GetDockScreen(string screenDeviceName)
        {
            ThrowIfDisposed();

            IList<NoteScreenData> noteScreens;

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var noteDomainDao = new NoteDomainDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
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
            return Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null");
        }

        private async Task LoadNoteAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            //あればそれを読み込んでなければ作る
            var noteData = GetNoteData();
            if(noteData == null) {
                NativeMethods.GetCursorPos(out var podPoint);
                var deviceCursorLocation = PodStructUtility.Convert(podPoint);
                noteData = CreateNoteData(deviceCursorLocation);
            } else {
                IEnumerable<NoteFileData> files;
                using(var context = MainDatabaseBarrier.WaitRead()) {
                    var noteFilesEntityDao = new NoteFilesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    files = noteFilesEntityDao.SelectNoteFiles(NoteId);
                }
                var fileElements = files.Select(a => new NoteFileElement(a, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, DispatcherWrapper, LoggerFactory));
                foreach(var fileElement in fileElements) {
                    await fileElement.InitializeAsync(cancellationToken);
                    Files.Add(fileElement);
                }
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
            HiddenMode = noteData.HiddenMode;
            CaptionPosition = noteData.CaptionPosition;

            FontElement = await OrderManager.CreateFontElementAsync(DefaultFontKind.Note, noteData.FontId, UpdateFontId, cancellationToken);
            var oldContentElement = ContentElement;
            ContentElement = await OrderManager.CreateNoteContentElementAsync(NoteId, ContentKind, cancellationToken);
            oldContentElement?.Dispose();
        }

        private void UpdateFontId(SavingFontElement fontElement, IDatabaseContext context, IDatabaseImplementation implementation)
        {
            ThrowIfDisposed();

            var notesEntityDao = new NotesEntityDao(context, DatabaseStatementLoader, implementation, LoggerFactory);
            notesEntityDao.UpdateFontId(NoteId, fontElement.FontId, DatabaseCommonStatus.CreateCurrentAccount());
        }

        public void ToggleCompactDelaySave()
        {
            ThrowIfDisposed();

            if(!IsCompact && !IsLocked) {
                ContentElement?.SafeFlush();
            }

            IsCompact = !IsCompact;
            if(IsCompact) {
                HiddenCompact = false;
            }

            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateCompact(NoteId, IsCompact, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ToggleTopmostDelaySave()
        {
            ThrowIfDisposed();

            IsTopmost = !IsTopmost;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateTopmost(NoteId, IsTopmost, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void SetTopmost(bool isTopmost)
        {
            IsTopmost = isTopmost;
        }

        public void ToggleLockDelaySave()
        {
            ThrowIfDisposed();

            IsLocked = !IsLocked;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateLock(NoteId, IsLocked, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ToggleTextWrapDelaySave()
        {
            ThrowIfDisposed();

            TextWrap = !TextWrap;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateTextWrap(NoteId, TextWrap, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeTitleDelaySave(string editingTitle)
        {
            if(Title == editingTitle) {
                Logger.LogDebug("同一タイトルのため書き込み抑制");
                return;
            }
            ThrowIfDisposed();

            Title = editingTitle;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateTitle(NoteId, Title, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeDockScreen(IScreen screen)
        {
            DockScreen = screen;
        }

        /// <summary>
        /// 座標・サイズの変更。
        /// </summary>
        /// <remarks>
        /// <para>各種算出済みの値。</para>
        /// </remarks>
        /// <param name="location"></param>
        public void ChangeViewAreaDelaySave(ViewAreaChangeTarget viewAreaChangeTargets, Point location, Size size)
        {
            ThrowIfDisposed();

            MainDatabaseDelayWriter.Stock(c => {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                var layout = new NoteLayoutData() {
                    NoteId = NoteId,
                    LayoutKind = LayoutKind,
                    X = location.X,
                    Y = location.Y,
                    Width = size.Width,
                    Height = size.Height,
                };
                noteLayoutsEntityDao.UpdatePickupLayout(layout, viewAreaChangeTargets.HasFlag(ViewAreaChangeTarget.Location), viewAreaChangeTargets.HasFlag(ViewAreaChangeTarget.Size), DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        /// <summary>
        /// スクリーン位置が異なる場合に遅延変更。
        /// </summary>
        /// <param name="screen"></param>
        public void SaveDisplayDelaySave(IScreen screen)
        {
            ThrowIfDisposed();

            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                var currentNote = notesEntityDao.SelectNote(NoteId);
                if(currentNote is null) {
                    return;
                }

                var screensEntityDao = new ScreensEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                if(!screensEntityDao.SelectExistsScreen(screen.DeviceName)) {
                    screensEntityDao.InsertScreen(screen, DatabaseCommonStatus.CreateCurrentAccount());
                }

                if(screen.DeviceName != currentNote.ScreenName) {
                    notesEntityDao.UpdateScreen(NoteId, screen.DeviceName, DatabaseCommonStatus.CreateCurrentAccount());
                }

            }, UniqueKeyPool.Get());
        }

        public void ChangeForegroundColorDelaySave(Color color)
        {
            ThrowIfDisposed();

            ForegroundColor = color;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateForegroundColor(NoteId, ForegroundColor, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }
        public void ChangeBackgroundColorDelaySave(Color color)
        {
            ThrowIfDisposed();

            BackgroundColor = color;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateBackgroundColor(NoteId, BackgroundColor, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeCaptionPositionDelaySave(NoteCaptionPosition captionPosition)
        {
            ThrowIfDisposed();

            CaptionPosition = captionPosition;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateCaptionPosition(NoteId, CaptionPosition, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        /// <summary>
        /// 単純にコンテンツ種別変更が可能であるかをチェック。
        /// </summary>
        /// <remarks>
        /// <para>ほぼ空っぽ状態じゃないと無理。</para>
        /// </remarks>
        /// <param name="targetContentKind"></param>
        /// <returns></returns>
        public bool CanChangeContentKind(NoteContentKind targetContentKind)
        {
            ThrowIfDisposed();

            // どうでもいいやつ
            if(targetContentKind == ContentKind) {
                return true;
            }

            //// 変換後データが存在すればもう無理
            //if(ExistsContentKind(targetContentKind)) {
            //    Logger.LogDebug("変換後データあり: {0}, {1}", NoteId, targetContentKind);
            //    return false;
            //}

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
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectExistsContent(NoteId);
            }
        }

        public void OpenLinkContent(string filePath, Encoding encoding, bool isOpen)
        {
            ThrowIfDisposed();

            ContentElement!.ChangeLink(filePath, encoding, isOpen);
        }

        public void Unlink(bool isRemove)
        {
            ThrowIfDisposed();

            ContentElement!.Unlink(isRemove);
        }

        private string ConvertContent(NoteContentKind fromKind, string fromRawContent, NoteContentKind toKind)
        {
            ThrowIfDisposed();

            Debug.Assert(FontElement != null);

            var noteContentConverter = new NoteContentConverter(LoggerFactory);
            switch(fromKind) {
                case NoteContentKind.Plain:
                    switch(toKind) {
                        case NoteContentKind.RichText:
                            Debug.Assert(DispatcherWrapper.CheckAccess());
                            return noteContentConverter.ToRichText(fromRawContent, FontElement.FontData, ForegroundColor);

                        case NoteContentKind.Plain:
                        default:
                            throw new NotImplementedException();
                    }

                case NoteContentKind.RichText:
                    switch(toKind) {
                        case NoteContentKind.Plain:
                            return noteContentConverter.ToPlain(fromRawContent);

                        case NoteContentKind.RichText:
                        default:
                            throw new NotImplementedException();
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public async Task ConvertContentKindAsync(NoteContentKind toContentKind, CancellationToken cancellationToken)
        {
            if(ContentKind == toContentKind) {
                throw new ArgumentException($"{nameof(ContentKind)} == {nameof(toContentKind)}", nameof(toContentKind));
            }
            if(IsLink) {
                throw new InvalidOperationException(nameof(IsLink));
            }
            ThrowIfDisposed();

            if(ContentElement == null) {
                return;
            }

            var prevContentKind = ContentKind;
            var oldContentElement = ContentElement;

            using(MainDatabaseDelayWriter.Pause()) {
                var fromRawContent = ContentElement.LoadRawContent();
                var convertedContent = ConvertContent(ContentKind, fromRawContent, toContentKind);
                var contentData = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = toContentKind,
                    Content = convertedContent,
                };
                using(var context = MainDatabaseBarrier.WaitWrite()) {
                    var noteContentsEntityDao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    if(noteContentsEntityDao.SelectExistsContent(contentData.NoteId)) {
                        noteContentsEntityDao.UpdateContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                    } else {
                        noteContentsEntityDao.InsertNewContent(contentData, DatabaseCommonStatus.CreateCurrentAccount());
                    }

                    var notesEntityDao = new NotesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    notesEntityDao.UpdateContentKind(NoteId, toContentKind, DatabaseCommonStatus.CreateCurrentAccount());

                    context.Commit();
                }
            }

            ContentKind = toContentKind;
            ContentElement = await OrderManager.CreateNoteContentElementAsync(NoteId, ContentKind, cancellationToken);

            oldContentElement?.Dispose();
        }

        public void ChangeLayoutKind(NoteLayoutData layoutData)
        {
            Flush();
            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var notesEntityDao = new NotesEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                notesEntityDao.UpdateLayoutKind(NoteId, layoutData.LayoutKind, DatabaseCommonStatus.CreateCurrentAccount());

                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(noteLayoutsEntityDao.SelectExistsLayout(NoteId, layoutData.LayoutKind)) {
                    noteLayoutsEntityDao.UpdateLayout(layoutData, DatabaseCommonStatus.CreateCurrentAccount());
                } else {
                    noteLayoutsEntityDao.InsertLayout(layoutData, DatabaseCommonStatus.CreateCurrentAccount());
                }

                context.Commit();
            }
            LayoutKind = layoutData.LayoutKind;
        }

        public void ChangeVisibleDelaySave(bool isVisible)
        {
            ThrowIfDisposed();

            if(!IsCompact && !IsLocked) {
                ContentElement?.SafeFlush();
            }

            IsVisible = isVisible;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateVisible(NoteId, IsVisible, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeHiddenModeDelaySave(NoteHiddenMode hiddenMode)
        {
            ThrowIfDisposed();

            HiddenMode = hiddenMode;
            MainDatabaseDelayWriter.Stock(c => {
                var notesEntityDao = new NotesEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                notesEntityDao.UpdateHiddenMode(NoteId, HiddenMode, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public NoteLayoutData? GetLayout()
        {
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
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
                throw new ArgumentException(null, $"{nameof(layout)}.{nameof(layout.NoteId)}");
            }
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var noteLayoutsEntityDao = new NoteLayoutsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(noteLayoutsEntityDao.SelectExistsLayout(layout.NoteId, layout.LayoutKind)) {
                    noteLayoutsEntityDao.UpdateLayout(layout, DatabaseCommonStatus.CreateCurrentAccount());
                } else {
                    noteLayoutsEntityDao.InsertLayout(layout, DatabaseCommonStatus.CreateCurrentAccount());
                }
                context.Commit();
            }
        }

        public void StartHidden()
        {
            if(HiddenMode == NoteHiddenMode.None) {
                throw new InvalidOperationException(nameof(HiddenMode));
            }

            StopHidden(true);

            TimeSpan waitTime;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var appNoteHiddenSettingEntityDao = new AppNoteHiddenSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                waitTime = appNoteHiddenSettingEntityDao.SelectHiddenWaitTime(HiddenMode);
            }

            HideWaitTimer = new System.Timers.Timer() {
                Interval = (int)waitTime.TotalMilliseconds,
                AutoReset = false,
            };
            HideWaitTimer.Elapsed += HideWaitTimer_Elapsed;
            HideWaitTimer.Start();
        }

        public void StopHidden(bool restore)
        {
            if(HideWaitTimer != null) {
                HideWaitTimer.Elapsed -= HideWaitTimer_Elapsed;
                HideWaitTimer.Stop();
                HideWaitTimer.Dispose();
                HideWaitTimer = null;
            }

            if(restore) {
                IsVisibleBlind = false;
                HiddenCompact = false;
            }
        }

        private void Hide()
        {
            Logger.LogInformation("自動的に隠す: {0}, {1}", HiddenMode, NoteId);

            switch(HiddenMode) {
                case NoteHiddenMode.Blind:
                    IsVisibleBlind = true;
                    break;

                case NoteHiddenMode.Compact:
                    if(!IsCompact) {
                        HiddenCompact = true;
                    }
                    break;

                case NoteHiddenMode.None:
                default:
                    throw new NotImplementedException();
            }
        }

        public void ReceiveInitialized()
        {
            // 今後は設定から読むように変更
            if(StartupPosition != NoteStartupPosition.Setting) {
                StartupPosition = NoteStartupPosition.Setting;
            }
        }

        /// <summary>
        /// 添付ファイルを追加。
        /// </summary>
        /// <param name="path">対象ファイルパス。</param>
        /// <param name="kind"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> AddFileAsync(string path, NoteFileKind kind, CancellationToken cancellationToken)
        {
            return Task.Run(async () => {
                var isFile = File.Exists(path);
                var idDir = !isFile && Directory.Exists(path);

                if(!isFile && !idDir) {
                    return false;
                }

                cancellationToken.ThrowIfCancellationRequested();

                NoteFileData noteFileData;
                using(var mainContext = MainDatabaseBarrier.WaitWrite()) {
                    var noteFilesEntityDao = new NoteFilesEntityDao(mainContext, DatabaseStatementLoader, mainContext.Implementation, LoggerFactory);

                    // 現存データ有無確認
                    var noteFileId = noteFilesEntityDao.SelectNoteFileExistsFilePath(NoteId, path);
                    if(noteFileId is not null) {
                        // インデックス側 更新
                        if(kind == NoteFileKind.Reference) {
                            // リンクの場合なにもやることはない
                            return true;
                        }
                        throw new NotImplementedException();
                    } else {
                        // インデックス側 追加
                        var sequence = noteFilesEntityDao.SelectNextSequenceNoteFiles(NoteId);
                        noteFileId = IdFactory.CreateNoteFileId();

                        noteFileData = new NoteFileData() {
                            NoteId = NoteId,
                            NoteFileId = noteFileId.Value,
                            NoteFileKind = kind,
                            NoteFilePath = path,
                            Sequence = sequence,
                        };
                        noteFilesEntityDao.InsertNoteFiles(noteFileData, DatabaseCommonStatus.CreateCurrentAccount());
                    }

                    var todo = false;
                    if(todo) {
                        using(var largeContext = LargeDatabaseBarrier.WaitWrite()) {
                            // 既存データ破棄
                            //TODO: 取り込み処理
                            largeContext.Commit();
                        }
                    }

                    mainContext.Commit();
                }

                var noteFileElement = new NoteFileElement(noteFileData, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, DispatcherWrapper, LoggerFactory);
                await noteFileElement.InitializeAsync(cancellationToken);
                Files.Add(noteFileElement);

                return true;
            }, cancellationToken);
        }

        /// <summary>
        /// ノートに添付したファイルの削除。
        /// </summary>
        /// <remarks>
        /// <para>物理削除はされない。</para>
        /// </remarks>
        /// <param name="noteFileId">ノートファイルID。</param>
        /// <returns>削除成功状態。</returns>
        public bool UnlinkFile(NoteFileId noteFileId)
        {
            var file = Files.FirstOrDefault(a => a.NoteFileId == noteFileId);
            if(file is null) {
                Logger.LogWarning("削除失敗: {NoteFileId}", noteFileId);
                return false;
            }

            using(var mainContext = MainDatabaseBarrier.WaitWrite()) {
                var noteFilesEntityDao = new NoteFilesEntityDao(mainContext, DatabaseStatementLoader, mainContext.Implementation, LoggerFactory);
                // 削除処理
                noteFilesEntityDao.DeleteNoteFilesById(NoteId, noteFileId);

                var todo = false;
                if(todo) {
                    // 添付削除
                    using(var largeContext = LargeDatabaseBarrier.WaitWrite()) {
                    }
                }

                mainContext.Commit();
            }

            Files.Remove(file);
            file.Dispose();

            return true;
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return LoadNoteAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                if(disposing) {
                    FontElement?.Dispose();
                    ContentElement?.Dispose();
                    var files = Files.ToArray();
                    foreach(var file in files) {
                        file.Dispose();
                    }
                    StopHidden(false);
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
            MainDatabaseDelayWriter.SafeFlush();
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
            if(RestoreVisibleNotifyLogId != NotifyLogId.Empty) {
                NotifyManager.ClearLog(RestoreVisibleNotifyLogId);
            }

            var windowItem = OrderManager.CreateNoteWindow(this);

            ViewCreated = true;
        }

        #endregion

        #region IWindowCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            ChangeVisibleDelaySave(false);
            return true;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosedAsync(bool)"/>
        public async Task ReceiveViewClosedAsync(bool isUserOperation, CancellationToken cancellationToken)
        {
            if(isUserOperation) {
                if(!IsVisible) {
                    var notifyMessage = new NotifyMessage(
                        NotifyLogKind.Undo,
                        Properties.Resources.String_Note_Notify_Hidden_Header,
                        new NotifyLogContent(
                            TextUtility.ReplaceFromDictionary(
                                Properties.Resources.String_Note_Notify_Hidden_Content_Format,
                                new Dictionary<string, string>() {
                                    ["NOTE-CAPTION"] = Title,
                                }
                            )
                        ),
                        () => {
                            if(!ViewCreated) {
                                RestoreVisibleNotifyLogId = NotifyLogId.Empty;
                                ChangeVisibleDelaySave(true);
                                StartView();
                            }
                        }
                    );
                    RestoreVisibleNotifyLogId = await NotifyManager.AppendLogAsync(notifyMessage, cancellationToken);
                }
            }

            ViewCreated = false;
        }


        #endregion

        private void HideWaitTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            StopHidden(false);
            Hide();
        }
    }
}
