using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppNotifyLogSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppNotifyLogSettingEntityDto: CommonDtoBase
        {
            #region property

            public bool IsVisible { get; set; }
            public string Position { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string Position => "Position";

            #endregion
        }

        #endregion

        public AppNotifyLogSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public SettingAppNotifyLogSettingData SelectSettingNotifyLogSetting()
        {
            var notifyLogPositionTransfer = new EnumTransfer<NotifyLogPosition>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppNotifyLogSettingEntityDto>(statement);
            var data = new SettingAppNotifyLogSettingData() {
                IsVisible = dto.IsVisible,
                Position = notifyLogPositionTransfer.ToEnum(dto.Position),
            };
            return data;
        }

        public void UpdateSettingNotifyLogSetting(SettingAppNotifyLogSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var notifyLogPositionTransfer = new EnumTransfer<NotifyLogPosition>();

            var statement = LoadStatement();
            var dto = new AppNotifyLogSettingEntityDto() {
                IsVisible = data.IsVisible,
                Position = notifyLogPositionTransfer.ToString(data.Position),
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }

        #endregion
    }
}
