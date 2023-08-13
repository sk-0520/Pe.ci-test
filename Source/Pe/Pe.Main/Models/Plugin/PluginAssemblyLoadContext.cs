using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン読み込み用処理。
    /// </summary>
    public class PluginAssemblyLoadContext: AssemblyLoadContext
    {
        public PluginAssemblyLoadContext(FileInfo pluginFile, IReadOnlyList<DirectoryInfo> libraryDirectories, ILoggerFactory loggerFactory)
            : this(pluginFile, libraryDirectories, true, loggerFactory)
        { }

        public PluginAssemblyLoadContext(FileInfo pluginFile, IReadOnlyList<DirectoryInfo> libraryDirectories, bool isCollectible, ILoggerFactory loggerFactory)
            : base(isCollectible)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginFile = pluginFile;
            AssemblyDependencyResolver = new AssemblyDependencyResolver(PluginFile.FullName);
            LibraryDirectories = libraryDirectories;
        }

        #region property

        private FileInfo PluginFile { get; }
        private ILogger Logger { get; }
        private AssemblyDependencyResolver AssemblyDependencyResolver { get; }
        private IReadOnlyList<DirectoryInfo> LibraryDirectories { get; }

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
            var assemblyPathFromPlugin = AssemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
            if(assemblyPathFromPlugin != null) {
                //Logger.LogDebug("[{0}] 解決[plugin] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromPlugin);
                return LoadFromAssemblyPath(assemblyPathFromPlugin);
            }

            //foreach(var libraryDirectory in LibraryDirectories) {
            //    var resolver = new AssemblyDependencyResolver(libraryDirectory.FullName);
            //    var assemblyPathFromLibrary = resolver.ResolveAssemblyToPath(assemblyName);
            //    if(assemblyPathFromLibrary != null) {
            //        Logger.LogDebug("[{0}] 解決[library1] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromLibrary);
            //        return LoadFromAssemblyPath(assemblyPathFromLibrary);
            //    }
            //}

            var assemblyPathFromBase = base.Load(assemblyName);
            if(assemblyPathFromBase != null) {
                //Logger.LogDebug("[{0}] 解決[base] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromBase);
                return assemblyPathFromBase;
            }

            //// アセンブリバージョンとかガン無視
            //foreach(var libraryDirectory in LibraryDirectories) {
            //    var name = assemblyName.Name;
            //    var targets = new[] {
            //        "Pe.Bridge",
            //        "Pe.Core",
            //        "Pe.Main",
            //    };
            //    if(targets.Any(i => i == name)) {
            //        var assemblyPathFromLibrary = Path.Combine(libraryDirectory.FullName, PathUtility.AppendExtension(assemblyName.Name!, "dll"));
            //        if(File.Exists(assemblyPathFromLibrary)) {
            //            Logger.LogDebug("[{0}] 解決[library2] {1}, {2}", PluginFile.Name, assemblyName, assemblyPathFromLibrary);
            //            return LoadFromAssemblyPath(assemblyPathFromLibrary);
            //        }
            //    }
            //}

            //Logger.LogDebug("[{0}] 未解決 {1}", PluginFile.Name, assemblyName);
            return assemblyPathFromBase;
        }

        #endregion
    }
}
