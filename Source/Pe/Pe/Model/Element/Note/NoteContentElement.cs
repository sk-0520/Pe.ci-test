using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Element.Note
{
    public class NoteContentElement: ElementBase, IFlush
    {
        #region variable
        #endregion

        public NoteContentElement(Guid noteId, NoteContentKind contentKind, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = noteId;
            ContentKind = contentKind;

            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.NoteContentMainDatabaseLazyWriterWaitTime, this);
        }

        #region property

        public Guid NoteId { get; }
        public NoteContentKind ContentKind { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        DatabaseLazyWriter MainDatabaseLazyWriter { get; }
        UniqueKeyPool UniqueKeyPool { get; } = new UniqueKeyPool();

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

        public string LoadPlainContent()
        {
            if(ContentKind != NoteContentKind.Plain) {
                throw new InvalidOperationException();
            }

            if(!Exists()) {
                // 存在しなければこのタイミングで生成
                var content = string.Empty;
                CreateNewContent(content);
                // 作ったやつを返すだけなので別に。
                return content;
            }

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectFullContent(NoteId, ContentKind);
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

        #endregion

        #region ElementBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
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
        }

        #endregion
    }
}
