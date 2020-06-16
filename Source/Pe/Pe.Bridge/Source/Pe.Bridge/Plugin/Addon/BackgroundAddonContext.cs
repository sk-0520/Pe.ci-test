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
    public interface IBackgroundAddonRunShutdownContext
    { }

    public interface IBackgroundAddonKeyboardContext
    {
        #region property

        Key Key { get; }
        bool IsDown { get; }

        DateTime TimestampUtc { get; }

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
        DateTime TimestampUtc { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseButtonContext
    {
        #region proeprty

        MouseButton Button { get; }
        MouseButtonState State { get; }
        DateTime TimestampUtc { get; }

        #endregion
    }

}
