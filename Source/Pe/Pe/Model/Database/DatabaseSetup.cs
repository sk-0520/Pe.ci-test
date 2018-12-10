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
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao;
using ContentTypeTextNet.Pe.Main.Model.Database.Setup;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseSetup : DisposerBase
    {
        public DatabaseSetup(DirectoryInfo baseDirectory, ILogFactory logFactory)
        {
            Logger = logFactory.CreateCurrentClass();
            StatementLoader = new ApplicationDatabaseStatementLoader(baseDirectory, TimeSpan.Zero, Logger.Factory);
        }

        #region property

        IDatabaseStatementLoader StatementLoader { get; }
        ILogger Logger { get; }
        DatabaseCommonStatus CommonStatus { get; } = new DatabaseCommonStatus();

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

        void ExecuteCore(IDatabaseAccessor accessor, IReadOnlySetupDto dto, Action<IDatabaseCommander, IReadOnlySetupDto> ddl, Action<IDatabaseTransaction, IReadOnlySetupDto> dml)
        {
            Logger.Debug("DDL");
            ddl(accessor, dto);

            Logger.Debug("DML");
            using(var tran = accessor.BeginTransaction()) {
                dml(tran, dto);
            }
        }

        void Execute(IDatabaseAccessorPack accessorPack, IReadOnlySetupDto dto, SetupBase setup)
        {
            Logger.Information($"SETUP: {setup.Version}, {setup.GetType().Name}");

            ExecuteCore(accessorPack.Main, dto, setup.ExecuteMainDefine, setup.ExecuteMainManipulate);
            ExecuteCore(accessorPack.File, dto, setup.ExecuteFileDefine, setup.ExecuteFileManipulate);
            ExecuteCore(accessorPack.Temporary, dto, setup.ExecuteTemporaryDefine, setup.ExecuteTemporaryManipulate);
        }

        public void Initialize(IDatabaseAccessorPack accessorPack)
        {
            Logger.Information("init");

            var dto = CreateSetupDto(new Version(0, 0, 0, 0));
            var setup = new Setup_V_00_84_00_00(StatementLoader, Logger.Factory);

            Execute(accessorPack, dto, setup);
        }

        public void Migrate(IDatabaseAccessorPack accessorPack, Version lastVersion)
        {
            Logger.Information("migrate");

            var dto = CreateSetupDto(lastVersion);

            var setups = new SetupBase[] {
                // これ最後
                new Setup_V_99_99_99_99(StatementLoader, Logger.Factory),
            };

            foreach(var setup in setups) {
                if(lastVersion < setup.Version) {
                    Execute(accessorPack, dto, setup);
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
