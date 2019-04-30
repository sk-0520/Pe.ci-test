using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Note;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteContentElement : ElementBase, IFlush
    {
        #region variable
        #endregion

        public NoteContentElement(Guid noteId, NoteContentKind contentKind, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            ContentKind = contentKind;

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            DispatcherWapper = dispatcherWapper;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.NoteContentMainDatabaseLazyWriterWaitTime, this);

            LinkContentLazyChanger = new LazyAction(nameof(LinkContentLazyChanger), TimeSpan.FromSeconds(5), Logger.Factory);

        }

        #region property

        public Guid NoteId { get; }
        public NoteContentKind ContentKind { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDispatcherWapper DispatcherWapper { get; }
        DatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        NoteLinkContentWatcher LinkWatcher { get; set; }
        LazyAction LinkContentLazyChanger { get; }

        #endregion

        #region function

        public bool Exists()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectExistsContent(NoteId, ContentKind);
            }
        }

        void CreateNewContent(string content)
        {
            Logger.Information($"ノート空コンテンツ生成: {NoteId}, {ContentKind}");
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = ContentKind,
                    Content = content,
                };
                dao.InsertNewContent(data, DatabaseCommonStatus.CreateCurrentAccount());
                commander.Commit();
            }
        }

        public string LoadRawContent()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectFullContent(NoteId, ContentKind);
            }
        }

        public string LoadPlainContent()
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

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

            if(!Exists()) {
                var factory = new NoteContentFactory();
                var content = factory.CreateRichText();
                CreateNewContent(content);
                return content;
            }

            return LoadRawContent();
        }

        public NoteLinkContentData LoadLinkSetting()
        {
            if(ContentKind != NoteContentKind.Link) {
                throw new InvalidOperationException();
            }

            var converter = new NoteContentConverter(Logger.Factory);

            if(!Exists()) {
                var factory = new NoteContentFactory();
                var setting = factory.CreateLink();

                var content = converter.ToLinkSettingString(setting);
                CreateNewContent(content);
                return setting;
            }

            var rawSetting = LoadRawContent();
            return converter.ToLinkSetting(rawSetting);
        }

        public string LoadLinkContent(FileInfo file, Encoding encoding)
        {
            using(var stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)) {
                var reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
        }

        public NoteLinkContentWatcher StartLinkWatch(NoteLinkContentData linkData)
        {
            if(LinkWatcher == null) {
                LinkWatcher = new NoteLinkContentWatcher(linkData, Logger.Factory);
            }
            LinkWatcher.Start();
            return LinkWatcher;
        }

        public void StopLinkWatch()
        {
            if(LinkWatcher != null) {
                LinkWatcher.Stop();
            }
        }

        public void ChangePlainContent(string content)
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new NoteContentsEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = ContentKind,
                    Content = content,
                };
                dao.UpdateContent(data, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        public void ChangeRichTextContent(string content)
        {
            if(ContentKind != NoteContentKind.RichText) {
                throw new InvalidOperationException();
            }

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new NoteContentsEntityDao(c, StatementLoader, c.Implementation, Logger.Factory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    ContentKind = ContentKind,
                    Content = content,
                };
                dao.UpdateContent(data, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        void DelayLinkContentChange(string content)
        {
            LinkContentLazyChanger.DelayAction(() => {
                LinkWatcher.Stop();

                using(var stream = new FileStream(LinkWatcher.File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) {
                    using(var writer = new StreamWriter(stream, LinkWatcher.Encoding)) {
                        writer.Write(content);
                    }
                }

                LinkWatcher.Start();
            });
        }

        public void ChangeLinkContent(string content)
        {
            if(ContentKind != NoteContentKind.Link) {
                throw new InvalidOperationException();
            }
            if(LinkWatcher == null) {
                throw new InvalidOperationException();
            }

            DelayLinkContentChange(content);
        }

        void DisposeLinkWatcher()
        {
            if(LinkWatcher != null) {
                LinkWatcher.Dispose();
            }
        }

        #endregion

        #region ElementBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
                DisposeLinkWatcher();
                if(disposing) {
                    LinkContentLazyChanger.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            // この子は遅延読み込みさせたいのです
        }

        #endregion

        #region IFlush

        public void Flush()
        {
            MainDatabaseLazyWriter.Flush();
            LinkContentLazyChanger.Flush();
        }

        #endregion

    }
}
