using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherRedoSuccessExitCodesEntityDao: EntityDaoBase
    {
        #region define

        private class LauncherRedoSuccessExitCodesDto: CommonDtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }
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

        public IEnumerable<int> SelectRedoSuccessExitCodes(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<int>(statement, parameter);
        }

        public int InsertSuccessExitCodes(Guid launcherItemId, IEnumerable<int> successExitCodes, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var parameter = new LauncherRedoSuccessExitCodesDto() {
                LauncherItemId = launcherItemId,
            };

            var insertCount = 0;
            foreach(var code in successExitCodes) {
                parameter.SuccessExitCode = code;
                commonStatus.WriteCreateTo(parameter);
                var result = Context.Execute(statement, parameter);
                insertCount += result;
            }

            return insertCount;
        }

        public int DeleteSuccessExitCodes(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Execute(statement, parameter);
        }


        #endregion
    }
}
