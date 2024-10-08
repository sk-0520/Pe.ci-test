using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Adjust
{
    public abstract class AdjustBase
    {
        protected AdjustBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateLogger(GetType());

            var commonStatus = DatabaseCommonStatus.CreateCurrentAccount();
            AdjustCommonDtoSource = commonStatus.CreateCommonDtoMapping();
        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        private IDictionary<string, object> AdjustCommonDtoSource { get; }

        #endregion

        #region function

        protected IDictionary<string, object> GetCommonDto()
        {
            return new Dictionary<string, object>(AdjustCommonDtoSource);
        }

        protected abstract void AdjustImpl(IDatabaseContext context);

        public void Adjust(IDatabaseContext context)
        {
            AdjustImpl(context);
        }

        #endregion
    }
}
