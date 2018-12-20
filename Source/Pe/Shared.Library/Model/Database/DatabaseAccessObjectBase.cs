using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    /// <summary>
    /// だお！
    /// </summary>
    public abstract class DatabaseAccessObjectBase
    {
        DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader)
        {
            Commander = commander;
            StatementLoader = statementLoader;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogger logger)
        {
            Logger = logger;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(commander, statementLoader)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        /// <summary>
        /// データベース文。
        /// </summary>
        protected IDatabaseCommander Commander { get; }
        /// <summary>
        /// データベース文の読み込みストア。
        /// </summary>
        protected IDatabaseStatementLoader StatementLoader { get; }
        /// <summary>
        /// ログ出力担当。
        /// </summary>
        protected ILogger Logger { get; }

        #endregion

        #region function
        #endregion
    }
}
