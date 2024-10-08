using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Adjust
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
    public class Adjust_LauncherGroups: AdjustBase
    {
        public Adjust_LauncherGroups(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        {
            IdFactory = idFactory;
        }

        #region property

        private IIdFactory IdFactory { get; }

        #endregion

        #region TunerBase

        private bool ExistsRows(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return context.QuerySingle<bool>(statement, GetCommonDto());
        }

        private int InsertEmptyGroup(IDatabaseContext context)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var param = GetCommonDto();
            param["LauncherGroupId"] = IdFactory.CreateLauncherGroupId();
            param["Name"] = Properties.Resources.String_NewEmptyGroupName;
            return context.Execute(statement, param);
        }

        protected override void AdjustImpl(IDatabaseContext context)
        {
            if(!ExistsRows(context)) {
                InsertEmptyGroup(context);
            }
        }

        #endregion
    }
}
