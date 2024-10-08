using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteEntityEraser: EntityEraserBase
    {
        public NoteEntityEraser(NoteId noteId, IDatabaseContextsPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextsPack, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        public NoteEntityEraser(NoteId noteId, IDatabaseContexts mainContexts, IDatabaseContexts fileContexts, IDatabaseContexts temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContexts, fileContexts, temporaryContexts, statementLoader, loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        private NoteId NoteId { get; }

        #endregion

        #region EntityEraserBase

        protected override void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var noteContentsEntityDao = new NoteContentsEntityDao(context, statementLoader, implementation, LoggerFactory);
            var noteLayoutsEntityDao = new NoteLayoutsEntityDao(context, statementLoader, implementation, LoggerFactory);
            var notesEntityDao = new NotesEntityDao(context, statementLoader, implementation, LoggerFactory);

            noteContentsEntityDao.DeleteContents(NoteId);
            noteLayoutsEntityDao.DeleteLayouts(NoteId);
            notesEntityDao.DeleteNote(NoteId);
        }

        protected override void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            //TODO: 添付ファイル(そもそも添付ファイル自体実装してない)
        }

        protected override void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
