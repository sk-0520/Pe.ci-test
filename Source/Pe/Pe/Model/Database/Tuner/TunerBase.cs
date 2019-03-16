using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Tune
{
    public abstract class TunerBase
    {
        #region define

        class TuneCommonDtoImpl: CommonDtoBase
        { }

        #endregion

        public TunerBase(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateTartget(GetType());

            var commonStatus = DatabaseCommonStatus.CreateUser();
            var tuneCommonDto = new TuneCommonDtoImpl();
            commonStatus.WriteCommon(tuneCommonDto);
            foreach(var propertyInfo in tuneCommonDto.GetType().GetProperties()) {
                var value = propertyInfo.GetValue(tuneCommonDto);
                TuneCommonDtoSource.Add(propertyInfo.Name, value);
            }

        }

        #region property

        protected IDatabaseStatementLoader StatementLoader { get; }
        protected ILogger Logger { get; }

        Dictionary<string, object> TuneCommonDtoSource { get; } = new Dictionary<string, object>();
        #endregion

        #region function

        protected IDictionary<string, object> GetCommonDto()
        {
            return new Dictionary<string, object>(TuneCommonDtoSource);
        }

        protected abstract void TuneImpl(IDatabaseCommander commander);

        public void Tune(IDatabaseCommander commander)
        {
            TuneImpl(commander);
        }

        #endregion
    }
}
