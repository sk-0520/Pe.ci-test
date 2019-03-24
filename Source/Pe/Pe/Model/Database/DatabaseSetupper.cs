using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao;
using ContentTypeTextNet.Pe.Main.Model.Database.Setupper;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseSetupper
    {
        public DatabaseSetupper(IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            StatementLoader = statementLoader;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        IDatabaseStatementLoader StatementLoader { get; }
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
                    Logger.Debug("DDL");
                    ddl(commander, dto);

                    Logger.Debug("DML");
                    dml(commander, dto);

                    return true;
                });
                if(!result.Success) {
                    throw result.FailureValue;
                }
            } else {
                Logger.Debug("DDL");
                ddl(accessor, dto);

                Logger.Debug("DML");
                using(var tran = accessor.BeginTransaction()) {
                    dml(tran, dto);
                }
            }
        }

        void Execute(IDatabaseAccessorPack accessorPack, IReadOnlySetupDto dto, SetupperBase setupper)
        {
            Logger.Information($"SETUP: {setupper.Version}, {setupper.GetType().Name}");

            ExecuteCore(accessorPack.Main, dto, setupper.ExecuteMainDDL, setupper.ExecuteMainDML);
            ExecuteCore(accessorPack.File, dto, setupper.ExecuteFileDDL, setupper.ExecuteFileDML);
            ExecuteCore(accessorPack.Temporary, dto, setupper.ExecuteTemporaryDDL, setupper.ExecuteTemporaryDML);
        }

        public void Initialize(IDatabaseAccessorPack accessorPack)
        {
            Logger.Information("init");

            var dto = CreateSetupDto(new Version(0, 0, 0, 0));
            var setup = new Setupper_V_00_84_00_00(StatementLoader, Logger.Factory);

            Execute(accessorPack, dto, setup);
        }

        public void Migrate(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.Information("migrate");

            var dto = CreateSetupDto(lastVersion);

            var setuppers = new SetupperBase[] {
                // これ最後
                new Setupper_V_99_99_99_99(StatementLoader, Logger.Factory),
            };

            foreach(var setupper in setuppers) {
                if(lastVersion < setupper.Version) {
                    Execute(accessorPack, dto, setupper);
                }
            }
        }

        bool ExistsVersionTable(IDatabaseAccessor mainAccessor)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return mainAccessor.Query<bool>(sql, null, false).FirstOrDefault();
        }

        public Version GetLastVersion(IDatabaseAccessor mainAccessor)
        {
            if(!ExistsVersionTable(mainAccessor)) {
                Logger.Warning("not found: version table");
                return null;
            }

            var sql = StatementLoader.LoadStatementByCurrent();
            return mainAccessor.Query<Version>(sql, null, false).FirstOrDefault();
        }

        #endregion
    }
}
