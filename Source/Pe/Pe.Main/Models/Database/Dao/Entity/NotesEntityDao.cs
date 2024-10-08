using System;
using System.Collections.Generic;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NotesEntityDao: EntityDaoBase
    {
        #region define

        private sealed class NotesEntityDto: CommonDtoBase
        {
            #region property

            public NoteId NoteId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string ScreenName { get; set; } = string.Empty;
            public string LayoutKind { get; set; } = string.Empty;
            public bool IsVisible { get; set; }
            public FontId FontId { get; set; }
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

        private static class Column
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

        public NotesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private NoteData ConvertFromDto(NotesEntityDto dto)
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

        private NotesEntityDto ConvertFromData(NoteData data, IDatabaseCommonStatus commonStatus)
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

            commonStatus.WriteCommonTo(result);

            return result;
        }

        public IEnumerable<NoteId> SelectAllNoteIds()
        {
            var statement = LoadStatement();
            return Context.Query<NoteId>(statement);
        }

        //SelectExistsScreen
        public NoteData? SelectNote(NoteId noteId)
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

        public void InsertNewNote(NoteData noteData, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var param = ConvertFromData(noteData, commonStatus);

            Context.InsertSingle(statement, param);
        }

        public void UpdateScreen(NoteId noteId, string screenName, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = databaseCommonStatus.CreateCommonDtoMapping();
            parameter[Column.NoteId] = noteId;
            parameter[Column.ScreenName] = screenName;

            Context.UpdateByKey(statement, parameter);
        }

        public bool UpdateCompact(NoteId noteId, bool isCompact, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsCompact] = isCompact;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateTopmost(NoteId noteId, bool isTopmost, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsTopmost] = isTopmost;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateLock(NoteId noteId, bool isLocked, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsLocked] = isLocked;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateTextWrap(NoteId noteId, bool textWrap, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.TextWrap] = textWrap;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateTitle(NoteId noteId, string title, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.Title] = title;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateFontId(NoteId noteId, FontId fontId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.FontId] = fontId;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateForegroundColor(NoteId noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.ForegroundColor] = FromColor(color);

            return Context.UpdateByKeyOrNothing(statement, param);
        }
        public bool UpdateBackgroundColor(NoteId noteId, Color color, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.BackgroundColor] = FromColor(color);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateCaptionPosition(NoteId noteId, NoteCaptionPosition captionPosition, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteCaptionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.CaptionPosition] = noteCaptionPositionTransfer.ToString(captionPosition);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateContentKind(NoteId noteId, NoteContentKind contentKind, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteContentKindTransfer = new EnumTransfer<NoteContentKind>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.ContentKind] = noteContentKindTransfer.ToString(contentKind);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateLayoutKind(NoteId noteId, NoteLayoutKind layoutKind, IDatabaseCommonStatus databaseCommonStatus)
        {
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.LayoutKind] = noteLayoutKindTransfer.ToString(layoutKind);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateVisible(NoteId noteId, bool isVisible, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.IsVisible] = isVisible;

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public bool UpdateHiddenMode(NoteId noteId, NoteHiddenMode hiddenMode, IDatabaseCommonStatus databaseCommonStatus)
        {
            var hiddenModeTransfer = new EnumTransfer<NoteHiddenMode>();

            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.NoteId] = noteId;
            param[Column.HiddenMode] = hiddenModeTransfer.ToString(hiddenMode);

            return Context.UpdateByKeyOrNothing(statement, param);
        }

        public void DeleteNote(NoteId noteId)
        {
            var statement = LoadStatement();
            var parameter = new {
                NoteId = noteId,
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
