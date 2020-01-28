using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class AppPlatformSettingEntityDto : CommonDtoBase
    {
        #region property

        public bool SuppressSystemIdle { get; set; }
        public bool SupportHorizontalScroll { get; set; }


        #endregion
    }
    public class AppPlatformSettingEntityDao: EntityDaoBase
    {
        public AppPlatformSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppPlatformSettingData SelectSettingPlatformSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppPlatformSettingEntityDto>(statement);
            var data = new SettingAppPlatformSettingData() {
                SupportHorizontalScroll = dto.SupportHorizontalScroll,
                SuppressSystemIdle =  dto.SuppressSystemIdle,
            };
            return data;
        }

        public bool UpdateSettingPlatformSetting(SettingAppPlatformSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppPlatformSettingEntityDto() {
                SupportHorizontalScroll = data.SupportHorizontalScroll,
                SuppressSystemIdle = data.SuppressSystemIdle,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }



        #endregion

    }
}
