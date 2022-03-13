using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class PluginVersionChecksEntityDao: EntityDaoBase
    {
        #region define

        private class PluginVersionCheckDto: CommonDtoBase
        {
            #region property

            public Guid PluginId { get; set; }
            public long Sequence { get; set; }
            public string CheckUrl { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string PluginId { get; } = "PluginId";
            public static string Sequence { get; } = "Sequence";
            public static string CheckUrl { get; } = "CheckUrl";

            #endregion
        }

        #endregion

        public PluginVersionChecksEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public IEnumerable<string> SelectPluginVersionCheckUrls(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.SelectOrdered<string>(statement, parameter);
        }

        public void InsertPluginVersionCheckUrl(Guid pluginId, long sequence, string checkUrl, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var parameter = new PluginVersionCheckDto() {
                PluginId = pluginId,
                Sequence = sequence,
                CheckUrl = checkUrl,
            };
            databaseCommonStatus.WriteCommonTo(parameter);

            Context.InsertSingle(statement, parameter);
        }

        public int DeletePluginVersionChecks(Guid pluginId)
        {
            var statement = LoadStatement();
            var parameter = new {
                PluginId = pluginId,
            };

            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
