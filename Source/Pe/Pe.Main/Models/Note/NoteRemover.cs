using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteRemover: EntityRemoverBase
    {
        public NoteRemover(NoteId noteId, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        private NoteId NoteId { get; }

        #endregion

        #region EntityRemoverBase

        public override bool IsTarget(Pack pack)
        {
            switch(pack) {
                case Pack.Main:
                    return true;

                case Pack.Large: // 悩み中
                case Pack.Temporary:
                    return false;

                default:
                    throw new NotImplementedException();
            }
        }

        protected override EntityRemoverResult RemoveMain(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var reuslt = new EntityRemoverResult(Pack.Main);

            var daoGroup = new EntityDeleteDaoGroup();
            daoGroup.Add(new NoteContentsEntityDao(context, statementLoader, implementation, LoggerFactory), dao => dao.DeleteContents(NoteId));
            daoGroup.Add(new NoteLayoutsEntityDao(context, statementLoader, implementation, LoggerFactory), dao => dao.DeleteLayouts(NoteId));
            daoGroup.Add(new NotesEntityDao(context, statementLoader, implementation, LoggerFactory), dao => { dao.DeleteNote(NoteId); return 1; });

            foreach(var item in daoGroup.Execute()) {
                reuslt.Items.Add(item);
            }

            return reuslt;
        }

        protected override EntityRemoverResult RemoveFile(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            return new EntityRemoverResult(Pack.Large);
        }

        protected override EntityRemoverResult RemoveTemporary(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
