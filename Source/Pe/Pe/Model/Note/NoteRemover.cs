using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteRemover : EntityRemoverBase
    {
        public NoteRemover(Guid noteId, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            NoteId = noteId;
        }

        #region property

        Guid NoteId { get; }

        #endregion

        #region EntityRemoverBase

        public override bool IsTarget(Pack pack)
        {
            switch(pack) {
                case Pack.Main:
                    return true;

                case Pack.File: // 悩み中
                case Pack.Temporary:
                    return false;

                default:
                    throw new NotImplementedException();
            }
        }

        protected override EntityRemoverResult RemoveMain(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var reuslt = new EntityRemoverResult(Pack.Main);

            var daoGroup = new EntityDeleteDaoGroup();
            daoGroup.Add(new NoteContentsEntityDao(commander, statementLoader, implementation, Logger.Factory), dao => dao.DeleteContents(NoteId));
            daoGroup.Add(new NoteLayoutsEntityDao(commander, statementLoader, implementation, Logger.Factory), dao => dao.DeleteLayouts(NoteId));
            daoGroup.Add(new NotesEntityDao(commander, statementLoader, implementation, Logger.Factory), dao => dao.DeleteNote(NoteId));

            foreach(var item in daoGroup.Execute()) {
                reuslt.Items.Add(item);
            }

            return reuslt;
        }

        protected override EntityRemoverResult RemoveFile(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            return new EntityRemoverResult(Pack.File);
        }

        protected override EntityRemoverResult RemoveTemporary(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
