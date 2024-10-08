using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppNoteSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppNoteSettingEntityDto: CommonDtoBase
        {
            #region property

            public FontId FontId { get; set; }
            public string TitleKind { get; set; } = string.Empty;
            public string LayoutKind { get; set; } = string.Empty;
            public string ForegroundColor { get; set; } = string.Empty;
            public string BackgroundColor { get; set; } = string.Empty;
            public bool IsTopmost { get; set; }
            public string CaptionPosition { get; set; } = string.Empty;

            #endregion
        }

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public AppNoteSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public FontId SelectAppNoteSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<FontId>(statement);
        }

        public SettingAppNoteSettingData SelectSettingNoteSetting()
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var noteCaptionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppNoteSettingEntityDto>(statement);
            var data = new SettingAppNoteSettingData() {
                FontId = dto.FontId,
                TitleKind = noteCreateTitleKindTransfer.ToEnum(dto.TitleKind),
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                ForegroundColor = ToColor(dto.ForegroundColor),
                BackgroundColor = ToColor(dto.BackgroundColor),
                IsTopmost = dto.IsTopmost,
                CaptionPosition = noteCaptionPositionTransfer.ToEnum(dto.CaptionPosition),
            };
            return data;
        }

        public void UpdateSettingNoteSetting(SettingAppNoteSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();
            var noteCaptionPositionTransfer = new EnumTransfer<NoteCaptionPosition>();

            var statement = LoadStatement();
            var dto = new AppNoteSettingEntityDto() {
                FontId = data.FontId,
                TitleKind = noteCreateTitleKindTransfer.ToString(data.TitleKind),
                LayoutKind = noteLayoutKindTransfer.ToString(data.LayoutKind),
                ForegroundColor = FromColor(data.ForegroundColor),
                BackgroundColor = FromColor(data.BackgroundColor),
                IsTopmost = data.IsTopmost,
                CaptionPosition = noteCaptionPositionTransfer.ToString(data.CaptionPosition),
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }

        #endregion
    }
}
