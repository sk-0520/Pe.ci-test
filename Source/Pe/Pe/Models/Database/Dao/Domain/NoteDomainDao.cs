using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class NoteDomainDao : DomainDaoBase
    {
        public NoteDomainDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        {}

        #region function

        NoteScreenData ConvertFromDto(NoteScreenRowDto dto)
        {
            var data = new NoteScreenData() {
                NoteId = dto.NoteId,
                ScreenName = dto.ScreenName,
                X = dto.ScreenX,
                Y = dto.ScreenY,
                Height = dto.ScreenHeight,
                Width = dto.ScreenWidth,
            };

            return data;
        }

        public IEnumerable<NoteScreenData> SelectNoteScreens(Guid noteId)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
            };
            return Commander.Query<NoteScreenRowDto>(statement, param)
                .Select(i => ConvertFromDto(i))
            ;
        }

        #endregion
    }
}
