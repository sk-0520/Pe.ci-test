using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    public interface IBackgroundAddonRunStartupContext
    { }

    public interface IBackgroundAddonRunPauseContext
    {
        #region proeprty

        /// <summary>
        /// 処理を停止中か。
        /// </summary>
        bool IsPausing { get; }

        #endregion
    }
    public interface IBackgroundAddonRunShutdownContext
    { }

    public interface IBackgroundAddonKeyboardContext
    {
        #region property

        Key Key { get; }
        bool IsDown { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseMoveContext
    {
        #region proeprty

        /// <summary>
        /// マウスカーソルの物理座標。
        /// </summary>
        [PixelKind(Px.Device)]
        Point Location { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseButtonContext
    {
        #region proeprty

        MouseButton Button { get; }
        MouseButtonState State { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

}
