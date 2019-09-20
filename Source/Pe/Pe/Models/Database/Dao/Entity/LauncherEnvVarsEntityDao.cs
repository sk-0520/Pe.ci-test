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
            public static string Kind { get; } = "Kind";
            public static string EnvValue { get; } = "EnvValue";

            #endregion
        }

        #endregion

        #region function

        LauncherEnvironmentVariableItem ConvertFromDto(LauncherEnvVarsEntityDto dto)
        {
            var environmentVariableKindTransfer = new  EnumTransfer<LauncherEnvironmentVariableKind>();

            var data = new LauncherEnvironmentVariableItem() {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                Kind = environmentVariableKindTransfer.ToEnum(dto.Kind),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                Name = dto.EnvName,
                Value = dto.EnvValue,
            };

            return data;
        }

        public IEnumerable<LauncherEnvironmentVariableItem> SelectItems(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.EnvName);
            builder.AddSelect(Column.Kind);
            builder.AddSelect(Column.EnvValue);

            builder.AddValue(Column.LauncherItemId, launcherItemId);

            var result = Select<LauncherEnvVarsEntityDto>(builder)
                .Select(i => ConvertFromDto(i))
            ;
            return result;
        }

        #endregion
    }
}
