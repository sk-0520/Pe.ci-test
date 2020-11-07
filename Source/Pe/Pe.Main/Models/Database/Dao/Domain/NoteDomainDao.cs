using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    internal class NoteScreenRowDto : RowDtoBase
    {
        #region property
        public Guid NoteId { get; set; }

        public string ScreenName { get; set; } = string.Empty;
        [PixelKind(Px.Device)]
        public long ScreenX { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenY { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenWidth { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenHeight { get; set; }

        #endregion
    }

    public class NoteDomainDao : DomainDaoBase
    {
        public NoteDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
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
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            return Context.Query<NoteScreenRowDto>(statement, param)
                .Select(i => ConvertFromDto(i))
            ;
        }

        #endregion
    }
}
