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
    public class AppUpdateSettingEntityDao : EntityDaoBase
    {
        public AppUpdateSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppUpdateSettingData SelectSettingUpdateSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppUpdateSettingEntityDto>(statement);
            var result = new SettingAppUpdateSettingData() {
                IsCheckReleaseVersion = dto.CheckReleaseVersion,
                IsCheckRcVersion = dto.CheckRcVersion,
            };
            return result;
        }


        public bool UpdateSettingUpdateSetting(SettingAppUpdateSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppUpdateSettingEntityDto() {
                CheckReleaseVersion = data.IsCheckReleaseVersion,
                CheckRcVersion = data.IsCheckRcVersion,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }


        #endregion
    }
}
