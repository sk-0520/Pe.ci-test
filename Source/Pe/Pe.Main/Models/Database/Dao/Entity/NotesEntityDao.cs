using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NotesEntityDao: EntityDaoBase
    {
        #region define

        private class NotesEntityDto: CommonDtoBase
        {
            #region property

            public Guid NoteId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string ScreenName { get; set; } = string.Empty;
            public string LayoutKind { get; set; } = string.Empty;
            public bool IsVisible { get; set; }
            public Guid FontId { get; set; }
            public string ForegroundColor { get; set; } = string.Empty;
            public string BackgroundColor { get; set; } = string.Empty;
            public bool IsLocked { get; set; }
            public bool IsTopmost { get; set; }
            public bool IsCompact { get; set; }
            public bool TextWrap { get; set; }
            public string ContentKind { get; set; } = string.Empty;
            public string HiddenMode { get; set; } = string.Empty;
            public string CaptionPosition { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public NotesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
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
            public static string LayoutKind { get; } = "LayoutKind";
            public static string ScreenName { get; } = "ScreenName";
            public static string HiddenMode { get; } = "HiddenMode";
            public static string CaptionPosition { get; } = "CaptionPosition";

            #endregion
        }

        #endregion

        #region function

        NoteData ConvertFromDto(NotesEntityDto dto)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var contentKindTransfer = new EnumTransfer<NoteContentKind>();
            var hiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();
            var captionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

            var data = new NoteData() {
                NoteId = dto.NoteId,
                Title = dto.Title,
                ScreenName = dto.ScreenName,
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                IsVisible = dto.IsVisible,
                FontId = dto.FontId,
                ForegroundColor = ToColor(dto.ForegroundColor),
                BackgroundColor = ToColor(dto.BackgroundColor),
                IsLocked = dto.IsLocked,
                IsTopmost = dto.IsTopmost,
                IsCompact = dto.IsCompact,
                TextWrap = dto.TextWrap,
                ContentKind = contentKindTransfer.ToEnum(dto.ContentKind),
                HiddenMode = hiddenModeTransfer.ToEnum(dto.HiddenMode),
                CaptionPosition = captionPositionTransfer.ToEnum(dto.CaptionPosition),
            };

            return data;
        }

        NotesEntityDto ConvertFromData(NoteData data, IDatabaseCommonStatus commonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var contentKindTransfer = new EnumTransfer<NoteContentKind>();
            var hiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();
            var captionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

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
                HiddenMode = hiddenModeTransfer.ToString(data.HiddenMode),
                CaptionPosition = captionPositionTransfer.ToString(data.CaptionPosition),
            };

            commonStatus.WriteCommon(result);

            return result;

        }

        public IEnumerable<Guid> SelectAllNoteIds()
        {
            var statement = LoadStatement();
            return Context.Query<Guid>(statement);
        }

        //SelectExistsScreen
        public NoteData? SelectNote(Guid noteId)
        {
            var statement = LoadStatement();
            var param = new {
                NoteId = noteId,
            };
            var dto = Context.QueryFirstOrDefault<NotesEntityDto>(statement, param);
            if(dto == null) {
                return null;
            }

            return ConvertFromDto(dto);
        }

        public bool InsertNewNote(NoteData noteData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteData, commonStatus);
            return Context.Execute(statement, param) == 1;
        }

        internal bool InsertOldNote(NoteData noteData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteData, commonStatus);
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateScreen(Guid noteId, string screenName, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.NoteId] = noteId;
            parameter[Column.ScreenName] = screenName;
            return Context.Execute(statement, parameter) == 1;
        }

        public bool UpdateCompact(Guid noteId, bool isCompact, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsCompact] = isCompact;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateTopmost(Guid noteId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsTopmost] = isTopmost;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateLock(Guid noteId, bool isLocked, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsLocked] = isLocked;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateTextWrap(Guid noteId, bool textWrap, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.TextWrap] = textWrap;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateTitle(Guid noteId, string title, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.Title] = title;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateFontId(Guid noteId, Guid fontId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.FontId] = fontId;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateForegroundColor(Guid noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.ForegroundColor] = FromColor(color);
            return Context.Execute(statement, param) == 1;

        }
        public bool UpdateBackgroundColor(Guid noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.BackgroundColor] = FromColor(color);
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateCaptionPosition(Guid noteId, NoteCaptionPosition captionPosition, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteCaptionPositionTansfer = new EnumTransfer<NoteCaptionPosition>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.CaptionPosition] = noteCaptionPositionTansfer.ToString(captionPosition);
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateContentKind(Guid noteId, NoteContentKind contentKind, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTansfer = new EnumTransfer<NoteContentKind>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.ContentKind] = noteContentKindTansfer.ToString(contentKind);
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateLayoutKind(Guid noteId, NoteLayoutKind layoutKind, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTansfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.LayoutKind] = noteLayoutKindTansfer.ToString(layoutKind);
            return Context.Execute(statement, param) == 1;
        }


        public bool UpdateVisible(Guid noteId, bool isVisible, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsVisible] = isVisible;
            return Context.Execute(statement, param) == 1;
        }

        public bool UpdateHiddenMode(Guid noteId, NoteHiddenMode hiddenMode, IDatabaseCommonStatus databaseCommonStatus)
        {
            var hiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.HiddenMode] = hiddenModeTransfer.ToString(hiddenMode);
            return Context.Execute(statement, param) == 1;
        }

        public int DeleteNote(Guid noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
