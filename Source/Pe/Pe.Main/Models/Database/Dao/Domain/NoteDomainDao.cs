using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class NoteDomainDao: DomainDaoBase
    {
        #region define

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
        private sealed class NoteScreenRowDto: RowDtoBase
        {
            #region property
            public NoteId NoteId { get; set; }

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

        #endregion

        public NoteDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private NoteScreenData ConvertFromDto(NoteScreenRowDto dto)
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

        public IEnumerable<NoteScreenData> SelectNoteScreens(NoteId noteId)
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
