using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// ツールバーテーマ。
    /// </summary>
    public interface ILauncherToolbarTheme
    {
        #region function

        /// <summary>
        /// ボタンの詰め(padding)領域を取得。
        /// </summary>
        /// <param name="toolbarPosition"></param>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Thickness GetButtonPadding(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale);
        /// <summary>
        /// ボタン内のアイコンの余白(margin)領域を取得。
        /// </summary>
        /// <param name="toolbarPosition"></param>
        /// <param name="iconScale"></param>
        /// <param name="isIconOnly"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Thickness GetIconMargin(AppDesktopToolbarPosition toolbarPosition, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        /// <summary>
        /// ツールバーの表示中サイズを取得。
        /// </summary>
        /// <param name="buttonPadding"></param>
        /// <param name="iconMargin"></param>
        /// <param name="iconScale"></param>
        /// <param name="isIconOnly"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Size GetDisplaySize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        /// <summary>
        /// ツールバーの隠れているサイズを取得。
        /// </summary>
        /// <param name="buttonPadding"></param>
        /// <param name="iconMargin"></param>
        /// <param name="iconScale"></param>
        /// <param name="isIconOnly"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Size GetHiddenSize([PixelKind(Px.Logical)] Thickness buttonPadding, [PixelKind(Px.Logical)] Thickness iconMargin, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);

        /// <summary>
        /// 通常ボタンのテンプレートを取得。
        /// </summary>
        /// <returns></returns>
        ControlTemplate GetLauncherItemNormalButtonControlTemplate();
        /// <summary>
        /// トグルボタンのテンプレートを取得。
        /// </summary>
        /// <returns></returns>
        ControlTemplate GetLauncherItemToggleButtonControlTemplate();

        /// <summary>
        /// ツールバーの背景ブラシを取得。
        /// </summary>
        /// <param name="toolbarPosition"></param>
        /// <param name="viewState"></param>
        /// <param name="iconScale"></param>
        /// <param name="isIconOnly"></param>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        Brush GetToolbarBackground(AppDesktopToolbarPosition toolbarPosition, ViewState viewState, in IconScale iconScale, bool isIconOnly, [PixelKind(Px.Logical)] double textWidth);
        /// <summary>
        /// ツールバーの前景ブラシを取得。
        /// </summary>
        /// <returns></returns>
        Brush GetToolbarForeground();

        DependencyObject GetLauncherSeparator(bool isHorizontal, int width);

        #endregion
    }
}
