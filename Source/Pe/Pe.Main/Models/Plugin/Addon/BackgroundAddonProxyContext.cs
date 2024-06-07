using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class BackgroundAddonProxyRunStartupContext: IBackgroundAddonRunStartupContext
    { }

    public class BackgroundAddonProxyRunPauseContext: IBackgroundAddonRunPauseContext
    {
        public BackgroundAddonProxyRunPauseContext(bool isPausing)
        {
            IsPausing = isPausing;
        }

        #region IBackgroundAddonRunPauseContext

        public bool IsPausing { get; }

        #endregion
    }

    public class BackgroundAddonProxyRunExecuteContext: IBackgroundAddonRunExecuteContext
    {
        public BackgroundAddonProxyRunExecuteContext(RunExecuteKind runExecuteKind, object? parameter, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            RunExecuteKind = runExecuteKind;
            Parameter = parameter;
            Timestamp = timestamp;
        }

        #region IBackgroundAddonRunPauseContext

        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.RunExecuteKind"/>
        public RunExecuteKind RunExecuteKind { get; }

        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.Parameter"/>
        public object? Parameter { get; }
        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.Timestamp"/>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }

    public class BackgroundAddonProxyRunShutdownContext: IBackgroundAddonRunShutdownContext
    { }

    public class BackgroundAddonProxyKeyboardContext: IBackgroundAddonKeyboardContext
    {
        public BackgroundAddonProxyKeyboardContext(KeyboardHookEventArgs keyboardHookEventArgs)
        {
            KeyboardHookEventArgs = keyboardHookEventArgs;
        }

        #region property

        public KeyboardHookEventArgs KeyboardHookEventArgs { get; }

        #endregion

        #region IBackgroundAddonKeyboardContext

        public Key Key => KeyboardHookEventArgs.Key;

        public bool IsDown => KeyboardHookEventArgs.IsDown;

        public DateTime Timestamp => KeyboardHookEventArgs.Timestamp;

        #endregion
    }

    public class BackgroundAddonProxyMouseMoveContext: IBackgroundAddonMouseMoveContext
    {
        public BackgroundAddonProxyMouseMoveContext(MouseHookEventArgs mouseHookEventArgs)
        {
            MouseHookEventArgs = mouseHookEventArgs;
        }

        #region property

        public MouseHookEventArgs MouseHookEventArgs { get; }

        #endregion

        #region IBackgroundAddonMouseMoveContext

        public Point Location => MouseHookEventArgs.DeviceLocation;

        public DateTime Timestamp => MouseHookEventArgs.Timestamp;


        #endregion
    }

    public class BackgroundAddonProxyMouseButtonContext: IBackgroundAddonMouseButtonContext
    {
        public BackgroundAddonProxyMouseButtonContext(MouseHookEventArgs mouseHookEventArgs)
        {
            MouseHookEventArgs = mouseHookEventArgs;
        }

        #region property

        public MouseHookEventArgs MouseHookEventArgs { get; }

        #endregion

        #region IBackgroundAddonMouseButtonContext

        public MouseButton Button => MouseHookEventArgs.Button;

        public MouseButtonState State => MouseHookEventArgs.ButtonState;

        public DateTime Timestamp => MouseHookEventArgs.Timestamp;

        #endregion
    }
}
