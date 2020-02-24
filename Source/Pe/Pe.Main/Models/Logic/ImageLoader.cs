using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class ImageLoader
    {
        public ImageLoader(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 画像を即時読み込み。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>returns>
        public BitmapSource Load(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            using(Initializer.Begin(bitmapImage)) {
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                bitmapImage.StreamSource = stream;
            }
            return bitmapImage;
        }

        #endregion
    }
}
