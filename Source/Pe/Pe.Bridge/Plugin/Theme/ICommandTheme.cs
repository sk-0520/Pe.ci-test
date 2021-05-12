using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// 入力状態。
    /// </summary>
    public enum InputState
    {
        /// <summary>
        /// 未入力。
        /// </summary>
        Empty,
        /// <summary>
        /// 検索。
        /// </summary>
        Finding,
        /// <summary>
        /// 検索完了。
        /// </summary>
        Complete,
        /// <summary>
        /// 検索対象なし。
        /// </summary>
        NotFound,
    }

    /// <summary>
    /// コマンドtheme。
    /// </summary>
    public interface ICommandTheme
    {
        #region function

        /// <summary>
        /// ウィンドウの背景ブラシ取得。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBackgroundBrush(bool isActive);

        /// <summary>
        /// ウィンドウの枠サイズを取得。
        /// </summary>
        /// <returns></returns>
        Thickness GetViewBorderThickness();

        /// <summary>
        /// ウィンドウの枠ブラシを取得。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBorderBrush(bool isActive);

        /// <summary>
        /// つかむところの幅を取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        double GetGripWidth();
        /// <summary>
        /// つかむところの色を取得。
        /// </summary>
        /// <returns></returns>
        Brush GetGripBrush(bool isActive);

        /// <summary>
        /// 表示アイコンのマージンを取得。
        /// </summary>
        /// <param name="iconScale">アイコンスケール。</param>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Thickness GetSelectedIconMargin(in IconScale iconScale);

        /// <summary>
        ///入力欄の境界線を取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        Thickness GetInputBorderThickness();
        /// <summary>
        /// 入力欄の境界線ブラシを取得。
        /// </summary>
        /// <param name="inputState"></param>
        /// <returns></returns>
        Brush GetInputBorderBrush(InputState inputState);
        /// <summary>
        /// 入力欄の前景色を取得。
        /// </summary>
        /// <param name="inputState"></param>
        /// <returns></returns>
        Brush GetInputForeground(InputState inputState);
        /// <summary>
        /// 入力欄の前景色を取得。
        /// </summary>
        /// <param name="inputState"></param>
        /// <returns></returns>
        Brush GetInputBackground(InputState inputState);

        /// <summary>
        /// 実行ボタンのテンプレ。
        /// </summary>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        ControlTemplate GetExecuteButtonControlTemplate(in IconScale iconScale);

        #endregion
    }
}
