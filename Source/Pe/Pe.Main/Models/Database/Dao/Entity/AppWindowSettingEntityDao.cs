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
    internal class AppWindowSettingEntityDto : CommonDtoBase
    {
        #region property

        public bool IsEnabled { get; set; }
        public long Count { get; set; }
        public TimeSpan Interval { get; set; }

        #endregion
    }

    public class AppWindowSettingEntityDao : EntityDaoBase
    {
        public AppWindowSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppWindowSettingData SelectSettingWindowSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppWindowSettingEntityDto>(statement);
            var result = new SettingAppWindowSettingData() {
                IsEnabled = dto.IsEnabled,
                Count = ToInt(dto.Count),
                Interval = dto.Interval,
            };
            return result;
        }


        public bool UpdateSettingWindowSetting(SettingAppWindowSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppWindowSettingEntityDto() {
                IsEnabled = data.IsEnabled,
                Count = data.Count,
                Interval = data.Interval,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }



        #endregion
    }
}
