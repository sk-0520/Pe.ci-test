using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteContentElement: ElementBase, IFlushable
    {
        #region event

        public event EventHandler? LinkContentChanged;

        #endregion

        #region variable

        private bool _isLink;

        #endregion

        public NoteContentElement(NoteId noteId, NoteContentKind contentKind, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseDelayWriter mainDatabaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            ContentKind = contentKind;

            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;

            DispatcherWrapper = dispatcherWrapper;

            MainDatabaseDelayWriter = mainDatabaseDelayWriter;

            LinkContentDelayChanger = new DelayAction(nameof(LinkContentDelayChanger), TimeSpan.FromSeconds(5), LoggerFactory);
        }

        #region property

        public NoteId NoteId { get; }
        public NoteContentKind ContentKind { get; }

        public bool IsLink
        {
            get => this._isLink;
            private set => SetProperty(ref this._isLink, value);
        }
        public bool IsLinkLoadError { get; set; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private IMainDatabaseDelayWriter MainDatabaseDelayWriter { get; }
        private UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        private NoteLinkWatcher? LinkWatcher { get; set; }
        private DelayAction LinkContentDelayChanger { get; }

        #endregion

        #region function

        public bool Exists()
        {
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectExistsContent(NoteId);
            }
        }

        void CreateNewContent(string content)
        {
            Logger.LogInformation("ノート空コンテンツ生成: {0}, {1}", NoteId, ContentKind);
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = ContentKind,
                    Content = content,
                    //TODO: あとあとかんがえよ
                    IsLink = false,
                    FilePath = string.Empty,
                    Encoding = EncodingConverter.DefaultEncoding,
                    DelayTime = TimeSpan.Zero,
                    BufferSize = 0,
                    RefreshTime = TimeSpan.Zero,
                    IsEnabledRefresh = true,
                };
                dao.InsertNewContent(data, DatabaseCommonStatus.CreateCurrentAccount());
                context.Commit();
            }
        }

        public string LoadRawContent()
        {
            ThrowIfDisposed();

            if(IsLink) {
                return LoadLinkContent();
            }

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectFullContent(NoteId);
            }
        }

        public string LoadPlainContent()
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }
            ThrowIfDisposed();

            if(!Exists()) {
                // 存在しなければこのタイミングで生成
                var factory = new NoteContentFactory();
                var content = factory.CreatePlain();
                CreateNewContent(content);
                // 作ったやつを返すだけなので別に。
                return content;
            }

            return LoadRawContent();
        }

        public string LoadRichTextContent()
        {
            if(ContentKind != NoteContentKind.RichText) {
                throw new InvalidOperationException();
            }
            ThrowIfDisposed();

            if(!Exists()) {
                var factory = new NoteContentFactory();
                var content = factory.CreateRichText();
                CreateNewContent(content);
                return content;
            }

            return LoadRawContent();
        }

        private NoteLinkWatchParameter? GetLinkParameter() => (NoteLinkWatchParameter?)LinkWatcher?.WatchParameter;

        private string LoadLinkContent()
        {
            ThrowIfDisposed();

            var parameter = GetLinkParameter();
            if(parameter == null) {
                Logger.LogWarning("リンクがおかしい: {0}", NoteId);
                return string.Empty;
            }

            using(var stream = parameter.File!.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                using(var reader = new StreamReader(stream, parameter.Encoding!)) {
                    return reader.ReadToEnd();
                }
            }
        }

        public void StartLinkWatch(NoteLinkWatchParameter noteLinkWatchParameter)
        {
            ThrowIfDisposed();

            if(LinkWatcher == null) {
                LinkWatcher = new NoteLinkWatcher(noteLinkWatchParameter, LoggerFactory);
                LinkWatcher.FileContentChanged += LinkWatcher2_FileContentChanged;
            }
            LinkWatcher.Start();
        }

        public void StopLinkWatch()
        {
            ThrowIfDisposed();

            LinkWatcher?.Stop();
        }

        private void SaveLinkContent(string? content)
        {
            ThrowIfDisposed();
            if(IsLinkLoadError) {
                Logger.LogWarning("エラーありのため保存スキップ: {0}", NoteId);
                return;
            }

            var parameter = (NoteLinkWatchParameter?)LinkWatcher?.WatchParameter;
            if(parameter == null) {
                Logger.LogWarning("リンクがおかしい: {0}", NoteId);
                return;
            }

            StopLinkWatch();
            using(new ActionDisposer(d => StartLinkWatch(parameter))) {
                try {
                    using(var stream = parameter.File!.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                        using(var writer = new StreamWriter(stream, parameter.Encoding!)) {
                            writer.Write(content);
                        }
                    }
                } catch(IOException ex) {
                    Logger.LogError(ex, "{0} {1}", ex.Message, NoteId);
                }
            }
        }

        private void ChangeRawContentDelaySave(NoteContentKind contentKind, string content, object stockKey)
        {
            ThrowIfDisposed();

            if(IsLink) {
                SaveLinkContent(content);
                return;
            }

            MainDatabaseDelayWriter.Stock(c => {
                var dao = new NoteContentsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = contentKind,
                    Content = content,
                };
                dao.UpdateContent(data, DatabaseCommonStatus.CreateCurrentAccount());
            }, stockKey);
        }

        public void ChangePlainContent(string content)
        {
            ThrowIfDisposed();

            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            ChangeRawContentDelaySave(ContentKind, content, UniqueKeyPool.Get());
        }

        public void ChangeRichTextContent(string content)
        {
            ThrowIfDisposed();

            if(ContentKind != NoteContentKind.RichText) {
                throw new InvalidOperationException();
            }

            ChangeRawContentDelaySave(ContentKind, content, UniqueKeyPool.Get());
        }

        private void DisposeLinkWatcher()
        {
            if(LinkWatcher != null) {
                LinkWatcher.FileContentChanged -= LinkWatcher2_FileContentChanged;
                LinkWatcher.Dispose();
                LinkWatcher = null;
            }
        }

        private void OnLinkContentChanged()
        {
            ThrowIfDisposed();

            LinkContentChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeLink(string filePath, Encoding encoding, bool isOpen)
        {
            ThrowIfDisposed();

            Flush();
            DisposeLinkWatcher();

            var path = Environment.ExpandEnvironmentVariables(filePath);

            if(!isOpen) {
                var content = LoadRawContent();
                using(var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)) {
                    using(var writer = new StreamWriter(stream, encoding)) {
                        writer.Write(content);
                    }
                }
            }

            var noteLinkWatchParameter = new NoteLinkWatchParameter() {
                File = new FileInfo(path),
                Encoding = encoding,
            };

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                // 環境変数込みで書き込み
                dao.UpdateLinkEnabled(NoteId, filePath, encoding, noteLinkWatchParameter, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            IsLink = true;
            StartLinkWatch(noteLinkWatchParameter);

            if(isOpen) {
                OnLinkContentChanged();
            }
        }

        public void Unlink(bool isRemove)
        {
            ThrowIfDisposed();

            Flush();

            var parameter = GetLinkParameter();
            if(parameter == null || parameter.File == null) {
                DisposeLinkWatcher();
                Logger.LogWarning("リンク状態が不正: {0}", NoteId);
                return;
            }
            DisposeLinkWatcher();

            parameter.File.Refresh();
            if(parameter.File.Exists) {
                var content = LoadLinkContent();

                switch(ContentKind) {
                    case NoteContentKind.Plain:
                        ChangePlainContent(content);
                        break;

                    case NoteContentKind.RichText:
                        ChangeRichTextContent(content);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            } else {
                Logger.LogWarning("リンク先が存在しない: {0}, {1}", parameter.File.FullName, NoteId);
            }

            IsLink = false;

            using(var context = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                dao.UpdateLinkDisabled(NoteId, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            if(isRemove) {
                if(parameter.File.Exists) {
                    try {
                        parameter.File!.Delete();
                    } catch(Exception ex) {
                        Logger.LogError(ex, ex.Message);
                    }
                }
            }
        }

        private (bool success, bool isLink, NoteLinkWatchParameter parameter) LoadLinkWatchParameter()
        {
            ThrowIfDisposed();

            NoteContentData linkData;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                linkData = dao.SelectLinkParameter(NoteId);
            }

            var parameter = new NoteLinkWatchParameter();

            if(!linkData.IsLink) {
                return (true, false, parameter);
            }

            if(string.IsNullOrWhiteSpace(linkData.FilePath)) {
                return (false, false, parameter);
            }

            var path = Environment.ExpandEnvironmentVariables(linkData.FilePath);
            parameter.File = new FileInfo(path);
            parameter.Encoding = linkData.Encoding;
            parameter.BufferSize = linkData.BufferSize;
            parameter.DelayTime = linkData.DelayTime;
            parameter.RefreshTime = linkData.RefreshTime;
            parameter.IsEnabledRefresh = linkData.IsEnabledRefresh;

            return (true, true, parameter);
        }

        public string GetLinkFilePath()
        {
            ThrowIfDisposed();

            var parameter = GetLinkParameter();
            return parameter?.File?.FullName ?? string.Empty;
        }

        #endregion

        #region ElementBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                DisposeLinkWatcher();
                if(disposing) {
                    LinkContentDelayChanger.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            if(!Exists()) {
                return Task.CompletedTask;
            }

            var values = LoadLinkWatchParameter();
            if(!values.success) {
                Logger.LogError("ノート内容初期化失敗: {0}", NoteId);
                return Task.CompletedTask;
            }

            IsLink = values.isLink;
            if(IsLink) {
                StartLinkWatch(values.parameter);
            }

            return Task.CompletedTask;
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            LinkContentDelayChanger.SafeFlush();
            MainDatabaseDelayWriter.SafeFlush();
        }

        #endregion

        private void LinkWatcher2_FileContentChanged(object? sender, FileChangedEventArgs e)
        {
            OnLinkContentChanged();
        }
    }
}
