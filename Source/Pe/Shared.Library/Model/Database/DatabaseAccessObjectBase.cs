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
        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILogger logger)
        {
            Commander = commander;
            StatementLoader = statementLoader;
            Logger = logger;
        }

        public DatabaseAccessObjectBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(commander, statementLoader, loggerFactory.CreateCurrentClass())
        { }

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
