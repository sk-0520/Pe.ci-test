using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Note;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class NoteLayoutsEntityDao : EntityDaoBase
    {
        public NoteLayoutsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        NoteLayoutsEntityDto ConvertFromData(NoteLayoutData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var dto = new NoteLayoutsEntityDto() {
                NoteId = data.NoteId,
                LayoutKind = noteLayoutKindTransfer.ToText(data.LayoutKind),
                X = data.X,
                Y = data.Y,
                Width = data.Width,
                Height = data.Height,
            };

            databaseCommonStatus.WriteCommon(dto);

            return dto;
        }

        NoteLayoutData ConvertFromDto(NoteLayoutsEntityDto dto)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var data = new NoteLayoutData() {
                NoteId = dto.NoteId,
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                X = dto.X,
                Y = dto.Y,
                Width = dto.Width,
                Height = dto.Height,
            };

            return data;
        }

        public NoteLayoutData SelectLayout(Guid noteId, NoteLayoutKind layoutKind)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToText(layoutKind),
            };
            return Commander.QueryFirstOrDefault<NoteLayoutData>(sql, param);
        }
        public bool SelectExistsLayout(Guid noteId, NoteLayoutKind layoutKind)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToText(layoutKind),
            };
            return Commander.QuerySingle<bool>(sql, param);
        }

        public bool InsertLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Commander.Execute(sql, param) == 1;
        }

        public bool UpdateLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Commander.Execute(sql, param) == 1;
        }

        #endregion
    }
}
