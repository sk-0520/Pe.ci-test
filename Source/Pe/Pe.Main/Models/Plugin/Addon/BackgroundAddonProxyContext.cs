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
    internal class BackgroundAddonProxyKeyboardContext: IBackgroundAddonKeyboardContext
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

        public DateTime TimestampUtc => KeyboardHookEventArgs.Timestamp;

        #endregion
    }

    internal class BackgroundAddonProxyMouseMoveContext: IBackgroundAddonMouseMoveContext
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

        public DateTime TimestampUtc => MouseHookEventArgs.Timestamp;


        #endregion
    }

    internal class BackgroundAddonProxyMouseButtonContext: IBackgroundAddonMouseButtonContext
    {
        public BackgroundAddonProxyMouseButtonContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
        {
            MouseHookEventArgs = mouseHookEventArgs;
        }

        #region property

        public MouseHookEventArgs MouseHookEventArgs { get; }

        #endregion


        #region IBackgroundAddonMouseButtonContext

        public MouseButton Button => MouseHookEventArgs.Button;

        public MouseButtonState State => MouseHookEventArgs.ButtonState;

        public DateTime TimestampUtc => MouseHookEventArgs.Timestamp;

        #endregion
    }
}
