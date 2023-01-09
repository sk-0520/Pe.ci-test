using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal class LauncherEntityEraser: EntityEraserBase
    {
        public LauncherEntityEraser(LauncherItemId launcherItemId, IDatabaseContextsPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(contextsPack, statementLoader, loggerFactory)
        {
            LauncherItemId = launcherItemId;
        }

        public LauncherEntityEraser(LauncherItemId launcherItemId, IDatabaseContexts mainContexts, IDatabaseContexts fileContexts, IDatabaseContexts temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainContexts, fileContexts, temporaryContexts, statementLoader, loggerFactory)
        {
            LauncherItemId = launcherItemId;
        }

        #region property

        private LauncherItemId LauncherItemId { get; }

        #endregion

        #region EntityEraserBase

        protected override void ExecuteMain(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(LauncherItemId);

            var launcherFilesEntityDao = new LauncherFilesEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherFilesEntityDao.DeleteFileByLauncherItemId(LauncherItemId);

            var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(LauncherItemId);

            var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(LauncherItemId);

            var launcherTagsEntityDao = new LauncherTagsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherTagsEntityDao.DeleteTagByLauncherItemId(LauncherItemId);

            var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(LauncherItemId);

            var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(LauncherItemId);

            var launcherItemsEntityDao = new LauncherItemsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherItemsEntityDao.DeleteLauncherItem(LauncherItemId);
        }

        protected override void ExecuteFile(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
            var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherItemIconsEntityDao.DeleteAllSizeImageBinary(LauncherItemId);

            var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(context, statementLoader, implementation, LoggerFactory);
            launcherItemIconStatusEntityDao.DeleteAllSizeLauncherItemIconState(LauncherItemId);
        }

        protected override void ExecuteTemporary(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        { }

        #endregion
    }
}
