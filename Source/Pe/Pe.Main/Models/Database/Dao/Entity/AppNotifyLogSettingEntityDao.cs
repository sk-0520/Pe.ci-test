using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppNotifyLogSettingEntityDao : EntityDaoBase
    {
        #region define

        private class AppNotifyLogSettingEntityDto: CommonDtoBase
        {
            #region property

            public bool IsVisible { get; set; }
            public string Position { get; set; } = string.Empty;

            #endregion
        }

        #endregion

        public AppNotifyLogSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string Position => "Position";

            #endregion
        }

        #endregion

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

        public bool UpdateSettingNotifyLogSetting(SettingAppNotifyLogSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var notifyLogPositionTransfer = new EnumTransfer<NotifyLogPosition>();

            var statement = LoadStatement();
            var dto = new AppNotifyLogSettingEntityDto() {
                IsVisible = data.IsVisible,
                Position = notifyLogPositionTransfer.ToString(data.Position),
            };
            commonStatus.WriteCommon(dto);
            return Context.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
