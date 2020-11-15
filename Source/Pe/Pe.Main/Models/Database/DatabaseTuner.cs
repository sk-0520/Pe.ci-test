using System;
using System.Collections.Generic;
using System.Text;
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
        IIdFactory IdFactory { get; }
        IDatabaseAccessorPack AccessorPack { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        ILoggerFactory LoggerFactory { get; }
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
            var tuners = new TunerBase[] {

            };
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
