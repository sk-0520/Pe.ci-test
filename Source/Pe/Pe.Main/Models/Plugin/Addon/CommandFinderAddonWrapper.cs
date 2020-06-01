using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class CommandFinderAddonWrapper: AddonWrapperBase, ICommandFinder
    {
        #region variable

        IReadOnlyList<ICommandFinder>? _commandFinders;

        #endregion
        public CommandFinderAddonWrapper(IReadOnlyList<IAddon> addons, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, environmentParameters, userAgentManager, platformTheme, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        IReadOnlyList<ICommandFinder> CommandFinders
        {
            get
            {
                if(this._commandFinders == null) {
                    var list = new List<ICommandFinder>(Addons.Count);
                    foreach(var addon in Addons) {
                        Debug.Assert(addon.IsSupported(AddonKind.CommandFinder));
                        if(!addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                            var pluginContextFactory = new PluginContextFactory(EnvironmentParameters, UserAgentManager);
                            addon.Load(Bridge.Plugin.PluginKind.Addon, pluginContextFactory.CreateContext(addon.PluginInformations.PluginIdentifiers));
                        }
                        var commandFinder = addon.BuildCommandFinder(CreateParameter());
                        list.Add(commandFinder);
                    }
                    this._commandFinders = list;
                }

                return this._commandFinders;
            }
        }

        #endregion


        #region ICommandFinder


        public bool IsInitialize { get; private set; }


        public void Initialize()
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            foreach(var commandFinder in CommandFinders) {
                if(!commandFinder.IsInitialize) {
                    continue;
                }
                commandFinder.Initialize();
            }

            IsInitialize = true;
        }

        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            foreach(var commandFinder in CommandFinders) {
                Debug.Assert(commandFinder.IsInitialize);
                var results = commandFinder.ListupCommandItems(inputValue, inputRegex, hitValuesCreator, cancellationToken);
                foreach(var result in results) {
                    yield return result;
                }
            }
        }

        public void Refresh()
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }
        }

        #endregion
    }
}
