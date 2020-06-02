using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Addon
{
    internal class FileFinderAddonImpl: AddonBase
    {
        public FileFinderAddonImpl(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        { }

        protected override IReadOnlyCollection<AddonKind> SupportedKinds { get; } = new[] {
            AddonKind.CommandFinder,
        };

        protected internal override void Load(IPluginContext pluginContext)
        {
        }

        protected internal override void Unload(IPluginContext pluginContext)
        {
        }

        public override ICommandFinder BuildCommandFinder(IAddonParameter parameter)
        {
            return new FileCommandFinder(parameter);
        }
    }
}
