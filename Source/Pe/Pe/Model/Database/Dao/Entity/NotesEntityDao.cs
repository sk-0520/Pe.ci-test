using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Note;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class NotesEntityDao : EntityDaoBase
    {
        public NotesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string NoteId { get; } = "NoteId";

            #endregion
        }

        #endregion

        #region function

        NoteData ConvertFromDto(NotesEntityDto dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> SelectAllNoteIds()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<Guid>(sql);
        }

        //SelectExistsScreen
        public NoteData SelectNote(Guid noteId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
            };
            var dto = Commander.QueryFirstOrDefault<NotesEntityDto>(sql, param);
            if(dto == null) {
                return null;
            }

            return ConvertFromDto(dto);
        }

        #endregion
    }
}
