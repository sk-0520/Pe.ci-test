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
    public class LauncherMergeEnvVarsEntityDao : EntityDaoBase
    {
        public LauncherMergeEnvVarsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        LauncherMergeEnvironmentVariableItem ConvertFromDto(LauncherMergeEnvVarsEntityDto dto)
        {
            var data = new LauncherMergeEnvironmentVariableItem() {
                Name = dto.EnvName,
                Value = dto.EnvValue,
            };

            return data;
        }

        LauncherMergeEnvVarsEntityDto ConvertFromData(Guid launcherItemId, LauncherMergeEnvironmentVariableItem data, IDatabaseCommonStatus databaseCommonStatus)
        {
            var dto = new LauncherMergeEnvVarsEntityDto() {
                LauncherItemId = launcherItemId,
                EnvName = data.Name,
                EnvValue = data.Value,
            };

            databaseCommonStatus.WriteCreate(dto);

            return dto;
        }

        public IEnumerable<LauncherMergeEnvironmentVariableItem> SelectItems(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.EnvName);
            builder.AddSelect(Column.EnvValue);

            builder.AddValue(Column.LauncherItemId, launcherItemId);

            var result = Select<LauncherMergeEnvVarsEntityDto>(builder)
                .Select(i => ConvertFromDto(i))
            ;
            return result;
        }

        public void InsertMergeItems(Guid launcherItemId, IEnumerable<LauncherMergeEnvironmentVariableItem> items, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();

            foreach(var dto in items.Select(i => ConvertFromData(launcherItemId, i, databaseCommonStatus))) {
                Commander.Execute(statement, dto);
            }
        }

        public int DeleteMergeItemsByLauncherItemId(Guid launcherItemId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.LauncherItemId, launcherItemId);
            return ExecuteDelete(builder);
        }


        #endregion
    }
}
