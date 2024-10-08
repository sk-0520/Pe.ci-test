using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class IconImageLoaderBase: BindModelBase
    {
        protected IconImageLoaderBase(IDispatcherWrapper? dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            //IconBox = iconBox;
            DispatcherWrapper = dispatcherWrapper;
            RunningStatusImpl = new RunningStatus(LoggerFactory);
        }

        #region property

        //public IconBox IconBox { get; }

        /// <summary>
        /// 非 <see langword="null" /> の場合に、<see cref="DependencyObject"/>操作時に指定の<see cref="IDispatcherWrapper"/>で処理する。
        /// </summary>
        protected IDispatcherWrapper? DispatcherWrapper { get; }

        private RunningStatus RunningStatusImpl { get; }
        public IRunningStatus RunningStatus => RunningStatusImpl;

        public static IReadOnlyCollection<string> ImageFileExtensions { get; } = new[] { "png", "bmp", "jpeg", "jpg" };

        protected BitmapSource? CachedImage { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// 画像を即時読み込み。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>returns>
        private static BitmapSource LoadFromStream(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            using(Initializer.Begin(bitmapImage)) {
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                bitmapImage.StreamSource = stream;
            }
            return bitmapImage;
        }

        protected BitmapSource ToImage(byte[] imageBinaryItems)
        {
            ArgumentNullException.ThrowIfNull(imageBinaryItems);
            ThrowIfDisposed();

            using(var stream = new MemoryReleaseStream()) {
                using(var writer = new BinaryWriter(new KeepStream(stream))) {
                    foreach(var imageBinary in imageBinaryItems) {
                        writer.Write(imageBinary);
                    }
                }
                stream.Position = 0;

                static BitmapSource LoadImage(Func<Stream, BitmapSource> loader, Stream stream)
                {
                    var image = loader(stream);
                    FreezableUtility.SafeFreeze(image);
                    return image;
                }
                var iconImage = DispatcherWrapper?.Get(s => LoadImage(LoadFromStream, s), stream) ?? LoadImage(LoadFromStream, stream);

                return iconImage;
            }
        }

        /// <summary>
        /// <see cref="IconBox"/> より大きい場合にががっと縮小する。
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        protected BitmapSource ResizeImage(BitmapSource bitmapSource, in IconScale iconScale)
        {
            ThrowIfDisposed();

            var iconSize = iconScale.ToIconSize();

            if(iconSize.Width < bitmapSource.PixelWidth || iconSize.Height < bitmapSource.PixelHeight) {
                Logger.LogDebug("アイコンサイズを縮小: アイコン({0}x{1}), 指定({2}x{3})", bitmapSource.PixelWidth, bitmapSource.PixelHeight, iconSize.Width, iconSize.Height);
                var scaleX = iconSize.Width / (double)bitmapSource.PixelWidth;
                var scaleY = iconSize.Height / (double)bitmapSource.PixelHeight;
                Logger.LogTrace("scale: {0}x{1}", scaleX, scaleY);

                static BitmapSource ResizeCore(BitmapSource bitmapSource, double scaleX, double scaleY)
                {
                    var transformedBitmap = FreezableUtility.GetSafeFreeze(new TransformedBitmap(bitmapSource, new ScaleTransform(scaleX, scaleY)));
                    return FreezableUtility.GetSafeFreeze(new WriteableBitmap(transformedBitmap));
                }

                return DispatcherWrapper?.Get(args => ResizeCore(args.bitmapSource, args.scaleX, args.scaleY), (bitmapSource, scaleX, scaleY)) ?? ResizeCore(bitmapSource, scaleX, scaleY);
            }

            return bitmapSource;
        }

        protected Task<BitmapSource?> GetIconImageAsync(IReadOnlyIconData iconData, IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var path = TextUtility.SafeTrim(iconData.Path);
            if(string.IsNullOrEmpty(path)) {
                return Task.FromResult(default(BitmapSource));
            }

            return Task.Run(() => {
                var isFile = File.Exists(path);
                var isDir = !isFile && Directory.Exists(path);
                if(!isFile && !isDir) {
                    return null;
                }

                BitmapSource? iconImage = null;

                if(isFile && PathUtility.HasExtensions(path, ImageFileExtensions)) {
                    Logger.LogDebug("画像ファイルとして読み込み {0}", path);
                    using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        static BitmapSource LoadCore(Func<Stream, BitmapSource> loader, Stream stream)
                        {
                            var image = loader(stream);
                            return FreezableUtility.GetSafeFreeze(image);
                        }
                        iconImage = DispatcherWrapper?.Get(() => LoadCore(LoadFromStream, stream)) ?? LoadCore(LoadFromStream, stream);
                    }
                } else {
                    Logger.LogDebug("アイコンファイルとして読み込み {0}", path);
                    var iconLoader = new IconLoader(LoggerFactory);
                    static BitmapSource LoadCore(string path, int index, in IconScale iconScale, IconLoader iconLoader)
                    {
                        var iconSize = iconScale.ToIconSize();
                        var image = iconLoader.Load(path, index, iconSize);
                        return FreezableUtility.GetSafeFreeze(image!);
                    }
                    iconImage = DispatcherWrapper?.Get(() => LoadCore(path, iconData.Index, iconScale, iconLoader)) ?? LoadCore(path, iconData.Index, iconScale, iconLoader);
                }

                return iconImage;
            });
        }

        protected abstract Task<BitmapSource?> LoadImplAsync(IconScale iconScale, CancellationToken cancellationToken);

        public async Task<BitmapSource?> LoadAsync(bool useCache, IconScale iconScale, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if(useCache && CachedImage != null) {
                Logger.LogTrace("メモリキャッシュされたイメージの使用");
                RunningStatusImpl.State = RunningState.End;
                return CachedImage;
            }

            RunningStatusImpl.State = RunningState.Running;
            try {
                var iconImage = await LoadImplAsync(iconScale, cancellationToken);
                RunningStatusImpl.State = RunningState.End;
                if(useCache) {
                    CachedImage = iconImage;
                }
                return iconImage;
            } catch(OperationCanceledException ex) {
                Logger.LogWarning(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Cancel;
                throw;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Error;
                throw;
            }
        }

        //public void ClearCache()
        //{
        //    CachedImage = null;
        //}

        #endregion

        #region BindModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    RunningStatusImpl.Dispose();
                    CachedImage = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class IconImageLoader: IconImageLoaderBase
    {
        public IconImageLoader(IReadOnlyIconData iconData, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        {
            IconData = iconData;
        }

        #region property

        private IReadOnlyIconData IconData { get; }

        #endregion

        #region IconImageLoaderBase

        protected override Task<BitmapSource?> LoadImplAsync(IconScale iconScale, CancellationToken cancellationToken)
        {
            return GetIconImageAsync(IconData, iconScale, cancellationToken);
        }

        #endregion
    }
}
