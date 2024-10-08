using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class BackgroundAddonContextFactory: PluginContextFactoryBase
    {
        public BackgroundAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
          : base(databaseBarrierPack, databaseDelayWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public BackgroundAddonRunStartupContext CreateRunStartupContext(IPluginInformation pluginInformation)
        {
            var context = new BackgroundAddonRunStartupContext(pluginInformation.PluginIdentifiers);
            return context;
        }

        public BackgroundAddonRunPauseContext CreateRunPauseContext(IPluginInformation pluginInformation, bool isPausing)
        {
            var context = new BackgroundAddonRunPauseContext(pluginInformation.PluginIdentifiers, isPausing);
            return context;
        }

        public BackgroundAddonRunExecuteContext CreateRunExecuteContext(IPluginInformation pluginInformation, RunExecuteKind runExecuteKind, object? parameter, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            var context = new BackgroundAddonRunExecuteContext(pluginInformation.PluginIdentifiers, runExecuteKind, parameter, timestamp);
            return context;
        }

        public BackgroundAddonRunShutdownContext CreateRunShutdownContext(IPluginInformation pluginInformation)
        {
            var context = new BackgroundAddonRunShutdownContext(pluginInformation.PluginIdentifiers);
            return context;
        }

        public BackgroundAddonKeyboardContext CreateKeyboardContext(IPluginInformation pluginInformation, KeyboardHookEventArgs keyboardHookEventArgs)
        {
            var context = new BackgroundAddonKeyboardContext(pluginInformation.PluginIdentifiers, keyboardHookEventArgs);
            return context;
        }

        public BackgroundAddonMouseMoveContext CreateMouseMoveContext(IPluginInformation pluginInformation, MouseHookEventArgs mouseHookEventArgs)
        {
            var context = new BackgroundAddonMouseMoveContext(pluginInformation.PluginIdentifiers, mouseHookEventArgs);
            return context;
        }

        public BackgroundAddonMouseButtonContext CreateMouseButtonContext(IPluginInformation pluginInformation, MouseHookEventArgs mouseHookEventArgs)
        {
            var context = new BackgroundAddonMouseButtonContext(pluginInformation.PluginIdentifiers, mouseHookEventArgs);
            return context;
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => CommonDirectoryName;

        #endregion
    }
}
