using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal sealed class CommandFinderAddonProxy: AddonsProxyBase<ICommandFinder>, ICommandFinder
    {
        public CommandFinderAddonProxy(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        #endregion

        #region AddonWrapperBase

        protected override AddonKind AddonKind => AddonKind.CommandFinder;

        protected override ICommandFinder BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildCommandFinder(CreateParameter(loadedAddon));
        }


        #endregion

        #region ICommandFinder


        public bool IsInitialized { get; private set; }


        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var addonFunctions in FunctionUnits) {
                if(addonFunctions.IsInitialized) {
                    continue;
                }
                addonFunctions.Initialize();
            }

            IsInitialized = true;
        }

        public async IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var addonFunctions in FunctionUnits) {
                Debug.Assert(addonFunctions.IsInitialized);
                var results = addonFunctions.EnumerateCommandItemsAsync(inputValue, inputRegex, hitValuesCreator, cancellationToken);
                await foreach(var result in results) {
                    yield return result;
                }
            }
        }

        public void Refresh(IPluginContext pluginContext)
        {
            Debug.Assert(pluginContext.GetType() == typeof(NullPluginContext));

            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            foreach(var functionUnit in FunctionUnits) {
                var addon = GetAddon(functionUnit);
                using(var reader = PluginContextFactory.BarrierRead()) {
                    var context = PluginContextFactory.CreateContext(addon.PluginInformation, reader, true);
                    functionUnit.Refresh(context);
                }
            }
        }

        #endregion
    }
}
