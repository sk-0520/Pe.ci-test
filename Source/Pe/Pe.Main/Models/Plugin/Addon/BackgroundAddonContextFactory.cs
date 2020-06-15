using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonContextFactory: PluginContextFactoryBase
    {
        public BackgroundAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
          : base(databaseBarrierPack, databaseLazyWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        { }

        #region function

        public BackgroundAddonKeyboardContext CreateKeyboardContext(IPluginInformations pluginInformations, KeyboardHookEventArgs keyboardHookEventArgs, IDatabaseCommandsPack databaseCommandsPack)
        {
            var context = new BackgroundAddonKeyboardContext(pluginInformations.PluginIdentifiers, keyboardHookEventArgs.Key, keyboardHookEventArgs.IsDown);
            return context;
        }


        public BackgroundAddonMouseMoveContext CreateMouseMoveContex(IPluginInformations pluginInformations, [PixelKind(Px.Device)] Point location, IDatabaseCommandsPack databaseCommandsPack)
        {
            var context = new BackgroundAddonMouseMoveContext(pluginInformations.PluginIdentifiers, location);
            return context;
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "data";

        #endregion

    }
}
