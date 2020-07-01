using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal sealed class LauncherIconRefreshLoader: LauncherIconLoader
    {
        public LauncherIconRefreshLoader(Guid launcherItemId, IconBox iconBox, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, iconBox, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, new CurrentDispatcherWrapper(), loggerFactory)
        { }

        #region property

        #endregion

        #region function

        public Task LoadAndSaveAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }


    public class LauncherIconRefresher: ICronExecutor
    {
        public LauncherIconRefresher(TimeSpan refreshTime, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            RefreshTime = refreshTime;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        TimeSpan RefreshTime { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        IReadOnlyList<LauncherIconStatus> GetUpdateTartget(Guid launcherItemIs)
        {
            using var commander = FileDatabaseBarrier.WaitRead();
            var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
            var status = launcherItemIconStatusEntityDao.SelectLauncherItemIconStatus(launcherItemIs)
                .Where(i => RefreshTime < (DateTime.UtcNow - i.LastUpdatedTimestamp))
                .ToList()
            ;
            return status;
        }

        private Task UpdateTargetAsync(Guid launcherItemId, LauncherIconStatus target, CancellationToken cancellationToken)
        {
            var loader = new LauncherIconRefreshLoader(launcherItemId, target.IconBox, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            return loader.LoadAndSaveAsync();
        }


        #endregion

        #region ICronExecutor

        /// <inheritdoc cref="ICronExecutor.ExecuteAsync(CancellationToken)"/>
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var allLauncherItemIds = MainDatabaseBarrier.ReadData(c => {
                var dao = new LauncherItemsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return dao.SelectAllLauncherItemIds().ToList();
            });

            return Task.Run(async () => {
                foreach(var launcherItemId in allLauncherItemIds) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var targets = GetUpdateTartget(launcherItemId);

                    foreach(var target in targets) {
                        cancellationToken.ThrowIfCancellationRequested();
                        await UpdateTargetAsync(launcherItemId, target, cancellationToken);
                    }
                }
            });
        }

        #endregion
    }
}
