using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public abstract class PluginContextFactoryBase
    {
        protected PluginContextFactoryBase(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseLazyWriterPack = databaseLazyWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger{ get; }

        protected IDatabaseBarrierPack DatabaseBarrierPack { get; }
        protected IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        protected EnvironmentParameters EnvironmentParameters { get; }

        protected IUserAgentManager UserAgentManager { get; }

        protected abstract string BaseDirectoryName {get;}

        #endregion

        #region function

        protected string ConvertDirectoryName(IPluginIdentifiers pluginId)
        {
            return pluginId.PluginId.ToString("D");
        }

        private DirectoryInfo GetDirectory(IPluginIdentifiers pluginId, DirectoryInfo pluginDataDirectory, string baseDirectoryName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(baseDirectoryName));

            var pluginDirName = ConvertDirectoryName(pluginId);
            var dirPath = Path.Combine(pluginDataDirectory.FullName, pluginDirName, baseDirectoryName);

            return new DirectoryInfo(dirPath);
        }

        protected DirectoryInfo GetUserDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);
        protected DirectoryInfo GetMachineDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);
        protected DirectoryInfo GetTemporaryDirectory(IPluginIdentifiers pluginIdentifiers) => GetDirectory(pluginIdentifiers, EnvironmentParameters.UserPluginDataDirectory, BaseDirectoryName);


        #endregion
    }
}
