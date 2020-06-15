using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン読み込み用処理。
    /// </summary>
    public class PluginAssemblyLoadContext: AssemblyLoadContext
    {
        public PluginAssemblyLoadContext(FileInfo pluginFile)
            : this(pluginFile, true)
        { }
        public PluginAssemblyLoadContext(FileInfo pluginFile, bool isCollectible)
            : base(isCollectible)
        {
            PluginFile = pluginFile;
            AssemblyDependencyResolver = new AssemblyDependencyResolver(PluginFile.FullName);
        }

        #region property

        FileInfo PluginFile { get; }
        AssemblyDependencyResolver AssemblyDependencyResolver { get; }

        #endregion

        #region function

        public Assembly Load()
        {
            return LoadFromAssemblyPath(PluginFile.FullName);
        }

        #endregion

        #region AssemblyLoadContext

        #endregion
    }
}
