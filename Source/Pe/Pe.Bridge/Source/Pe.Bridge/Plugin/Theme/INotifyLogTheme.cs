using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public interface INotifyLogTheme
    {
        #region property

        #endregion

        #region function

        /// <summary>
        /// ビューの境界線幅。
        /// </summary>
        /// <returns></returns>
        Thickness GetViewBorderThickness();
        /// <summary>
        /// ビューの境界線ブラシ。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBorderBrush();
        /// <summary>
        /// ビューの境界線の丸み。
        /// </summary>
        /// <returns></returns>
        CornerRadius GetViewBorderCornerRadius();

        /// <summary>
        /// ビューの背景ブラシ。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBackgroundBrush();
        /// <summary>
        /// ビューのパディング領域。
        /// </summary>
        /// <returns></returns>
        Thickness GetViewPaddingThickness();

        /// <summary>
        /// ログ ヘッダ部のブラシ。
        /// </summary>
        /// <param name="isTopmost">最上位固定メッセージか。</param>
        /// <returns></returns>
        Brush GetHeaderForegroundBrush(bool isTopmost);
        /// <summary>
        /// ログ メッセージ部のブラシ。
        /// </summary>
        /// <param name="isTopmost">最上位固定メッセージか。</param>
        /// <returns></returns>
        Brush GetContentForegroundBrush(bool isTopmost);

        #endregion
    }
}
