using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model.Data
{
    public abstract class DataAccessModelBase
    {
        public DataAccessModelBase(IDatabaseCommander databaseCommander, ILogger logger)
        {
            DatabaseCommander = databaseCommander;
            Logger = logger;
        }

        public DataAccessModelBase(IDatabaseCommander databaseCommander, ILoggerFactory loggerFactory)
        {
            DatabaseCommander = databaseCommander;
            Logger = loggerFactory.CreateLogger(GetType());
        }


        #region property

        protected IDatabaseCommander DatabaseCommander { get; }
        protected ILogger Logger { get; }

        #endregion
    }
}
