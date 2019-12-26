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
    public class AppStandardInputOutputSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string OutputForegroundColor { get; set; } = string.Empty;
        public string OutputBackgroundColor { get; set; } = string.Empty;
        public string ErrorForegroundColor { get; set; } = string.Empty;
        public string ErrorBackgroundColor { get; set; } = string.Empty;
        public bool IsTopmost { get; set; }

        #endregion
    }

    public class AppStandardInputOutputSettingEntityDao : EntityDaoBase
    {
        public AppStandardInputOutputSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public Guid SelectStandardInputOutputSettingFontId()
        {
            var statement = LoadStatement();
            return Commander.QueryFirst<Guid>(statement);
        }

        public SettingAppStandardInputOutputSettingData SelectSettingStandardInputOutputSetting()
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppStandardInputOutputSettingEntityDto>(statement);
            var data = new SettingAppStandardInputOutputSettingData() {
                FontId = dto.FontId,
                OutputForegroundColor = ToColor(dto.OutputForegroundColor),
                OutputBackgroundColor = ToColor(dto.OutputBackgroundColor),
                ErrorForegroundColor = ToColor(dto.ErrorForegroundColor),
                ErrorBackgroundColor = ToColor(dto.ErrorBackgroundColor),
                IsTopmost = dto.IsTopmost,
            };
            return data;
        }

        public bool UpdateSettingStandardInputOutputSetting(SettingAppStandardInputOutputSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var dto = new AppStandardInputOutputSettingEntityDto() {
                FontId = data.FontId,
                OutputForegroundColor = FromColor(data.OutputForegroundColor),
                OutputBackgroundColor = FromColor(data.OutputBackgroundColor),
                ErrorForegroundColor = FromColor(data.ErrorForegroundColor),
                ErrorBackgroundColor = FromColor(data.ErrorBackgroundColor),
                IsTopmost = data.IsTopmost,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }



        #endregion
    }
}
