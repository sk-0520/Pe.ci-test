using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using ContentTypeTextNet.Pe.Main.Models.Database.Setupper;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    public class DatabaseSetupper
    {
        public DatabaseSetupper(IIdFactory idFactory, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            StatementLoader = statementLoader;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property
        IIdFactory IdFactory { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IDatabaseCommonStatus CommonStatus { get; } = DatabaseCommonStatus.CreateCurrentAccount();

        #endregion

        #region function

        SetupDto CreateSetupDto(Version lastVersion)
        {
            var result = new SetupDto() {
                LastVersion = lastVersion,
                ExecuteVersion = Assembly.GetExecutingAssembly().GetName().Version,
            };
            CommonStatus.WriteCommon(result);

            return result;
        }

        void ExecuteCore(IDatabaseAccessor accessor, IReadOnlySetupDto dto, Action<IDatabaseCommander, IReadOnlySetupDto> ddl, Action<IDatabaseCommander, IReadOnlySetupDto> dml)
        {
            if(accessor.DatabaseFactory.CreateImplementation().SupportedTransactionDDL) {
                var result = accessor.Batch(commander => {
                    Logger.LogInformation("DDL");
                    ddl(commander, dto);

                    Logger.LogInformation("DML");
                    dml(commander, dto);

                    return true;
                });
                if(!result.Success) {
                    // この時点で FailureValue は例外が入っている
                    throw result.FailureValue!;
                }
            } else {
                Logger.LogInformation("DDL");
                ddl(accessor, dto);

                Logger.LogInformation("DML");
                using(var tran = accessor.BeginTransaction()) {
                    dml(tran, dto);
                }
            }
        }

        void Execute(IDatabaseAccessorPack accessorPack, IReadOnlySetupDto dto, SetupperBase setupper)
        {
            Logger.LogInformation("SETUP: {0}, {1}", setupper.Version, setupper.GetType().Name);

            ExecuteCore(accessorPack.Main, dto, setupper.ExecuteMainDDL, setupper.ExecuteMainDML);
            ExecuteCore(accessorPack.File, dto, setupper.ExecuteFileDDL, setupper.ExecuteFileDML);
            ExecuteCore(accessorPack.Temporary, dto, setupper.ExecuteTemporaryDDL, setupper.ExecuteTemporaryDML);
        }

        public void Initialize(IDatabaseAccessorPack accessorPack)
        {
            Logger.LogInformation("init");

            var dto = CreateSetupDto(new Version(0, 0, 0, 0));
            var setup = new Setupper_V_00_84_00_00(IdFactory, StatementLoader, LoggerFactory);

            Execute(accessorPack, dto, setup);
        }

        public void Migrate(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.LogInformation("migrate");

            var dto = CreateSetupDto(lastVersion);

            var setuppers = new SetupperBase[] {
                // これ最後
                new Setupper_V_99_99_99_99(IdFactory, StatementLoader, LoggerFactory),
            };

            foreach(var setupper in setuppers) {
                if(lastVersion < setupper.Version) {
                    Execute(accessorPack, dto, setupper);
                }
            }
        }

        bool ExistsExecuteTable(IDatabaseAccessor mainAccessor)
        {
            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return mainAccessor.Query<bool>(statement, null, false).FirstOrDefault();
        }

        public Version? GetLastVersion(IDatabaseAccessor mainAccessor)
        {
            if(!ExistsExecuteTable(mainAccessor)) {
                Logger.LogWarning("not found: version table");
                return null;
            }

            var statement = StatementLoader.LoadStatementByCurrent(GetType());
            return mainAccessor.Query<Version>(statement, null, false).FirstOrDefault();
        }

        #endregion
    }
}
