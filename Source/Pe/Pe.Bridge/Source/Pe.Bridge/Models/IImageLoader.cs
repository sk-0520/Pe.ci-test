using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 画像系のただめんどい処理を担当。
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IImageLoader
    {
        #region function

        /// <summary>
        /// プライマリスクリーンのDPIスケールを取得。
        /// </summary>
        /// <returns></returns>
        Point GetPrimaryDpiScale();

        /// <summary>
        /// スクリーンのDPIスケールを取得。
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        Point GetDpiScale(IScreen screen);

        /// <summary>
        /// 複数画像からサイズが一番近しい画像を取得する。
        /// </summary>
        /// <param name="iconUri"></param>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        public BitmapSource GetImageFromFrames(IReadOnlyCollection<BitmapSource> frames, IconScale iconScale);

        /// <summary>
        /// ファイルからアイコンを取得。
        /// </summary>
        /// <param name="iconPath">対象ファイルパス。</param>
        /// <param name="iconIndex">アイコンインデックス(0基点)。</param>
        /// <param name="iconScale">サイズ。</param>
        /// <returns>取得したアイコン。取得失敗時は null が返る。</returns>
        public BitmapSource? LoadIconFromFile(string iconPath, int iconIndex, IconScale iconScale);


        #endregion
    }
}
