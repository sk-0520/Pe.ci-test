using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IBackgroundAddonKeyboardContext" />
    internal class BackgroundAddonKeyboardContext: PluginIdentifiersContextBase, IBackgroundAddonKeyboardContext
    {
        public BackgroundAddonKeyboardContext(IPluginIdentifiers pluginIdentifiers, KeyboardHookEventArgs keyboardHookEventArgs)
            : base(pluginIdentifiers)
        {
            Key = keyboardHookEventArgs.Key;
            IsDown = keyboardHookEventArgs.IsDown;
            TimestampUtc = keyboardHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonKeyboardContext

        /// <inheritdoc cref="IBackgroundAddonKeyboardContext.Key" />
        public Key Key { get; }
        /// <inheritdoc cref="IBackgroundAddonKeyboardContext.IsDown" />
        public bool IsDown { get; }

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.TimestampUtc" />
        [Timestamp(DateTimeKind.Utc)]
        public DateTime TimestampUtc { get; }

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonMouseMoveContext" />
    internal class BackgroundAddonMouseMoveContext: PluginIdentifiersContextBase, IBackgroundAddonMouseMoveContext
    {
        public BackgroundAddonMouseMoveContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
            : base(pluginIdentifiers)
        {
            if(mouseHookEventArgs.IsButtonEvent) {
                throw new ArgumentException(nameof(mouseHookEventArgs.IsButtonEvent));
            }

            Location = mouseHookEventArgs.DeviceLocation;
            TimestampUtc = mouseHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonMouseMoveContext

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.Location" />
        [PixelKind(Px.Device)]
        public Point Location { get; }

        /// <inheritdoc cref="IBackgroundAddonMouseMoveContext.TimestampUtc" />
        [Timestamp(DateTimeKind.Utc)]
        public DateTime TimestampUtc { get; }

        #endregion
    }

    /// <inheritdoc cref="IBackgroundAddonMouseButtonContext" />
    internal class BackgroundAddonMouseButtonContext: IBackgroundAddonMouseButtonContext
    {
        public BackgroundAddonMouseButtonContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
        {
            if(!mouseHookEventArgs.IsButtonEvent) {
                throw new ArgumentException(nameof(mouseHookEventArgs.IsButtonEvent));
            }

            Button = mouseHookEventArgs.Button;
            State = mouseHookEventArgs.ButtonState;
            TimestampUtc = mouseHookEventArgs.Timestamp;
        }

        #region IBackgroundAddonMouseButtonContext

        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.Button" />
        public MouseButton Button { get; }
        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.State" />
        public MouseButtonState State { get; }
        /// <inheritdoc cref="IBackgroundAddonMouseButtonContext.TimestampUtc" />
        [Timestamp(DateTimeKind.Utc)]
        public DateTime TimestampUtc { get; }

        #endregion

    }
}
