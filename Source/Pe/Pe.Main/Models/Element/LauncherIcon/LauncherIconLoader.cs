using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon
{
    public class LauncherIconLoader : IconImageLoaderBase, ILauncherItemId
    {
        public LauncherIconLoader(Guid launcherItemId, IconBox iconBox, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(iconBox, dispatcherWapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        static EnvironmentPathExecuteFileCache EnvironmentPathExecuteFileCache { get; } = EnvironmentPathExecuteFileCache.Instance;

        #endregion

        #region function

        BitmapSource? ToImage(IReadOnlyList<byte[]>? imageBynaryItems)
        {
            if(imageBynaryItems == null || imageBynaryItems.Count == 0) {
                return null;
            }

            using(var stream = new BinaryChunkedStream()) {
                using(var writer = new BinaryWriter(new KeepStream(stream))) {
                    foreach(var imageBinary in imageBynaryItems) {
                        writer.Write(imageBinary);
                    }
                }
                stream.Position = 0;
                BitmapSource? iconImage = null;
                DispatcherWapper.Invoke(() => {
                    var imageLoader = new ImageLoader(LoggerFactory);
                    iconImage = imageLoader.Load(stream);
                    FreezableUtility.SafeFreeze(iconImage);
                });
                return iconImage;

            }
        }

        Task<ResultSuccessValue<BitmapSource>> LoadExistsImageAsync()
        {
            return Task.Run(() => {
                IReadOnlyList<byte[]>? imageBinary;
                using(var commander = FileDatabaseBarrier.WaitRead()) {
                    var dao = new LauncherItemIconsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    imageBinary = dao.SelectImageBinary(LauncherItemId, IconBox);
                }

                if(imageBinary != null && imageBinary.Count == 0) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }
                var image = ToImage(imageBinary);

                if(image == null) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }

                return ResultSuccessValue.Success(image);
            });
        }

        LauncherIconData GetIconData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectIcon(LauncherItemId);
            }
        }

        Task<BitmapSource?> GetImageCoreAsync(LauncherItemKind kind, IconData iconData, CancellationToken cancellationToken)
        {
            var editIconData = new IconData() {
                Path = Environment.ExpandEnvironmentVariables(iconData.Path),
                Index = iconData.Index,
            };

            if(Path.IsPathFullyQualified(editIconData.Path)) {
                return GetIconImageAsync(editIconData, cancellationToken);
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
            return GetIconImageAsync(editIconData, cancellationToken);
        }

        /// <summary>
        /// <see cref="IconBox"/> より大きい場合にががっと縮小する。
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        Task<BitmapSource> ResizeImageAsync(BitmapSource bitmapSource)
        {
            var iconSize = new IconSize(IconBox);

            if(iconSize.Width < bitmapSource.PixelWidth || iconSize.Height < bitmapSource.PixelHeight) {
                return Task.Run(() => {
                    Logger.LogDebug("アイコンサイズを縮小: アイコン({0}x{1}), 指定({2}x{3})", bitmapSource.PixelWidth, bitmapSource.PixelHeight, iconSize.Width, iconSize.Height);
                    var scaleX = iconSize.Width / (double)bitmapSource.PixelWidth;
                    var scaleY = iconSize.Height / (double)bitmapSource.PixelHeight;
                    Logger.LogTrace("scale: {0}x{1}", scaleX, scaleY);
                    var transformedBitmap = FreezableUtility.GetSafeFreeze(new TransformedBitmap(bitmapSource, new ScaleTransform(scaleX, scaleY)));
                    //var smallImage = new WriteableBitmap(iconSize.Width, iconSize.Height, bitmapSource.DpiX, bitmapSource.DpiY, bitmapSource.Format, bitmapSource.Palette);
                    return (BitmapSource)DispatcherWapper.Get(() => FreezableUtility.GetSafeFreeze(new WriteableBitmap(transformedBitmap)));
                });
            }

            return Task.FromResult(bitmapSource);
        }

        async Task<BitmapSource?> GetImageAsync(LauncherIconData launcherIconData, bool tuneSize, CancellationToken cancellationToken)
        {
            var iconImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Icon, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                if(tuneSize) {
                    return await ResizeImageAsync(iconImage).ConfigureAwait(false);
                }
                return iconImage;
            }

            var commandImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Path, cancellationToken).ConfigureAwait(false);
            if(commandImage != null) {
                if(tuneSize) {
                    return await ResizeImageAsync(commandImage).ConfigureAwait(false);
                }
                return commandImage;
            }

            // 標準のやつ。。。 xaml でやるべき？
            return null;
        }

        void WriteStream(BitmapSource iconImage, Stream stream)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(iconImage));
            encoder.Save(stream);
        }

        void SaveImage(BitmapSource iconImage)
        {
            using(var stream = new BinaryChunkedStream()) {
                WriteStream(iconImage, stream);
#if DEBUG
                using(var debugStream = new MemoryStream()) {
                    WriteStream(iconImage, debugStream);
                    Debug.Assert(stream.Position == debugStream.Position, $"{nameof(stream)}: {stream.Length}, {nameof(debugStream)}: {debugStream.Length}");
                }
#endif
                using(var commander = FileDatabaseBarrier.WaitWrite()) {
                    var dao = new LauncherItemIconsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    dao.DeleteImageBinary(LauncherItemId, IconBox);
                    dao.InsertImageBinary(LauncherItemId, IconBox, stream.BinaryChunkedList, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }
            }


        }

        async Task<BitmapSource?> MakeImageAsync(CancellationToken cancellationToken)
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = await GetImageAsync(launcherIconData, true, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                // データ書き込み(失敗してもアイコンが取得できてるならOK)
                SaveImage(iconImage);
            } else {
                Logger.LogWarning("アイコン取得失敗: {0}, {1}", LauncherItemId, ObjectDumper.GetDumpString(launcherIconData));
            }

            return iconImage;
        }


        #endregion

        #region IconImageLoaderBase

        protected override async Task<BitmapSource?> LoadImplAsync(CancellationToken cancellationToken)
        {
            var existisResult = await LoadExistsImageAsync().ConfigureAwait(false);
            if(existisResult.Success) {
                return existisResult.SuccessValue;
            }

            var image = await MakeImageAsync(cancellationToken).ConfigureAwait(false);
            return image;
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

    }
}
