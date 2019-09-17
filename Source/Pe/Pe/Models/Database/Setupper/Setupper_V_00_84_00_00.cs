using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// 誰が何と言おうと新生初期バージョン。
    /// </summary>
    public class Setupper_V_00_84_00_00 : SetupperBase
    {
        public Setupper_V_00_84_00_00(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        { }

        #region SetupBase

        public override Version Version { get; } = new Version(0, 84, 0, 0);

        public override void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(), dto);
        }

        public override void ExecuteFileDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteTemporaryDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        #endregion
    }
}
