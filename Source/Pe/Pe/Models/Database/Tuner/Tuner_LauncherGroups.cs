using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Tuner
{
    public class Tuner_LauncherGroups : TunerBase
    {
        public Tuner_LauncherGroups(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(statementLoader, loggerFactory)
        {
            IdFactory = idFactory;
        }

        #region property

        IIdFactory IdFactory { get; }

        #endregion

        #region TunerBase

        bool ExistsRows(IDatabaseCommander commander)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return commander.QuerySingle<bool>(statement, GetCommonDto());
        }

        int InsertEmptyGroup(IDatabaseCommander commander)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            var param = GetCommonDto();
            param["LauncherGroupId"] = IdFactory.CreateLauncherGroupId();
            param["Name"] = "@name";
            return commander.Execute(statement, param);
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
