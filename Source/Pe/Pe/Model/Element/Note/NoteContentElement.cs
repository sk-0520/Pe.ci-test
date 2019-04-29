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
    public class NoteContentElement: ElementBase
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

            MainDatabaseLazyWriter = new DatabaseLazyWriter(MainDatabaseBarrier, Constants.Config.NoteMainDatabaseLazyWriterWaitTime, this);
        }

        #region property

        public Guid NoteId { get; }
        public NoteContentKind ContentKind { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        DatabaseLazyWriter MainDatabaseLazyWriter { get; }

        #endregion

        #region function

        public bool Exists()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new NoteContentsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectExistsContent(NoteId, ContentKind);
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            // この子は遅延読み込みさせたいのです
        }

        #endregion
    }
}
