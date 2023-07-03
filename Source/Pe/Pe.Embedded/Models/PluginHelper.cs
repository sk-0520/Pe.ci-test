using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Embedded.Models
{
    /// <summary>
    /// プラグインヘルパー。
    /// </summary>
    internal static class PluginHelper
    {
        #region function

        public static Assembly GetPluginAssembly() => Assembly.GetExecutingAssembly();

        /// <summary>
        /// プラグインアセンブリのイメージを取得
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="imageLoader"></param>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        public static DependencyObject GetPluginImage(string relativePath, IImageLoader imageLoader, in IconScale iconScale)
        {
            var assemblyName = GetPluginAssembly().GetName().Name;
            var uri = new Uri("pack://application:,,,/" + assemblyName + ";component/" + relativePath.TrimStart('/'));

            var decoder = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            var bitmap = imageLoader.GetImageFromFrames(decoder.Frames, iconScale);
            var image = new System.Windows.Controls.Image() {
                Source = bitmap,
            };
            return image;
        }

        /// <summary>
        /// プラグインアセンブリの /Plugin.icon を取得する。
        /// </summary>
        /// <param name="imageLoader"></param>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        public static DependencyObject GetDefaultPluginIcon(IImageLoader imageLoader, in IconScale iconScale)
        {
            return GetPluginImage("Plugin.ico", imageLoader, iconScale);
        }

        #endregion
    }
}
