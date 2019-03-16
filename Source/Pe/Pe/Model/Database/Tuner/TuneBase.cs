using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Tune
{
    public abstract class TuneBase
    {
        public TuneBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function

        protected abstract void TuneImpl(IDatabaseCommander databaseCommander);

        public void Tune(IDatabaseCommander databaseCommander)
        {
            TuneImpl(databaseCommander);
        }

        #endregion
    }
}
