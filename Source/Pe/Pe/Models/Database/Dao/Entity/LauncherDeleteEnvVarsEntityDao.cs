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
    public class LauncherDeleteEnvVarsEntityDao : EntityDaoBase
    {
        public LauncherDeleteEnvVarsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string EnvName { get; } = "EnvName";

            #endregion
        }

        #endregion

        #region function

        public IEnumerable<string> SelectItems(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.EnvName);

            builder.AddValue(Column.LauncherItemId, launcherItemId);

            var result = Select<string>(builder);
            return result;
        }

        public void InsertDeleteItems(Guid launcherItemId, IEnumerable<string> items, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();

            foreach(var item in items) {
                var param = databaseCommonStatus.CreateCommonDtoMapping();
                param[Column.LauncherItemId] = launcherItemId;
                param[Column.EnvName] = item;
                Commander.Execute(statement, param);
            }
        }

        public int DeleteDeleteItemsByLauncherItemId(Guid launcherItemId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.LauncherItemId, launcherItemId);
            return ExecuteDelete(builder);
        }

        #endregion
    }
}
