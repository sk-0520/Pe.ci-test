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

        public abstract Version Version { get; }

        #endregion

        #region function

        public abstract void ExecuteMainDefine(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteMainManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto);

        public abstract void ExecuteFileDefine(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteFileManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto);

        public abstract void ExecuteTemporaryDefine(IDatabaseCommander commander, IReadOnlySetupDto dto);
        public abstract void ExecuteTemporaryManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto);

        protected IEnumerable<KeyValuePair<string, string>> SplitMultiSql(string sql)
        {
            return null;
        }

        protected void ExecuteSql(IDatabaseCommander commander, string sql)
        {
            var pairs = SplitMultiSql(sql);
            throw new NotImplementedException();
        }


        #endregion
    }
}
