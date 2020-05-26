using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginLoadContext: AssemblyLoadContext
    {
        public PluginLoadContext(FileInfo pluginFile)
            : this(pluginFile, true)
        { }
        public PluginLoadContext(FileInfo pluginFile, bool isCollectible)
            : base(isCollectible)
        {
            AssemblyDependencyResolver = new AssemblyDependencyResolver(pluginFile.FullName);
        }

        #region property

        AssemblyDependencyResolver AssemblyDependencyResolver { get; }

        #endregion

        #region function

        #endregion

        #region AssemblyLoadContext

        #endregion
    }
}
