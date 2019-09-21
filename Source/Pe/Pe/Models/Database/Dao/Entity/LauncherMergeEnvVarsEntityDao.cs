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

        #endregion
    }
}
