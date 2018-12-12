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
    /// マイグレーションの最後に実行される。
    /// </summary>
    public class Setup_V_99_99_99_99 : SetupBase
    {
        public Setup_V_99_99_99_99(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        { }

        #region SetupBase

        /// <summary>
        /// ここまでこない :-)
        /// </summary>
        public override Version Version { get; } = new Version(99, 99, 99, 99);

        public override void ExecuteMainDefine(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteMainManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteFileDefine(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteFileManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteTemporaryDefine(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
        }

        public override void ExecuteTemporaryManipulate(IDatabaseTransaction transaction, IReadOnlySetupDto dto)
        {
        }

        #endregion
    }
}
