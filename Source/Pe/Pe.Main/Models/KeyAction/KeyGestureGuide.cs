using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public interface IKeyGestureGuide
    {
        #region proeprty

        #endregion

        #region function

        #endregion
    }

    internal class KeyGestureGuide: IKeyGestureGuide
    {
        public KeyGestureGuide(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region proeprty

        ILogger Logger { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        #endregion

        #region function

        internal string GetKeyMappingSting(KeyActionKind keyActionKind, string parameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
