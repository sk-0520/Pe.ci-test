using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppCommandSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppCommandSettingEntityDto: CommonDtoBase
        {
            #region property

            public FontId FontId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double Width { get; set; }
            public TimeSpan HideWaitTime { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string Width { get; } = "Width";

            #endregion
        }

        #endregion

        public AppCommandSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public FontId SelectCommandSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<FontId>(statement);
        }


        public SettingAppCommandSettingData SelectSettingCommandSetting()
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppCommandSettingEntityDto>(statement);
            var result = new SettingAppCommandSettingData() {
                FontId = dto.FontId,
                IconBox = iconBoxTransfer.ToEnum(dto.IconBox),
                Width = dto.Width,
                HideWaitTime = dto.HideWaitTime,
            };
            return result;
        }

        public void UpdateSettingCommandSetting(SettingAppCommandSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = new AppCommandSettingEntityDto() {
                FontId = data.FontId,
                IconBox = iconBoxTransfer.ToString(data.IconBox),
                Width = data.Width,
                HideWaitTime = data.HideWaitTime,
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }

        public void UpdateCommandSettingWidth(double width, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.Width] = width;
            Context.UpdateByKey(statement, parameter);
        }

        #endregion
    }
}
