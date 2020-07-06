using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Setupper
{
    /// <summary>
    /// プラグインが仲間入り。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Setupper_V_00_99_010: SetupperBase
    {
        public Setupper_V_00_99_010(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(idFactory, statementLoader, loggerFactory)
        { }

        #region SetupBase


        /// <inheritdoc cref="SetupperBase.Version"/>
        public override Version Version { get; } =
#if DEBUG || BERA
            new Version(0, 99, 8)
#else
# error !! 0.99.010 !!
#endif
        ;

        public override void ExecuteMainDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteMainDML(IDatabaseCommander commander, IReadOnlySetupDto dto)
        { }

        public override void ExecuteFileDDL(IDatabaseCommander commander, IReadOnlySetupDto dto)
        {
            ExecuteStatement(commander, StatementLoader.LoadStatementByCurrent(GetType()), dto);
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
