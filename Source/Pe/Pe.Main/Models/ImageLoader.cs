using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal class ImageLoader: IImageLoader
    {
        public ImageLoader(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }

        #endregion

        #region IImageLoader

        /// <inheritdoc cref="IImageLoader.GetPrimaryDpiScale"/>
        public Point GetPrimaryDpiScale() => GetDpiScale(Screen.PrimaryScreen ?? throw new InvalidOperationException("Screen.PrimaryScreen is null"));

        /// <inheritdoc cref="IImageLoader.GetDpiScale(IScreen)"/>
        public Point GetDpiScale(IScreen screen)
        {
            return ScreenUtility.GetDpiScale(screen);
        }

        /// <inheritdoc cref="IImageLoader.GetImageFromFrames(IReadOnlyCollection{BitmapSource}, in IconScale)"/>
        public BitmapSource GetImageFromFrames(IReadOnlyCollection<BitmapSource> frames, in IconScale iconScale)
        {
            var size = iconScale.ToIconSize().Width;
            var result = frames.FirstOrDefault(f => f.Width == size);

            if(result == null) {
                // ダメっぽいときは一番近くっぽいのをとる。
                int diff = frames.Min(i => Math.Abs(i.PixelWidth - size));
                result = frames.First(i => Math.Abs(i.PixelWidth - size) == diff);
            }

            return result;
        }

        /// <inheritdoc cref="IImageLoader.LoadIconFromFile(string, int, in IconScale)"/>
        public BitmapSource? LoadIconFromFile(string iconPath, int iconIndex, in IconScale iconScale)
        {
            var iconLoader = new IconLoader(LoggerFactory);
            var result = iconLoader.Load(iconPath, iconIndex, iconScale.ToIconSize());
            return result;
        }

        #endregion
    }
}
