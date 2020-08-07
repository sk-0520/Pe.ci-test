using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public abstract class ContextWorkerBase<TPluginContextFactory>
        where TPluginContextFactory: PluginContextFactoryBase
    {
        protected ContextWorkerBase(TPluginContextFactory pluginContextFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginContextFactory = pluginContextFactory;
        }

        #region property

        protected TPluginContextFactory PluginContextFactory { get; }
        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion
    }

    public class ContextWorker: ContextWorkerBase<PluginContextFactory>, IContextWorker
    {
        public ContextWorker(PluginContextFactory pluginContextFactory, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, loggerFactory)
        { }

        #region IContextWorker

        public void RunPlugin(Action<IPluginContext> callback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class LauncherItemAddonContextWorker: ContextWorkerBase<LauncherItemAddonContextFactory>, ILauncherItemAddonContextWorker
    {
        public LauncherItemAddonContextWorker(LauncherItemAddonContextFactory launcherItemAddonContextFactory, ILoggerFactory loggerFactory)
            : base(launcherItemAddonContextFactory, loggerFactory)
        { }

        #region ILauncherItemAddonContextWorker

        public void RunLauncherItemAddon(Action<ILauncherItemAddonContext> callback)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
