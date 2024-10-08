using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherEnvVarsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherEnvVarsEntityDto: CommonDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string EnvName { get; set; } = string.Empty;
            public string EnvValue { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string EnvName { get; } = "EnvName";
            public static string EnvValue { get; } = "EnvValue";

            #endregion
        }

        #endregion

        public LauncherEnvVarsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherEnvironmentVariableData ConvertFromDto(LauncherEnvVarsEntityDto dto)
        {
            var data = new LauncherEnvironmentVariableData() {
                Name = dto.EnvName,
                Value = dto.EnvValue,
            };

            return data;
        }

        private LauncherEnvVarsEntityDto ConvertFromData(LauncherItemId launcherItemId, LauncherEnvironmentVariableData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new LauncherEnvVarsEntityDto() {
                LauncherItemId = launcherItemId,
                EnvName = data.Name,
                EnvValue = data.Value,
            };

            databaseCommonStatus.WriteCreateTo(dto);

            return dto;
        }

        public IEnumerable<LauncherEnvironmentVariableData> SelectEnvVarItems(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            var result = Context.Query<LauncherEnvVarsEntityDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
            return result;

        }

        public void InsertEnvVarItems(LauncherItemId launcherItemId, IEnumerable<LauncherEnvironmentVariableData> items, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();

            foreach(var dto in items.Select(i => ConvertFromData(launcherItemId, i, databaseCommonStatus))) {
                Context.InsertSingle(statement, dto);
            }
        }

        public int DeleteEnvVarItemsByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Delete(statement, parameter);
        }


        #endregion
    }
}
