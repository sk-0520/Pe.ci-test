using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    internal sealed class WebViewWidgetCallbacks
    {
        #region event

        public event EventHandler? MoveStarted;

        #endregion
        public WebViewWidgetCallbacks(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property
        IPluginIdentifiers PluginIdentifiers { get; }
        ILogger Logger { get; }
        #endregion

        #region function

        void OnMoveStarted()
        {
            MoveStarted?.Invoke(this, EventArgs.Empty);
        }

        public void MoveStart()
        {
            OnMoveStarted();
        }

        #endregion
    }
}
