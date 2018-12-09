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
    /// <summary>
    /// 誰が何と言おうと新生初期バージョン。
    /// </summary>
    public class Setup_V_00_84_00_00 : SetupBase
    {
        public Setup_V_00_84_00_00(IDatabaseStatementLoader statementLoader, ILogFactory logFactory)
            : base(statementLoader, logFactory)
        { }

        #region SetupBase

        protected override Version Version => throw new NotImplementedException();

        public override void ExecuteDefine(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            var multiSql = StatementLoader.LoadStatementByCurrent();
        }

        public override void ExecuteManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
