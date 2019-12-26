using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// マイグレーションの最後に実行される。
    /// </summary>
    public class Setupper_V_99_99_99_99 : SetupperBase
    {
        public Setupper_V_99_99_99_99(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase

        /// <summary>
        /// ここまでこない :-)
        /// </summary>
        public override Version Version { get; } = new Version(99, 99, 99, 99);

        public override void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(GetType()), dto);
        }

        public override void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteFileDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteTemporaryDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteTemporaryDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        #endregion
    }
}
