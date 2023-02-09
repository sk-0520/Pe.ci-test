using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class NoteFilesEntityDao: EntityDaoBase
    {
        #region define

        private static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public NoteFilesEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function



        #endregion
    }
}
