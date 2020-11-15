using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppNoteSettingEntityDao : EntityDaoBase
    {
        #region define

        private class AppNoteSettingEntityDto: CommonDtoBase
        {
            #region property

            public Guid FontId { get; set; }
            public string TitleKind { get; set; } = string.Empty;
            public string LayoutKind { get; set; } = string.Empty;
            public string ForegroundColor { get; set; } = string.Empty;
            public string BackgroundColor { get; set; } = string.Empty;
            public bool IsTopmost { get; set; }
            public string CaptionPosition { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public AppNoteSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        public Guid SelectAppNoteSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<Guid>(statement);
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

        public bool UpdateSettingNoteSetting(SettingAppNoteSettingData data, IDatabaseCommonStatus commonStatus)
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
            commonStatus.WriteCommon(dto);
            return Context.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
