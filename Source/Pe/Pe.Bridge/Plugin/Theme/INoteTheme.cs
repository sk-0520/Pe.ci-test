using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// ノート タイトルバーボタン。
    /// </summary>
    public enum NoteCaptionButtonKind
    {
        /// <summary>
        /// 最小化・復帰ボタン。
        /// </summary>
        Compact,
        /// <summary>
        /// 最前面ボタン。
        /// </summary>
        Topmost,
        /// <summary>
        /// 閉じるボタン。
        /// </summary>
        Close,
    }

    /// <summary>
    /// ノート タイトルバーボタン状態。
    /// </summary>
    public enum NoteCaptionButtonState
    {
        /// <summary>
        /// 通常。
        /// </summary>
        None,
        /// <summary>
        /// マウスオーバー。
        /// </summary>
        Over,
        /// <summary>
        /// ボタン押下時。
        /// </summary>
        Pressed,
    }

    /// <summary>
    /// ノートのテーマ。
    /// </summary>
    public interface INoteTheme
    {
        #region function

        /// <summary>
        /// タイトルバーの高さを取得。
        /// </summary>
        /// <remarks>
        /// <para>縦置き(左右指定)の場合は横幅となる。</para>
        /// </remarks>
        /// <returns>論理ピクセル。</returns>
        [return: PixelKind(Px.Logical)]
        double GetCaptionHeight();
        /// <summary>
        /// タイトルバーのフォントサイズを取得。
        /// </summary>
        /// <param name="baseFontSize"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        double GetCaptionFontSize(double baseFontSize);
        /// <summary>
        /// タイトルバーのフォントファミリを取得。
        /// </summary>
        /// <param name="baseFontFamily">ノートに設定されているフォントファミリ。</param>
        /// <returns></returns>
        FontFamily GetCaptionFontFamily(FontFamily baseFontFamily);

        /// <summary>
        /// ノートの境界線幅を取得。
        /// </summary>
        /// <returns>論理ピクセル。</returns>
        [return: PixelKind(Px.Logical)]
        Thickness GetBorderThickness();
        /// <summary>
        /// ノートのリサイズグリップサイズを取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Size GetResizeGripSize();

        /// <summary>
        /// タイトルバーボタンのブラシを取得。
        /// </summary>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしてのブラシ。</returns>
        ColorPair<Brush> GetCaptionBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        /// <summary>
        /// 境界線のブラシを取得。
        /// </summary>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしてのブラシ。</returns>
        Brush GetBorderBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        /// <summary>
        /// 本文のブラシを取得。
        /// </summary>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしてのブラシ。</returns>
        ColorPair<Brush> GetContentBrush(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);
        /// <summary>
        /// タイトルバーボタンの背景ブラシを取得。
        /// </summary>
        /// <param name="buttonState">ボタン状態。</param>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしてのボタン背景色。</returns>
        Brush GetCaptionButtonBackgroundBrush(NoteCaptionButtonState buttonState, NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);

        /// <summary>
        /// タイトルバーボタンの表示要素を取得。
        /// </summary>
        /// <param name="buttonKind">ボタン種別。</param>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="isEnabled">有効か</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしての表示要素。</returns>
        DependencyObject GetCaptionImage(NoteCaptionButtonKind buttonKind, NoteCaptionPosition captionPosition, bool isEnabled, ColorPair<Color> baseColor);
        /// <summary>
        /// リサイズグリップの表示要素を取得。
        /// </summary>
        /// <param name="captionPosition">タイトルバー位置。</param>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしての表示要素。</returns>
        DependencyObject GetResizeGripImage(NoteCaptionPosition captionPosition, ColorPair<Color> baseColor);

        /// <summary>
        ///ブラインド時のエフェクト取得。
        /// </summary>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしてのエフェクト要素。</returns>
        Effect GetBlindEffect(ColorPair<Color> baseColor);
        /// <summary>
        ///ブラインド時の本文表示要素。
        /// </summary>
        /// <param name="baseColor">ノートに設定されている色。</param>
        /// <returns>テーマとしての本文表示要素。</returns>
        DependencyObject GetBlindContent(ColorPair<Color> baseColor);

        #endregion
    }
}
