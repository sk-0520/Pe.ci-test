using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Tune;
using ContentTypeTextNet.Pe.Main.Model.Database.Tuner;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseTuner
    {
        public DatabaseTuner(IIdFactory idFactory, IDatabaseAccessorPack accessorPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            AccessorPack = accessorPack;
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property
        IIdFactory IdFactory { get; }
        IDatabaseAccessorPack AccessorPack { get; }
        IDatabaseStatementLoader StatementLoader { get; }
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
                new Tuner_LauncherGroups(IdFactory, StatementLoader, Logger.Factory),
            };
            TuneImpl(AccessorPack.Main, tuners);
        }

        void TuneFile()
        {
            var tuners = new TunerBase[] {

            };
            TuneImpl(AccessorPack.File, tuners);
        }

        public void Tune()
        {
            TuneMain();
            TuneFile();
        }

        #endregion
    }
}
