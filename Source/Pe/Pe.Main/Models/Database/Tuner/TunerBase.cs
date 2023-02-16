using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Tuner
{
    public abstract class TunerBase
    {
        protected TunerBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateLogger(GetType());

            var commonStatus = DatabaseCommonStatus.CreateCurrentAccount();
            TuneCommonDtoSource = commonStatus.CreateCommonDtoMapping();
        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        private IDictionary<string, object> TuneCommonDtoSource { get; }

        #endregion

        #region function

        protected IDictionary<string, object> GetCommonDto()
        {
            return new Dictionary<string, object>(TuneCommonDtoSource);
        }

        protected abstract void TuneImpl(IDatabaseContext context);

        public void Tune(IDatabaseContext context)
        {
            TuneImpl(context);
        }

        #endregion
    }
}
