using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonKeyboardContext: PluginIdentifiersContextBase, IBackgroundAddonKeyboardContext
    {
        public BackgroundAddonKeyboardContext(IPluginIdentifiers pluginIdentifiers, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, Key key, bool isDown)
            : base(pluginIdentifiers)
        {
            Key = key;
            IsDown = isDown;
            TimestampUtc = timestamp;
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

    internal class BackgroundAddonMouseMoveContext: PluginIdentifiersContextBase, IBackgroundAddonMouseMoveContext
    {
        public BackgroundAddonMouseMoveContext(IPluginIdentifiers pluginIdentifiers, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, [PixelKind(Px.Device)] Point location)
            : base(pluginIdentifiers)
        {
            Location = location;
            TimestampUtc = timestamp;
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
}
