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
        #region property

        /// <summary>
        /// 処理を停止中か。
        /// </summary>
        bool IsPausing { get; }

        #endregion
    }
    public interface IBackgroundAddonRunShutdownContext
    { }

    /// <summary>
    /// 実行処理コンテキスト。
    /// </summary>
    public interface IBackgroundAddonRunExecuteContext
    {
        #region property

        RunExecuteKind RunExecuteKind { get; }
        object? Parameter { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

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
        #region property

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
        #region property

        MouseButton Button { get; }
        MouseButtonState State { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        DateTime Timestamp { get; }

        #endregion
    }

}
