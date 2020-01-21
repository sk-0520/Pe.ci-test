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
    internal class AppNoteSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string TitleKind { get; set; } = string.Empty;
        public string LayoutKind { get; set; } = string.Empty;
        public string ForegroundColor { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public bool IsTopmost { get; set; }


        #endregion
    }

    public class AppNoteSettingEntityDao : EntityDaoBase
    {
        public AppNoteSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
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
            return Commander.QueryFirst<Guid>(statement);
        }

        public SettingAppNoteSettingData SelectSettingNoteSetting()
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppNoteSettingEntityDto>(statement);
            var data = new SettingAppNoteSettingData() {
                FontId = dto.FontId,
                TitleKind = noteCreateTitleKindTransfer.ToEnum(dto.TitleKind),
                LayoutKind = noteLayoutKindTransfer.ToEnum(dto.LayoutKind),
                ForegroundColor = ToColor(dto.ForegroundColor),
                BackgroundColor = ToColor(dto.BackgroundColor),
                IsTopmost = dto.IsTopmost,
            };
            return data;
        }

        public bool UpdateSettingNoteSetting(SettingAppNoteSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var dto = new AppNoteSettingEntityDto() {
                FontId = data.FontId,
                TitleKind = noteCreateTitleKindTransfer.ToString(data.TitleKind),
                LayoutKind = noteLayoutKindTransfer.ToString(data.LayoutKind),
                ForegroundColor = FromColor(data.ForegroundColor),
                BackgroundColor = FromColor(data.BackgroundColor),
                IsTopmost = data.IsTopmost,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
