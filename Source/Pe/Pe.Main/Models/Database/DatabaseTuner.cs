using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Tuner;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    public class DatabaseTuner
    {
        public DatabaseTuner(IIdFactory idFactory, IDatabaseAccessorPack accessorPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            AccessorPack = accessorPack;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="IIdFactory"/>
        private IIdFactory IdFactory { get; }
        /// <inheritdoc cref="IDatabaseAccessorPack"/>
        private IDatabaseAccessorPack AccessorPack { get; }
        /// <inheritdoc cref="IDatabaseStatementLoader"/>
        private IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        private void TuneImpl(IDatabaseAccessor accessor, IEnumerable<TunerBase> tuners)
        {
            using(var transaction = accessor.BeginTransaction()) {
                foreach(var tuner in tuners) {
                    tuner.Tune(transaction);
                }
                transaction.Commit();
            }
        }

        private void TuneMain()
        {
            var tuners = new TunerBase[] {
                new Tuner_LauncherGroups(IdFactory, StatementLoader, LoggerFactory),
            };
            TuneImpl(AccessorPack.Main, tuners);
        }

        private void TuneFile()
        {
            var tuners = System.Array.Empty<TunerBase>();
            TuneImpl(AccessorPack.Large, tuners);
        }

        public void Tune()
        {
            TuneMain();
            TuneFile();
        }

        #endregion
    }
}
