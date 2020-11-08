using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginValuesEntityDao: EntityDaoBase
    {
        public PluginValuesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";

            #endregion
        }

        #endregion

        #region function

        public int DeletePluginValuesByPluginId(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
