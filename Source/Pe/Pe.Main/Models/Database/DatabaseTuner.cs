using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Tuner;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
        IIdFactory IdFactory { get; }
        /// <inheritdoc cref="IDatabaseAccessorPack"/>
        IDatabaseAccessorPack AccessorPack { get; }
        /// <inheritdoc cref="IDatabaseStatementLoader"/>
        IDatabaseStatementLoader StatementLoader { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }

        #endregion

        #region function

        void TuneImpl(IDatabaseAccessor accessor, IEnumerable<TunerBase> tuners)
        {
            using(var transaction = accessor.BeginTransaction()) {
                foreach(var tuner in tuners) {
                    tuner.Tune(transaction);
                }
                transaction.Commit();
            }
        }

        void TuneMain()
        {
            var tuners = new TunerBase[] {
                new Tuner_LauncherGroups(IdFactory, StatementLoader, LoggerFactory),
            };
            TuneImpl(AccessorPack.Main, tuners);
        }

        void TuneFile()
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
