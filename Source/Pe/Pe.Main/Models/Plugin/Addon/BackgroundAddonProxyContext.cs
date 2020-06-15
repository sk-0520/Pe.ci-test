using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonProxyKeyboardContext: BackgroundAddonKeyboardContext
    {
        public BackgroundAddonProxyKeyboardContext(KeyboardHookEventArgs keyboardHookEventArgs)
            : base(new NullPluginInformation().PluginIdentifiers, keyboardHookEventArgs)
        { }
    }

    internal class BackgroundAddonProxyMouseMoveContext: BackgroundAddonMouseMoveContext
    {
        public BackgroundAddonProxyMouseMoveContext(MouseHookEventArgs mouseHookEventArgs)
            : base(new NullPluginInformation().PluginIdentifiers, mouseHookEventArgs)
        { }
    }

    internal class BackgroundAddonProxyMouseButtonContext: BackgroundAddonMouseButtonContext
    {
        public BackgroundAddonProxyMouseButtonContext(IPluginIdentifiers pluginIdentifiers, MouseHookEventArgs mouseHookEventArgs)
            : base(new NullPluginInformation().PluginIdentifiers, mouseHookEventArgs)
        { }
    }
}
