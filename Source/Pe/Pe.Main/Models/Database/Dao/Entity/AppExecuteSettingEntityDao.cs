using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
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
        public bool IsEnabledTelemetry { get; set; }
        #endregion
    }

    internal class AppGeneralFirstEntityDto : CommonDtoBase
    {
        #region property

        public Version FirstVersion { get; set; } = new Version();

        [Timestamp(DateTimeKind.Utc)]
        public DateTime FirstTimestamp { get; set; }


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
            public static string UserId => "UserId";
            public static string IsEnabledTelemetry => "IsEnabledTelemetry";

            public static string FirstVersion => "FirstVersion";
            public static string FirstTimestamp => "FirstTimestamp";
            public static string ExecuteCount => "ExecuteCount";

            #endregion
        }

        #endregion

        #region function

        public SettingAppExecuteSettingData SelectSettingExecuteSetting()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppExecuteSettingEntityDto>(statement);
            var result = new SettingAppExecuteSettingData() {
                IsEnabledTelemetry = dto.IsEnabledTelemetry,
                UserId = dto.UserId,
            };
            return result;
        }

        public AppGeneralFirstData SelectFirstData()
        {
            var statement = LoadStatement();
            var dto = Commander.QueryFirst<AppGeneralFirstEntityDto>(statement);
            return new AppGeneralFirstData() {
                FirstExecuteVersion = dto.FirstVersion,
                FirstExecuteTimestamp = dto.FirstTimestamp,
            };
        }

        public bool UpdateSettingExecuteSetting(SettingAppExecuteSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppExecuteSettingEntityDto() {
                IsEnabledTelemetry = data.IsEnabledTelemetry,
                UserId = data.UserId,
            };
            commonStatus.WriteCommon(dto);
            return Commander.Execute(statement, dto) == 1;
        }

        public bool UpdateExecuteSettingAcceptInput(string userId, bool isEnabledTelemetry, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.UserId] = userId;
            parameter[Column.IsEnabledTelemetry] = isEnabledTelemetry;

            return Commander.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
