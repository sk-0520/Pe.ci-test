using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class CommandFinderAddonWrapper: AddonWrapperBase<ICommandFinder>, ICommandFinder
    {
        public CommandFinderAddonWrapper(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, platformTheme, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        #endregion

        #region AddonWrapperBase

        protected override AddonKind AddonKind => AddonKind.CommandFinder;

        protected override ICommandFinder BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildCommandFinder(CreateParameter());
        }


        #endregion


        #region ICommandFinder


        public bool IsInitialize { get; private set; }


        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            foreach(var addonFunctions in FunctionUnits) {
                if(addonFunctions.IsInitialize) {
                    continue;
                }
                addonFunctions.Initialize();
            }

            IsInitialize = true;
        }

        public IEnumerable<ICommandItem> EnumerateCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            foreach(var addonFunctions in FunctionUnits) {
                Debug.Assert(addonFunctions.IsInitialize);
                var results = addonFunctions.EnumerateCommandItems(inputValue, inputRegex, hitValuesCreator, cancellationToken);
                foreach(var result in results) {
                    yield return result;
                }
            }
        }

        public void Refresh(IPluginContext pluginContext)
        {
            Debug.Assert(pluginContext.GetType() == typeof(NullPluginContext));

            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            foreach(var functionUnit in FunctionUnits) {
                var addon = GetAddon(functionUnit);
                using(var reader = PluginContextFactory.BarrierRead()) {
                    var context = PluginContextFactory.CreateContext(addon.PluginInformations, reader, true);
                    functionUnit.Refresh(context);
                }
            }
        }

        #endregion
    }
}
