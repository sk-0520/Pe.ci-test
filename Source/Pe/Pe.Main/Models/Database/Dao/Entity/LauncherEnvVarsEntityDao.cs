using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherEnvVarsEntityDao : EntityDaoBase
    {
        public LauncherEnvVarsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string EnvName { get; } = "EnvName";
            public static string EnvValue { get; } = "EnvValue";

            #endregion
        }

        #endregion

        #region function

        LauncherEnvironmentVariableData ConvertFromDto(LauncherEnvVarsEntityDto dto)
        {
            var data = new LauncherEnvironmentVariableData() {
                Name = dto.EnvName,
                Value = dto.EnvValue,
            };

            return data;
        }

        LauncherEnvVarsEntityDto ConvertFromData(Guid launcherItemId, LauncherEnvironmentVariableData data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new LauncherEnvVarsEntityDto() {
                LauncherItemId = launcherItemId,
                EnvName = data.Name,
                EnvValue = data.Value,
            };

            databaseCommonStatus.WriteCreate(dto);

            return dto;
        }

        public IEnumerable<LauncherEnvironmentVariableData> SelectEnvVarItems(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.EnvName);
            builder.AddSelect(Column.EnvValue);

            builder.AddValueParameter(Column.LauncherItemId, launcherItemId);

            var result = Select<LauncherEnvVarsEntityDto>(builder)
                .Select(i => ConvertFromDto(i))
            ;
            return result;
        }

        public void InsertEnvVarItems(Guid launcherItemId, IEnumerable<LauncherEnvironmentVariableData> items, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();

            foreach(var dto in items.Select(i => ConvertFromData(launcherItemId, i, databaseCommonStatus))) {
                Commander.Execute(statement, dto);
            }
        }

        public int DeleteEnvVarItemsByLauncherItemId(Guid launcherItemId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.LauncherItemId, launcherItemId);
            return ExecuteDelete(builder);
        }


        #endregion
    }
}
