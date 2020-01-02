using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public enum InputState
    {
        /// <summary>
        /// 未入力。
        /// </summary>
        Empty,
        /// <summary>
        /// 入力中。
        /// </summary>
        Finding,
        /// <summary>
        /// 検索対象なし。
        /// </summary>
        NotFound,
    }

    public interface ICommandTheme
    {
        #region property

        /// <summary>
        /// ウィンドウの背景ブラシ取得。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBackgroundBrush(bool isActive);

        /// <summary>
        /// ウィンドウの枠サイズ。
        /// </summary>
        /// <returns></returns>
        Thickness GetViewBorderThickness();

        /// <summary>
        /// ウィンドウの枠ブラシ。
        /// </summary>
        /// <returns></returns>
        Brush GetViewBorderBrush(bool isActive);

        /// <summary>
        /// つかむところの幅。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Logical)]
        double GetGripWidth();
        /// <summary>
        /// つかむところの色。
        /// </summary>
        /// <returns></returns>
        Brush GetGripBrush(bool isActive);

        Border GetInputBorder(InputState inputState);
        Brush GetInputForeground(InputState inputState);
        Brush GetInputBackground(InputState inputState);

        DependencyObject GetExecuteButton();

        #endregion
    }
}
