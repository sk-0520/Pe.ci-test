using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppLauncherToolbarSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppLauncherToolbarSettingEntityDto: CommonDtoBase
        {
            #region property
            public string ContentDropMode { get; set; } = string.Empty;
            public string ShortcutDropMode { get; set; } = string.Empty;
            public string GroupMenuPosition { get; set; } = string.Empty;
            #endregion
        }

        private static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public AppLauncherToolbarSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public FontId SelectAppLauncherToolbarSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<FontId>(statement);
        }

        public AppLauncherToolbarSettingData SelectSettingLauncherToolbarSetting()
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherToolbarShortcutDropModeTransfer = new EnumTransfer<LauncherToolbarShortcutDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppLauncherToolbarSettingEntityDto>(statement);
            var result = new AppLauncherToolbarSettingData() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToEnum(dto.ContentDropMode),
                ShortcutDropMode = launcherToolbarShortcutDropModeTransfer.ToEnum(dto.ShortcutDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToEnum(dto.GroupMenuPosition),
            };
            return result;
        }

        public void UpdateSettingLauncherToolbarSetting(AppLauncherToolbarSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherToolbarShortcutDropModeTransfer = new EnumTransfer<LauncherToolbarShortcutDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();

            var statement = LoadStatement();
            var dto = new AppLauncherToolbarSettingEntityDto() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToString(data.ContentDropMode),
                ShortcutDropMode = launcherToolbarShortcutDropModeTransfer.ToString(data.ShortcutDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToString(data.GroupMenuPosition),
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }


        #endregion
    }
}
