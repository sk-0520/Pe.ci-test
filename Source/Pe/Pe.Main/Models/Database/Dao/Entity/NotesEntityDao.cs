using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Models.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
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
            public static string IsVisible { get; } = "IsVisible";
            public static string IsCompact { get; } = "IsCompact";
            public static string IsTopmost { get; } = "IsTopmost";
            public static string IsLocked { get; } = "IsLocked";
            public static string TextWrap { get; } = "TextWrap";
            public static string Title { get; } = "Title";
            public static string FontId { get; } = "FontId";
            public static string ForegroundColor { get; } = "ForegroundColor";
            public static string BackgroundColor { get; } = "BackgroundColor";
            public static string ContentKind { get; } = "ContentKind";


            #endregion
        }

        #endregion

        #region function

        NoteData ConvertFromDto(NotesEntityDto dto)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var contentKindTransfer = new EnumTransfer<NoteContentKind>();

            var data = new NoteData() {
                NoteId = dto.NoteId,
                Title = dto.Title,
                ScreenName = dto.ScreenName,
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                IsVisible = dto.IsVisible,
                FontId = dto.FontId,
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                ForegroundColor = ToColor(dto.ForegroundColor),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                BackgroundColor = ToColor(dto.BackgroundColor),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                IsLocked = dto.IsLocked,
                IsTopmost = dto.IsTopmost,
                IsCompact = dto.IsCompact,
                TextWrap = dto.TextWrap,
                ContentKind = contentKindTransfer.ToEnum(dto.ContentKind),
            };

            return data;
        }

        NotesEntityDto ConvertFromData(NoteData data, IDatabaseCommonStatus commonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var contentKindTransfer = new EnumTransfer<NoteContentKind>();

            var result = new NotesEntityDto() {
                NoteId = data.NoteId,
                Title = data.Title,
                ScreenName = data.ScreenName,
                LayoutKind = noteLayoutKindTransfer.ToString(data.LayoutKind),
                IsVisible = data.IsVisible,
                FontId = data.FontId,
                ForegroundColor = FromColor(data.ForegroundColor),
                BackgroundColor = FromColor(data.BackgroundColor),
                IsLocked = data.IsLocked,
                IsTopmost = data.IsTopmost,
                IsCompact = data.IsCompact,
                TextWrap = data.TextWrap,
                ContentKind = contentKindTransfer.ToString(data.ContentKind),
            };

            commonStatus.WriteCommon(result);

            return result;

        }

        public IEnumerable<Guid> SelectAllNoteIds()
        {
            var statement = LoadStatement();
            return Commander.Query<Guid>(statement);
        }

        //SelectExistsScreen
        public NoteData? SelectNote(Guid noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            var dto = Commander.QueryFirstOrDefault<NotesEntityDto>(statement, param);
            if(dto == null) {
                return null;
            }

            return ConvertFromDto(dto);
        }

        public bool InsertNewNote(NoteData noteData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteData, commonStatus);
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateCompact(Guid noteId, bool isCompact, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsCompact] = isCompact;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateTopmost(Guid noteId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsTopmost] = isTopmost;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateLock(Guid noteId, bool isLocked, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.IsLocked, isLocked);
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateTextWrap(Guid noteId, bool textWrap, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.TextWrap, textWrap);
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateTitle(Guid noteId, string title, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.Title] = title;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateFontId(Guid noteId, Guid fontId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.FontId] = fontId;
            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateForegroundColor(Guid noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.ForegroundColor, FromColor(color));
            return ExecuteUpdate(builder) == 1;
        }
        public bool UpdateBackgroundColor(Guid noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.BackgroundColor, FromColor(color));
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateContentKind(Guid noteId, NoteContentKind contentKind, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTansfer = new EnumTransfer<NoteContentKind>();

            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.ContentKind, noteContentKindTansfer.ToString(contentKind));
            return ExecuteUpdate(builder) == 1;
        }

        public bool UpdateVisible(Guid noteId, bool isVisible, IDatabaseCommonStatus databaseCommonStatus)
        {
            var builder = CreateUpdateBuilder(databaseCommonStatus);
            builder.AddKey(Column.NoteId, noteId);
            builder.AddValueParameter(Column.IsVisible, isVisible);
            return ExecuteUpdate(builder) == 1;
        }

        public int DeleteNote(Guid noteId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.NoteId, noteId);
            return ExecuteDelete(builder);
        }


        #endregion
    }
}
