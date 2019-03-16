using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Setupper
{
    /// <summary>
    /// マイグレーションの最後に実行される。
    /// </summary>
    public class Setupper_V_99_99_99_99 : SetupperBase
    {
        public Setupper_V_99_99_99_99(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
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
            ExecuteSql(commander, StatementLoader.LoadStatementByCurrent(), dto);
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
