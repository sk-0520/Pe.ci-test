using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherIconLoader: IconImageLoaderBase, ILauncherItemId
    {
        public LauncherIconLoader(LauncherItemId launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper? dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private static EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        private int RetryMaxCount { get; } = 5;
        private TimeSpan RetryWaitTime { get; } = TimeSpan.FromSeconds(1);

        #endregion

        #region function

        private Task<ResultSuccess<BitmapSource>> LoadExistsImageAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.Run((Func<ResultSuccess<BitmapSource>>)(() => {
                byte[] imageBinary;
                using(var context = LargeDatabaseBarrier.WaitRead()) {
                    var dao = new LauncherItemIconsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    imageBinary = dao.SelectImageBinary(LauncherItemId, iconScale);
                }

                if(imageBinary.Length == 0) {
                    return Result.CreateFailure<BitmapSource>();
                }

                var image = ToImage(imageBinary);

                if(image == null) {
                    return Result.CreateFailure<BitmapSource>();
                }

                return Result.CreateSuccess<BitmapSource>(image);
            }));
        }

        protected virtual LauncherIconData GetIconData()
        {
            ThrowIfDisposed();

            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemDomainDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                return dao.SelectFileIcon(LauncherItemId);
            }
        }

        protected virtual Task<BitmapSource?> GetImageCoreAsync(LauncherItemKind kind, IReadOnlyIconData iconData, IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var editIconData = new IconData() {
                Path = Environment.ExpandEnvironmentVariables(iconData.Path),
                Index = iconData.Index,
            };

            if(Path.IsPathFullyQualified(editIconData.Path)) {
                return GetIconImageAsync(editIconData, iconScale, cancellationToken);
            }

            if(string.IsNullOrWhiteSpace(editIconData.Path)) {
                // 無効パスのため後続処理不要
                return Task.FromResult(default(BitmapSource));
            }

            var pathItems = EnvironmentPathExecuteFileCache.GetItems(LoggerFactory);
            var environmentExecuteFile = new EnvironmentExecuteFile(LoggerFactory);
            var pathItem = environmentExecuteFile.Get(editIconData.Path, pathItems);
            if(pathItem == null) {
                Logger.LogWarning("指定されたコマンドからパス取得失敗: {0}", editIconData.Path);
                return Task.FromResult(default(BitmapSource));
            }

            editIconData.Path = pathItem.File.FullName;
            editIconData.Index = 0;
            return GetIconImageAsync(editIconData, iconScale, cancellationToken);
        }

        protected async Task<BitmapSource?> GetImageAsync(LauncherIconData launcherIconData, IconScale iconScale, bool tuneSize, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var iconImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Icon, iconScale, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                if(tuneSize) {
                    return ResizeImage(iconImage, iconScale);
                }
                return iconImage;
            }

            //if(launcherIconData.Kind == LauncherItemKind.StoreApp) {

            //}

            var commandImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Path, iconScale, cancellationToken).ConfigureAwait(false);
            if(commandImage != null) {
                if(tuneSize) {
                    return ResizeImage(commandImage, iconScale);
                }
                return commandImage;
            }

            // 標準のやつ。。。 xaml でやるべき？
            return null;
        }

        protected Task WriteStreamAsync(BitmapSource iconImage, Stream stream)
        {
            Debug.Assert(iconImage.IsFrozen);
            ThrowIfDisposed();

            return Task.Run(() => {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(iconImage));
                encoder.Save(stream);
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            });
        }

        protected async Task SaveImageAsync(BitmapSource iconImage, IconScale iconScale)
        {
            ThrowIfDisposed();

            using(var stream = new MemoryReleaseStream()) {
                await WriteStreamAsync(iconImage, stream);

                DateTime iconUpdatedTimestamp = DateTime.UtcNow;
                using(var context = LargeDatabaseBarrier.WaitWrite()) {
                    var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    launcherItemIconsEntityDao.DeleteImageBinary(LauncherItemId, iconScale);
                    launcherItemIconsEntityDao.InsertImageBinary(LauncherItemId, iconScale, stream.GetBuffer().Take((int)stream.Position), iconUpdatedTimestamp, DatabaseCommonStatus.CreateCurrentAccount());
                    context.Commit();
                }
            }
        }

        private async Task<BitmapSource?> MakeImageAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = await GetImageAsync(launcherIconData, iconScale, true, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                // データ書き込み(失敗してもアイコンが取得できてるならOK)
                var _ = SaveImageAsync(iconImage, iconScale);
            } else {
                Logger.LogWarning("アイコン取得失敗: {0}, {1}", LauncherItemId, ObjectDumper.GetDumpString(launcherIconData));
            }

            return iconImage;
        }

        #endregion

        #region IconImageLoaderBase

        protected override async Task<BitmapSource?> LoadImplAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            var counter = new Counter(RetryMaxCount);
            foreach(var count in counter) {
                try {
                    var existsResult = await LoadExistsImageAsync(iconScale, cancellationToken).ConfigureAwait(false);
                    if(existsResult.Success) {
                        return existsResult.SuccessValue;
                    }

                    var image = await MakeImageAsync(iconScale, cancellationToken).ConfigureAwait(false);
                    return image;
                } catch(SynchronizationLockException ex) {
                    if(count.IsLast) {
                        Logger.LogError(ex, "アイコン取得待機失敗: 全試行 {0}回 失敗: {1}", count.MaxCount, LauncherItemId);
                        return null;
                    } else {
                        Logger.LogWarning(ex, "アイコン取得待機失敗: {0}/{1}回 失敗, 再試行待機 {2}, {3}", count.CurrentCount, count.MaxCount, RetryWaitTime, LauncherItemId);
                        await Task.Delay(RetryWaitTime, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            throw new NotImplementedException();
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }
}
