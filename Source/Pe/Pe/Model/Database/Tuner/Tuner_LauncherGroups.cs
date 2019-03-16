using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Database.Tune;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Tuner
{
    public class Tuner_LauncherGroups: TunerBase
    {
        public Tuner_LauncherGroups(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        { }

        #region TunerBase

        bool ExistsRows(IDatabaseCommander commander)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return commander.QuerySingle<bool>(sql, GetCommonDto());
        }

        int InsertEmptyGroup(IDatabaseCommander commander)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = GetCommonDto();
            param["Name"] = "@name";
            return commander.Execute(sql, param);
        }

        protected override void TuneImpl(IDatabaseCommander commander)
        {
            if(!ExistsRows(commander)) {
                InsertEmptyGroup(commander);
            }
        }

        #endregion
    }
}
