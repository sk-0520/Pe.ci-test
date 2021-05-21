using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Addon
{
    internal class FileFinderAddonImpl: AddonBase
    {
        public FileFinderAddonImpl(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region AddonBase

        protected override IReadOnlyCollection<AddonKind> SupportedKinds { get; } = new[] {
            AddonKind.CommandFinder,
        };

        protected internal override void Load(IPluginLoadContext pluginLoadContext)
        {
        }

        protected internal override void Unload(IPluginUnloadContext pluginUnloadContext)
        {
        }

        public override ICommandFinder BuildCommandFinder(IAddonParameter parameter)
        {
            return new FileFinderCommandFinder(parameter);
        }

        #endregion
    }
}
