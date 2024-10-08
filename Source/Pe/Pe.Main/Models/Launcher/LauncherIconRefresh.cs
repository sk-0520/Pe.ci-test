using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    internal sealed class LauncherIconRefreshLoader: LauncherIconLoader
    {
        public LauncherIconRefreshLoader(LauncherItemId launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, mainDatabaseBarrier, largeDatabaseBarrier, databaseStatementLoader, null, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        public Task LoadAndSaveAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            Logger.LogDebug("ランチャーアイテムのアイコン取得開始: {0}, [{1}], [{2}], {3}", iconScale, launcherIconData.Path, launcherIconData.Icon, LauncherItemId);

            GetImageAsync(launcherIconData, iconScale, true, cancellationToken).ContinueWith(t => {
                try {
                    if(t.IsCompletedSuccessfully) {
                        var image = t.Result;
                        if(image != null) {
                            // 内部でシャットダウン
                            Logger.LogDebug("ランチャーアイテムのアイコン更新開始: {0}, {1}", iconScale, LauncherItemId);
                            return SaveImageAsync(image, iconScale);
                        } else {
                            Logger.LogDebug("ランチャーアイテムのアイコン更新失敗: {0}, {1}", iconScale, LauncherItemId);
                        }
                    } else {
                        Logger.LogDebug("ランチャーアイテムのアイコン取得停止: {0}, {1}", iconScale, LauncherItemId);
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
        public LauncherIconRefresher(TimeSpan refreshTime, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IOrderManager orderManager, INotifyManager notifyManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            RefreshTime = refreshTime;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            OrderManager = orderManager;
            NotifyManager = notifyManager;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private TimeSpan RefreshTime { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        private IOrderManager OrderManager { get; }
        private INotifyManager NotifyManager { get; }

        #endregion

        #region function

        private bool IsNeedUpdate(LauncherIconStatus launcherIconStatus, [DateTimeKind(DateTimeKind.Utc)] DateTime dateTime)
        {
            return true;// RefreshTime < (dateTime - launcherIconStatus.LastUpdatedTimestamp);
        }

        private IReadOnlyList<LauncherIconStatus> GetUpdateTarget(LauncherItemId launcherItemIs)
        {
            using var context = LargeDatabaseBarrier.WaitRead();
            var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
            var status = launcherItemIconsEntityDao.SelectLauncherItemIconAllStatus(launcherItemIs)
                .Where(i => IsNeedUpdate(i, DateTime.UtcNow))
                .ToList()
            ;
            return status;
        }

        private Task UpdateTargetAsync(LauncherItemId launcherItemId, LauncherIconStatus target, CancellationToken cancellationToken)
        {
            using(var context = LargeDatabaseBarrier.WaitRead()) {
                var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                var nowStatus = launcherItemIconsEntityDao.SelectLauncherItemIconKeyStatus(launcherItemId, target.IconScale);
                if(nowStatus == null) {
                    // 対象アイテムは破棄された
                    return Task.CompletedTask;
                }
                if(!IsNeedUpdate(nowStatus, DateTime.UtcNow)) {
                    // 対象アイテムはすでに更新された(待っている間にユーザー操作で変更された)
                    return Task.CompletedTask;
                }
            }

            var loader = new LauncherIconRefreshLoader(launcherItemId, MainDatabaseBarrier, LargeDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            return loader.LoadAndSaveAsync(target.IconScale, cancellationToken);
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

                var refreshedLauncherItemsIds = new List<LauncherItemId>(allLauncherItemIds.Count);

                foreach(var launcherItemId in allLauncherItemIds) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var targets = GetUpdateTarget(launcherItemId);

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
            }, cancellationToken);
        }

        #endregion
    }
}
