using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
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
                Kind = environmentVariableKindTransfer.ToEnum(dto.Kind),
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
