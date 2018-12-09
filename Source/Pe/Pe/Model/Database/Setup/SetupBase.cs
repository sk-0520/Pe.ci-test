using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Setup
{
    public abstract class SetupBase
    {
        public SetupBase(IDatabaseStatementLoader statementLoader, ILogFactory logFactory)
        {
            StatementLoader = statementLoader;
            Logger = logFactory.CreateCurrentClass();
        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        protected abstract Version Version { get; }

        #endregion

        #region function

        public abstract void ExecuteDefine(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto);

        #endregion
    }
}
