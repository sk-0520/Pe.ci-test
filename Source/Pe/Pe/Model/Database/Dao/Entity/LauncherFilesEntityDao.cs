using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherFilesEntityDao : EntityDaoBase
    {
        public LauncherFilesEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
         : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string File { get; } = "File";
            public static string Option { get; } = "Option";
            public static string WorkDirectory { get; } = "WorkDirectory";

            #endregion
        }

        #endregion

        #region function

        public bool InsertSimple(Guid launcherItemId, LauncherPathExecuteData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = commonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;
            param[Column.File] = data.Path;
            param[Column.Option] = data.Option;
            param[Column.WorkDirectory] = data.WorkDirectoryPath;

            return Commander.Execute(statement, param) == 1;
        }

        #endregion
    }
}
