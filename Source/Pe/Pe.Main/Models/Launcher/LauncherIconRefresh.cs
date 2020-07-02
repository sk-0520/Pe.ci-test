using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal sealed class LauncherIconRefreshLoader: LauncherIconLoader
    {
        public LauncherIconRefreshLoader(Guid launcherItemId, IconBox iconBox, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, iconBox, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, null, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        public Task LoadAndSaveAsync(CancellationToken cancellationToken)
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            Logger.LogDebug("ランチャーアイテムのアイコン取得開始: {0}, [{1}], [{2}], {3}", IconBox, launcherIconData.Path, launcherIconData.Icon, LauncherItemId);

            GetImageAsync(launcherIconData, true, cancellationToken).ContinueWith(t => {
                try {
                    if(t.IsCompletedSuccessfully) {
                        var image = t.Result;
                        if(image != null) {
                            // 内部でシャットダウン
                            Logger.LogDebug("ランチャーアイテムのアイコン更新開始: {0}, {1}", IconBox, LauncherItemId);
                            return SaveImageAsync(image);
                        } else {
                            Logger.LogDebug("ランチャーアイテムのアイコン更新失敗: {0}, {1}", IconBox, LauncherItemId);
                        }
                    } else {
                        Logger.LogDebug("ランチャーアイテムのアイコン取得停止: {0}, {1}", IconBox, LauncherItemId);
                    }
                    return Task.CompletedTask;
                } finally {
                    Dispatcher.CurrentDispatcher.InvokeShutdown();
                }
            });

            return Task.CompletedTask;
        }

        #endregion
    }


    public class LauncherIconRefresher: ICronExecutor
    {
        public LauncherIconRefresher(TimeSpan refreshTime, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            RefreshTime = refreshTime;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        TimeSpan RefreshTime { get; }

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        #endregion

        #region function

        bool IsNeedUpdate(LauncherIconStatus launcherIconStatus, [DateTimeKind(DateTimeKind.Utc)] DateTime dateTime)
        {
            return RefreshTime < (dateTime - launcherIconStatus.LastUpdatedTimestamp);
        }

        IReadOnlyList<LauncherIconStatus> GetUpdateTartget(Guid launcherItemIs)
        {
            using var commander = FileDatabaseBarrier.WaitRead();
            var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
            var status = launcherItemIconStatusEntityDao.SelectLauncherItemIconAllSizeStatus(launcherItemIs)
                .Where(i => IsNeedUpdate(i, DateTime.UtcNow))
                .ToList()
            ;
            return status;
        }

        private Task UpdateTargetAsync(Guid launcherItemId, LauncherIconStatus target, CancellationToken cancellationToken)
        {
            using(var commander = FileDatabaseBarrier.WaitRead()) {
                var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                var nowStatus = launcherItemIconStatusEntityDao.SelectLauncherItemIconSingleSizeStatus(launcherItemId, target.IconBox);
                if(nowStatus == null) {
                    // 対象アイテムは破棄された
                    return Task.CompletedTask;
                }
                if(!IsNeedUpdate(nowStatus, DateTime.UtcNow)) {
                    // 対象アイテムはすでに更新された(待っている間にユーザー操作で変更された)
                    return Task.CompletedTask;
                }
            }

            var loader = new LauncherIconRefreshLoader(launcherItemId, target.IconBox, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            return loader.LoadAndSaveAsync(cancellationToken);
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
                Logger.LogInformation("ランチャーアイテムのアイコンを更新開始");

                var refreshedLauncherItemsIds = new List<Guid>(allLauncherItemIds.Count);

                foreach(var launcherItemId in allLauncherItemIds) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var targets = GetUpdateTartget(launcherItemId);

                    if(targets.Count != 0) {
                        foreach(var target in targets) {
                            cancellationToken.ThrowIfCancellationRequested();
                            await UpdateTargetAsync(launcherItemId, target, cancellationToken);
                        }
                        refreshedLauncherItemsIds.Add(launcherItemId);
                    }
                }

                // 結構ちらつくし次回表示の際まで待機する方針のため更新通知は行わない
                /*
                foreach(var launcherItemId in refreshedLauncherItemsIds) {
                    OrderManager.RefreshLauncherItemElement(launcherItemId);
                }
                foreach(var launcherItemId in refreshedLauncherItemsIds) {
                    NotifyManager.SendLauncherItemChanged(launcherItemId);
                }
                */


                Logger.LogInformation("ランチャーアイテムのアイコンを更新終了");
            });
        }

        #endregion
    }
}
