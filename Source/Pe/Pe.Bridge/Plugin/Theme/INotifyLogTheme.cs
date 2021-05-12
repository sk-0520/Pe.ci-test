using System.Windows;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// NOTE: テーマというかただの色設定マシーンになってる。CSSみたいにほいほい表示位置変えられる仕組みほしいなぁ。
    /// </summary>
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
        /// <summary>
        /// ログ リンク前景のブラシ。
        /// </summary>
        /// <param name="isMouseOver"></param>
        /// <returns></returns>
        Brush GetHyperlinkForegroundBrush(bool isMouseOver);

        #endregion
    }
}
