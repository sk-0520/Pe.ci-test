using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
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
                ForegroundColor = ToColor(dto.BackgroundColor),
                BackgroundColor = ToColor(dto.ForegroundColor),
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
                ForegroundColor = FromColor(data.BackgroundColor),
                BackgroundColor = FromColor(data.ForegroundColor),
                IsTopmost = data.IsTopmost,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
