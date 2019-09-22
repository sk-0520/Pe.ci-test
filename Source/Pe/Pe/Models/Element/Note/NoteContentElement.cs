using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteContentElement : ElementBase, IFlushable
    {
        #region variable
        #endregion

        public NoteContentElement(Guid noteId, NoteContentKind contentKind, IMainDatabaseBarrier mainDatabaseBarrier, IMainDatabaseLazyWriter mainDatabaseLazyWriter, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            ContentKind = contentKind;

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            DispatcherWapper = dispatcherWapper;

            MainDatabaseLazyWriter = mainDatabaseLazyWriter;

            LinkContentLazyChanger = new LazyAction(nameof(LinkContentLazyChanger), TimeSpan.FromSeconds(5), LoggerFactory);

        }

        #region property

        public Guid NoteId { get; }
        public NoteContentKind ContentKind { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IDispatcherWapper DispatcherWapper { get; }
        IMainDatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

        [Obsolete]
        NoteLinkContentWatcher? LinkWatcher { get; set; }
        LazyAction LinkContentLazyChanger { get; }

        #endregion

        #region function

        public bool Exists()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectExistsContent(NoteId);
            }
        }

        void CreateNewContent(string content)
        {
            Logger.LogInformation("ノート空コンテンツ生成: {0}, {1}", NoteId, ContentKind);
            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    Content = content,
                    //TODO: あとあとかんがえよ
                    FilePath = string.Empty,
                    Encoding = Encoding.UTF8,
                    DelayTime = TimeSpan.Zero,
                    BufferSize = 0,
                    RefreshTime = TimeSpan.Zero,
                    IsEnabledRefresh = true,
                };
                dao.InsertNewContent(data, DatabaseCommonStatus.CreateCurrentAccount());
                commander.Commit();
            }
        }

        public string LoadRawContent()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFullContent(NoteId);
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

        [Obsolete]
        public NoteLinkContentData LoadLinkSetting()
        {
            if(ContentKind != NoteContentKind.Link) {
                throw new InvalidOperationException();
            }

            var converter = new NoteContentConverter(LoggerFactory);

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

        [Obsolete]
        public NoteLinkContentWatcher StartLinkWatch(NoteLinkContentData linkData)
        {
            if(LinkWatcher == null) {
                LinkWatcher = new NoteLinkContentWatcher(linkData, LoggerFactory);
            }
            LinkWatcher.Start();
            return LinkWatcher;
        }

        public void StopLinkWatch()
        {
            //if(LinkWatcher != null) {
            //    LinkWatcher.Stop();
            //}
        }

        public void ChangePlainContent(string? content)
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            MainDatabaseLazyWriter.Stock(c => {
                var dao = new NoteContentsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    Content = content,
                    //TODO: あとあとかんがえよ
                    FilePath = string.Empty,
                    Encoding = Encoding.UTF8,
                    DelayTime = TimeSpan.Zero,
                    BufferSize = 0,
                    RefreshTime = TimeSpan.Zero,
                    IsEnabledRefresh = true,
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
                var dao = new NoteContentsEntityDao(c, StatementLoader, c.Implementation, LoggerFactory);
                var data = new NoteContentData() {
                    NoteId = NoteId,
                    Content = content,
                };
                dao.UpdateContent(data, DatabaseCommonStatus.CreateCurrentAccount());
            }, UniqueKeyPool.Get());
        }

        [Obsolete]
        void DelayLinkContentChange(string content)
        {
            LinkContentLazyChanger.DelayAction(() => {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                LinkWatcher.Stop();
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

                using(var stream = new FileStream(LinkWatcher.File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) {
                    using(var writer = new StreamWriter(stream, LinkWatcher.Encoding)) {
                        writer.Write(content);
                    }
                }

                LinkWatcher.Start();
            });
        }

        [Obsolete]
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
            //if(LinkWatcher != null) {
            //    LinkWatcher.Dispose();
            //}
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
            LinkContentLazyChanger.SafeFlush();
            MainDatabaseLazyWriter.SafeFlush();
        }

        #endregion

    }
}
