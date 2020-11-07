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
    internal class AppLauncherToolbarSettingEntityDto: CommonDtoBase
    {
        #region property
        public string ContentDropMode { get; set; } = string.Empty;
        public string GroupMenuPosition { get; set; } = string.Empty;
        #endregion
    }


    public class AppLauncherToolbarSettingEntityDao: EntityDaoBase
    {
        public AppLauncherToolbarSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public Guid SelectAppLauncherToolbarSettingFontId()
        {
            var statement = LoadStatement();
            return Context.QueryFirst<Guid>(statement);
        }

        public AppLauncherToolbarSettingData SelectSettingLauncherToolbarSetting()
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();

            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppLauncherToolbarSettingEntityDto>(statement);
            var result = new AppLauncherToolbarSettingData() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToEnum( dto.ContentDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToEnum( dto.GroupMenuPosition),
            };
            return result;
        }

        public bool UpdateSettingLauncherToolbarSetting(AppLauncherToolbarSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var launcherToolbarContentDropModeTransfer = new EnumTransfer<LauncherToolbarContentDropMode>();
            var launcherGroupPositionTransfer = new EnumTransfer<LauncherGroupPosition>();

            var statement = LoadStatement();
            var dto = new AppLauncherToolbarSettingEntityDto() {
                ContentDropMode = launcherToolbarContentDropModeTransfer.ToString(data.ContentDropMode),
                GroupMenuPosition = launcherGroupPositionTransfer.ToString(data.GroupMenuPosition),
            };
            commonStatus.WriteCommon(dto);
            return Context.Execute(statement, dto) == 1;
        }


        #endregion
    }
}
