using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

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
            public static string LayoutKind { get; } = "LayoutKind";
            public static string X { get; } = "X";
            public static string Y { get; } = "Y";
            public static string Width { get; } = "Width";
            public static string Height { get; } = "Height";

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

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToText(layoutKind),
            };
            return Commander.QueryFirstOrDefault<NoteLayoutData>(statement, param);
        }
        public bool SelectExistsLayout(Guid noteId, NoteLayoutKind layoutKind)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                NoteId = noteId,
                LayoutKind = noteLayoutKindTransfer.ToText(layoutKind),
            };
            return Commander.QuerySingle<bool>(statement, param);
        }

        public bool InsertLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateLayout(NoteLayoutData noteLayout, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = ConvertFromData(noteLayout, databaseCommonStatus);
            return Commander.Execute(statement, param) == 1;
        }
        public bool UpdatePickupLayout(NoteLayoutData noteLayout, bool isEnabledLocation, bool isEnabledSize, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteLayout.NoteId;
            param[Column.LayoutKind] = noteLayoutKindTransfer.ToText(noteLayout.LayoutKind);
            param[Column.X] = noteLayout.X;
            param[Column.Y] = noteLayout.Y;
            param[Column.Width] = noteLayout.Width;
            param[Column.Height] = noteLayout.Height;
            param[nameof(isEnabledLocation)] = isEnabledLocation;
            param[nameof(isEnabledSize)] = isEnabledSize;
            return Commander.Execute(statement, param) == 1;
        }

        #endregion
    }
}
