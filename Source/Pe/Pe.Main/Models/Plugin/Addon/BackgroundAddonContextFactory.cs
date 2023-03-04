using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonContextFactory: PluginContextFactoryBase
    {
        public BackgroundAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
          : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public BackgroundAddonRunStartupContext CreateRunStartupContext(IPluginInformations pluginInformations)
        {
            var context = new BackgroundAddonRunStartupContext(pluginInformations.PluginIdentifiers);
            return context;
        }

        public BackgroundAddonRunPauseContext CreateRunPauseContext(IPluginInformations pluginInformations, bool isPausing)
        {
            var context = new BackgroundAddonRunPauseContext(pluginInformations.PluginIdentifiers, isPausing);
            return context;
        }

        public BackgroundAddonRunExecuteContext CreateRunExecuteContext(IPluginInformations pluginInformations, RunExecuteKind runExecuteKind, object? parameter, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            var context = new BackgroundAddonRunExecuteContext(pluginInformations.PluginIdentifiers, runExecuteKind, parameter, timestamp);
            return context;
        }

        public BackgroundAddonRunShutdownContext CreateRunShutdownContext(IPluginInformations pluginInformations)
        {
            var context = new BackgroundAddonRunShutdownContext(pluginInformations.PluginIdentifiers);
            return context;
        }

        public BackgroundAddonKeyboardContext CreateKeyboardContext(IPluginInformations pluginInformations, KeyboardHookEventArgs keyboardHookEventArgs)
        {
            var context = new BackgroundAddonKeyboardContext(pluginInformations.PluginIdentifiers, keyboardHookEventArgs);
            return context;
        }


        public BackgroundAddonMouseMoveContext CreateMouseMoveContex(IPluginInformations pluginInformations, MouseHookEventArgs mouseHookEventArgs)
        {
            var context = new BackgroundAddonMouseMoveContext(pluginInformations.PluginIdentifiers, mouseHookEventArgs);
            return context;
        }

        public BackgroundAddonMouseButtonContext CreateMouseButtonContex(IPluginInformations pluginInformations, MouseHookEventArgs mouseHookEventArgs)
        {
            var context = new BackgroundAddonMouseButtonContext(pluginInformations.PluginIdentifiers, mouseHookEventArgs);
            return context;
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => CommonDirectoryName;

        #endregion
    }
}
