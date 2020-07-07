using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
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

    public interface ICommandTheme
    {
        #region function

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

        Thickness GetSelectedIconMargin(in IconScale iconScale);

        Thickness GetInputBorderThickness();
        Brush GetInputBorderBrush(InputState inputState);
        Brush GetInputForeground(InputState inputState);
        Brush GetInputBackground(InputState inputState);

        ControlTemplate GetExecuteButtonControlTemplate(in IconScale iconScale);

        #endregion
    }
}
