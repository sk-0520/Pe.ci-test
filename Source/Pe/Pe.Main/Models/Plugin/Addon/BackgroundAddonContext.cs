using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IBackgroundAddonRunStartupContext" />
    public class BackgroundAddonRunStartupContext: PluginIdentifiersContextBase, IBackgroundAddonRunStartupContext
    {
        public BackgroundAddonRunStartupContext(IPluginIdentifiers pluginIdentifiers)
            : base(pluginIdentifiers)
        { }

        #region IBackgroundAddonRunStartupContext

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonRunPauseContext" />
    public class BackgroundAddonRunPauseContext: PluginIdentifiersContextBase, IBackgroundAddonRunPauseContext
    {
        public BackgroundAddonRunPauseContext(IPluginIdentifiers pluginIdentifiers, bool isPausing)
             : base(pluginIdentifiers)
        {
            IsPausing = isPausing;
        }

        #region IBackgroundAddonRunPauseContext

        /// <inheritdoc cref="IBackgroundAddonRunPauseContext.IsPausing" />
        public bool IsPausing { get; }

        #endregion
    }

    public class BackgroundAddonRunExecuteContext: PluginIdentifiersContextBase, IBackgroundAddonRunExecuteContext
    {
        public BackgroundAddonRunExecuteContext(IPluginIdentifiers pluginIdentifiers, RunExecuteKind runExecuteKind, object? parameter, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
            : base(pluginIdentifiers)
        {
            RunExecuteKind = runExecuteKind;
            Parameter = parameter;
            Timestamp = timestamp;
        }

        #region IBackgroundAddonRunExecuteContext

        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.RunExecuteKind"/>
        public RunExecuteKind RunExecuteKind { get; }

        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.Parameter"/>
        public object? Parameter { get; }

        /// <inheritdoc cref="IBackgroundAddonRunExecuteContext.Timestamp"/>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }


    /// <inheritdoc cref="IBackgroundAddonRunShutdownContext" />
    public class BackgroundAddonRunShutdownContext: PluginIdentifiersContextBase, IBackgroundAddonRunShutdownContext
    {
        public BackgroundAddonRunShutdownContext(IPluginIdentifiers pluginIdentifiers)
            : base(pluginIdentifiers)
        { }

        #region IBackgroundAddonRunShutdownContext

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonKeyboardContext" />
    public class BackgroundAddonKeyboardContext: PluginIdentifiersContextBase, IBackgroundAddonKeyboardContext
    {
        public BackgroundAddonKeyboardContext(IPluginIdentifiers pluginIdentifiers, KeyboardHookEventArgs keyboardHookEventArgs)
            : base(pluginIdentifiers)
        {
            Key = keyboardHookEventArgs.Key;
            IsDown = keyboardHookEventArgs.IsDown;
            Timestamp = keyboardHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonKeyboardContext

        /// <inheritdoc cref="IBackgroundAddonKeyboardContext.Key" />
        public Key Key { get; }
        /// <inheritdoc cref="IBackgroundAddonKeyboardContext.IsDown" />
        public bool IsDown { get; }

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.Timestamp" />
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonMouseMoveContext" />
    public class BackgroundAddonMouseMoveContext: PluginIdentifiersContextBase, IBackgroundAddonMouseMoveContext
    {
        public BackgroundAddonMouseMoveContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
            : base(pluginIdentifiers)
        {
            if(mouseHookEventArgs.IsButtonEvent) {
                throw new ArgumentException(null, nameof(mouseHookEventArgs.IsButtonEvent));
            }

            Location = mouseHookEventArgs.DeviceLocation;
            Timestamp = mouseHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonMouseMoveContext

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.Location" />
        [PixelKind(Px.Device)]
        public Point Location { get; }

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.Timestamp" />
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonMouseButtonContext" />
    public class BackgroundAddonMouseButtonContext: IBackgroundAddonMouseButtonContext
    {
        public BackgroundAddonMouseButtonContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
        {
            if(!mouseHookEventArgs.IsButtonEvent) {
                throw new ArgumentException(null, nameof(mouseHookEventArgs.IsButtonEvent));
            }

            Button = mouseHookEventArgs.Button;
            State = mouseHookEventArgs.ButtonState;
            Timestamp = mouseHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonMouseButtonContext

        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.Button" />
        public MouseButton Button { get; }
        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.State" />
        public MouseButtonState State { get; }
        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.Timestamp" />
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }
}
