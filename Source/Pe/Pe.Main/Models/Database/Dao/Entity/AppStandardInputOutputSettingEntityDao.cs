using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppStandardInputOutputSettingEntityDao: EntityDaoBase
    {
        #region define

        private class AppStandardInputOutputSettingEntityDto: CommonDtoBase
        {
            #region property

            public FontId FontId { get; set; }
            public string OutputForegroundColor { get; set; } = string.Empty;
            public string OutputBackgroundColor { get; set; } = string.Empty;
            public string ErrorForegroundColor { get; set; } = string.Empty;
            public string ErrorBackgroundColor { get; set; } = string.Empty;
            public bool IsTopmost { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public AppStandardInputOutputSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public FontId SelectStandardInputOutputSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<FontId>(statement);
        }

        public SettingAppStandardInputOutputSettingData SelectSettingStandardInputOutputSetting()
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppStandardInputOutputSettingEntityDto>(statement);
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

        public void UpdateSettingStandardInputOutputSetting(SettingAppStandardInputOutputSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var noteCreateTitleKindTransfer = new EnumTransfer<NoteCreateTitleKind>();
            var noteLayoutKindTransfer = new EnumTransfer<NoteLayoutKind>();

            var statement = LoadStatement();
            var parameter = new AppStandardInputOutputSettingEntityDto() {
                FontId = data.FontId,
                OutputForegroundColor = FromColor(data.OutputForegroundColor),
                OutputBackgroundColor = FromColor(data.OutputBackgroundColor),
                ErrorForegroundColor = FromColor(data.ErrorForegroundColor),
                ErrorBackgroundColor = FromColor(data.ErrorBackgroundColor),
                IsTopmost = data.IsTopmost,
            };
            commonStatus.WriteCommonTo(parameter);
            Context.UpdateByKey(statement, parameter);
        }

        #endregion
    }
}
