using System;
using System.Diagnostics;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginDirectoryManager
    {
        public PluginDirectoryManager(EnvironmentParameters environmentParameters)
            : this(string.Empty, environmentParameters)
        { }

        public PluginDirectoryManager(string baseDirectoryName, EnvironmentParameters environmentParameters)
        {
            BaseDirectoryName = baseDirectoryName ?? throw new ArgumentNullException(nameof(baseDirectoryName));
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
        string BaseDirectoryName { get; }

        #endregion

        #region function

        public DirectoryInfo GetModuleDirectory(IPluginIdentifiers pluginIdentifiers)
        {
            var nameDirPath = Path.Combine(EnvironmentParameters.MachinePluginModuleDirectory.FullName, pluginIdentifiers.PluginName);
            if(Directory.Exists(nameDirPath)) {
                return new DirectoryInfo(nameDirPath);
            }

            return EnvironmentParameters.MachinePluginModuleDirectory.CreateSubdirectory(PluginUtility.ConvertDirectoryName(pluginIdentifiers));
        }


        private DirectoryInfo GetDirectory(IPluginIdentifiers pluginId, DirectoryInfo pluginDataDirectory, string baseDirectoryName)
        {
            Debug.Assert(baseDirectoryName != null);

            var pluginDirName = PluginUtility.ConvertDirectoryName(pluginId);
            var dirPath = Path.Combine(pluginDataDirectory.FullName, pluginDirName, baseDirectoryName);

            return new DirectoryInfo(dirPath);
        }

        public DirectoryInfo GetUserDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);
        public DirectoryInfo GetMachineDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);
        public DirectoryInfo GetTemporaryDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);


        #endregion
    }
}
