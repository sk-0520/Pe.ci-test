using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Application;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon
{
    public class LauncherIconLoader : IconImageLoaderBase
    {
        public LauncherIconLoader(Guid launcherItemId, IconScale iconScale, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(iconScale, dispatcherWapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        public Guid LauncherItemId { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        #endregion

        #region function

        BitmapSource ToImage(IReadOnlyList<byte[]> imageBynaryItems)
        {
            using(var stream = new BinaryChunkedStream()) {
                using(var writer = new BinaryWriter(new KeepStream(stream))) {
                    foreach(var imageBinary in imageBynaryItems) {
                        writer.Write(imageBinary);
                    }
                }
                stream.Position = 0;
                BitmapSource iconImage = null;
                DispatcherWapper.Invoke(() => {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    iconImage = bitmap;
                    FreezableUtility.SafeFreeze(iconImage);
                });
                return iconImage;

            }
        }

        Task<ResultSuccessValue<BitmapSource>> LoadExistsImageAsync()
        {
            return Task.Run(() => {
                IReadOnlyList<byte[]> imageBinary;
                using(var commander = FileDatabaseBarrier.WaitRead()) {
                    var dao = new LauncherItemIconsDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                    imageBinary = dao.SelectImageBinary(LauncherItemId, IconScale);
                }

                if(imageBinary.Count == 0) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }
                var image = ToImage(imageBinary);

                return ResultSuccessValue.Success(image);
            });
        }

        LauncherIconData GetIconData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new ApplicationLauncherItemDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return dao.SelectIcon(LauncherItemId);
            }
        }

        Task<BitmapSource> GetImageCoreAsync(LauncherItemKind kind, IconData iconData, CancellationToken cancellationToken)
        {
            return GetIconImageAsync(iconData, cancellationToken);
        }

        async Task<BitmapSource> GetImageAsync(LauncherIconData launcherIconData, CancellationToken cancellationToken)
        {
            var iconImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Icon, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                return iconImage;
            }

            var commandImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Command, cancellationToken).ConfigureAwait(false);
            if(commandImage != null) {
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
                    var dao = new LauncherItemIconsDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                    dao.DeleteImageBinary(LauncherItemId, IconScale);
                    dao.InsertImageBinary(LauncherItemId, IconScale, stream.BinaryChunkedList, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }
            }


        }

        async Task<BitmapSource> MakeImageAsync(CancellationToken cancellationToken)
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = await GetImageAsync(launcherIconData, cancellationToken).ConfigureAwait(false);

            // データ書き込み(失敗してもアイコンが取得できてるならOK)
            SaveImage(iconImage);

            return iconImage;
        }


        #endregion

        #region IconImageLoaderBase

        protected override async Task<BitmapSource> LoadImplAsync(CancellationToken cancellationToken)
        {
            var existisResult = await LoadExistsImageAsync().ConfigureAwait(false);
            if(existisResult.Success) {
                return existisResult.SuccessValue;
            }

            var image = await MakeImageAsync(cancellationToken).ConfigureAwait(false);
            return image;
        }

        #endregion
    }
}
