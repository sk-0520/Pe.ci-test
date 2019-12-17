using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class AppExecuteSettingEntityDto : CommonDtoBase
    {
        #region property
        public bool Accepted { get; set; }
        public Version? FirstVersion { get; set; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime FirstTimestamp { get; set; }
        public Version? LastVersion { get; set; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime LastTimestamp { get; set; }
        public long ExecuteCount { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool SendUsageStatistics { get; set; }
        #endregion
    }

    public class AppExecuteSettingEntityDao : EntityDaoBase
    {
        public AppExecuteSettingEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        public SettingAppExecuteSettingData SelectSettingExecuteSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppExecuteSettingEntityDto>(statement);
            var result = new SettingAppExecuteSettingData() {
                SendUsageStatistics = dto.SendUsageStatistics,
                UserId = dto.UserId,
            };
            return result;
        }

        public bool UpdateSettingExecuteSetting(SettingAppExecuteSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppExecuteSettingEntityDto() {
                SendUsageStatistics = data.SendUsageStatistics,
                UserId = data.UserId,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }


        #endregion
    }
}
