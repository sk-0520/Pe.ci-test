using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon
{
    public class LauncherIconLoader : IconImageLoaderBase
    {
        public LauncherIconLoader(Guid launcherItemId, IconBox iconBasicSize, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(iconBasicSize, dispatcherWapper, loggerFactory)
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

        BitmapSource? ToImage(IReadOnlyList<byte[]> imageBynaryItems)
        {
            using(var stream = new BinaryChunkedStream()) {
                using(var writer = new BinaryWriter(new KeepStream(stream))) {
                    foreach(var imageBinary in imageBynaryItems) {
                        writer.Write(imageBinary);
                    }
                }
                stream.Position = 0;
                BitmapSource? iconImage = null;
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
                    var dao = new LauncherItemIconsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
#pragma warning disable CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                    imageBinary = dao.SelectImageBinary(LauncherItemId, IconBasicSize);
#pragma warning restore CS8600 // Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                }

                if(imageBinary != null && imageBinary.Count == 0) {
                    return ResultSuccessValue.Failure<BitmapSource>();
                }
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                var image = ToImage(imageBinary);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。

#pragma warning disable CS8634 // この型を、ジェネリック型またはメソッド内で型パラメーターとして使用することはできません。型引数の Null 許容性が 'class' 制約と一致しません。
#pragma warning disable CS8619 // 値における参照型の Null 許容性が、対象の型と一致しません。
                return ResultSuccessValue.Success(image);
#pragma warning restore CS8619 // 値における参照型の Null 許容性が、対象の型と一致しません。
#pragma warning restore CS8634 // この型を、ジェネリック型またはメソッド内で型パラメーターとして使用することはできません。型引数の Null 許容性が 'class' 制約と一致しません。
            });
        }

        LauncherIconData GetIconData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherItemDomainDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectIcon(LauncherItemId);
            }
        }

        Task<BitmapSource> GetImageCoreAsync(LauncherItemKind kind, IconData iconData, CancellationToken cancellationToken)
        {
#pragma warning disable CS8619 // 値における参照型の Null 許容性が、対象の型と一致しません。
            return GetIconImageAsync(iconData, cancellationToken);
#pragma warning restore CS8619 // 値における参照型の Null 許容性が、対象の型と一致しません。
        }

        async Task<BitmapSource?> GetImageAsync(LauncherIconData launcherIconData, CancellationToken cancellationToken)
        {
            var iconImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Icon, cancellationToken).ConfigureAwait(false);
            if(iconImage != null) {
                return iconImage;
            }

            var commandImage = await GetImageCoreAsync(launcherIconData.Kind, launcherIconData.Path, cancellationToken).ConfigureAwait(false);
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
                    var dao = new LauncherItemIconsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                    dao.DeleteImageBinary(LauncherItemId, IconBasicSize);
                    dao.InsertImageBinary(LauncherItemId, IconBasicSize, stream.BinaryChunkedList, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }
            }


        }

        async Task<BitmapSource?> MakeImageAsync(CancellationToken cancellationToken)
        {
            // アイコンパス取得
            var launcherIconData = GetIconData();

            // アイコン取得
            var iconImage = await GetImageAsync(launcherIconData, cancellationToken).ConfigureAwait(false);
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
    }
}
