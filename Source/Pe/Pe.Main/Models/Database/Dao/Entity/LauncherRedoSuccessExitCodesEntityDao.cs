using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherRedoSuccessExitCodesEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherRedoSuccessExitCodesDto: CommonDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public int SuccessExitCode { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        public LauncherRedoSuccessExitCodesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public IEnumerable<int> SelectRedoSuccessExitCodes(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<int>(statement, parameter);
        }

        public void InsertSuccessExitCodes(LauncherItemId launcherItemId, IEnumerable<int> successExitCodes, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = new LauncherRedoSuccessExitCodesDto() {
                LauncherItemId = launcherItemId,
            };

            foreach(var code in successExitCodes) {
                parameter.SuccessExitCode = code;
                commonStatus.WriteCreateTo(parameter);
                Context.InsertSingle(statement, parameter);
            }
        }

        public int DeleteSuccessExitCodes(LauncherItemId launcherItemId)
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
