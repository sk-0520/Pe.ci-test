using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン読み込み用処理。
    /// </summary>
    public class PluginAssemblyLoadContext: AssemblyLoadContext
    {
        public PluginAssemblyLoadContext(FileInfo pluginFile, ILoggerFactory loggerFactory)
            : this(pluginFile, true, loggerFactory)
        { }

        public PluginAssemblyLoadContext(FileInfo pluginFile, bool isCollectible, ILoggerFactory loggerFactory)
            : base(isCollectible)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginFile = pluginFile;
            AssemblyDependencyResolver = new AssemblyDependencyResolver(Path.GetDirectoryName(PluginFile.FullName)!);
        }

        #region property

        FileInfo PluginFile { get; }
        ILogger Logger { get; }
        AssemblyDependencyResolver AssemblyDependencyResolver { get; }

        #endregion

        #region function

        public Assembly Load()
        {
            return LoadFromAssemblyPath(PluginFile.FullName);
        }

        #endregion

        #region AssemblyLoadContext

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = AssemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
            if(assemblyPath != null) {
                Logger.LogDebug("[{0}] 解決 {1}, {2}", PluginFile.Name, assemblyName, assemblyPath);
                return LoadFromAssemblyPath(assemblyPath);
            }

            Logger.LogDebug("[{0}] 未解決1 {1}, {2}", PluginFile.Name, assemblyName, assemblyPath);

            var result =  base.Load(assemblyName);

            Logger.LogDebug("[{0}] 未解決2 {1}, {2}", PluginFile.Name, assemblyName, result);

            return result;
        }

        #endregion
    }
}
