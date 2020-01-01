using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginStore
    {
        public PluginStore(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ISet<IPlugin> Plugins { get; } = new HashSet<IPlugin>();

        #endregion

        #region function

        public IEnumerable<IPlugin> GetPlugins()
        {
            yield return new DefaultTheme();
        }

        public void AddPlugin(IPlugin plugin)
        {
            Plugins.Add(plugin);
        }

        #endregion
    }
}
