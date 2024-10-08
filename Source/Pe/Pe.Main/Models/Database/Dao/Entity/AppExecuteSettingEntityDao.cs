using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class AppExecuteSettingEntityDao: EntityDaoBase
    {
        #region define

        private sealed class AppExecuteSettingEntityDto: CommonDtoBase
        {
            #region property
            public bool Accepted { get; set; }
            public Version? FirstVersion { get; set; }
            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime FirstTimestamp { get; set; }
            public Version? LastVersion { get; set; }
            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime LastTimestamp { get; set; }
            public long ExecuteCount { get; set; }
            public string UserId { get; set; } = string.Empty;
            public bool IsEnabledTelemetry { get; set; }
            #endregion
        }

        private sealed class AppGeneralFirstEntityDto: CommonDtoBase
        {
            #region property

            public Version FirstVersion { get; set; } = new Version();

            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime FirstTimestamp { get; set; }


            #endregion
        }

        private static class Column
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

        public AppExecuteSettingEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public SettingAppExecuteSettingData SelectSettingExecuteSetting()
        {
            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppExecuteSettingEntityDto>(statement);
            var result = new SettingAppExecuteSettingData() {
                IsEnabledTelemetry = dto.IsEnabledTelemetry,
                UserId = dto.UserId,
            };
            return result;
        }

        public AppGeneralFirstData SelectFirstData()
        {
            var statement = LoadStatement();
            var dto = Context.QueryFirst<AppGeneralFirstEntityDto>(statement);
            return new AppGeneralFirstData() {
                FirstExecuteVersion = dto.FirstVersion,
                FirstExecuteTimestamp = dto.FirstTimestamp,
            };
        }

        public void UpdateSettingExecuteSetting(SettingAppExecuteSettingData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = new AppExecuteSettingEntityDto() {
                IsEnabledTelemetry = data.IsEnabledTelemetry,
                UserId = data.UserId,
            };
            commonStatus.WriteCommonTo(dto);
            Context.UpdateByKey(statement, dto);
        }

        public void UpdateExecuteSettingAcceptInput(string userId, bool isEnabledTelemetry, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.UserId] = userId;
            parameter[Column.IsEnabledTelemetry] = isEnabledTelemetry;

            Context.UpdateByKey(statement, parameter);
        }

        #endregion
    }
}
