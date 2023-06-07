using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class NullPluginInformation: PluginInformation
    {
        public NullPluginInformation()
            : base(
                  new PluginIdentifiers(PluginId.Empty, "NullPlugin"),
                  new PluginVersions(new Version(), new Version(), new Version(), Array.Empty<string>()),
                  new PluginAuthors(new Author("Pe"), PluginLicense.Unknown)
            )
        { }
    }
}
