using System;
using System.Diagnostics;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public abstract class PluginContextFactoryBase
    {
        protected PluginContextFactoryBase(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseDelayWriterPack = databaseDelayWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        protected IDatabaseBarrierPack DatabaseBarrierPack { get; }
        protected IDatabaseDelayWriterPack DatabaseDelayWriterPack { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        protected EnvironmentParameters EnvironmentParameters { get; }

        protected IUserAgentManager UserAgentManager { get; }

        protected abstract string BaseDirectoryName { get; }

        protected string CommonDirectoryName { get; } = "data";
        protected string LauncherItemDirectoryName { get; } = "launcher-item";
        protected string WidgetDirectoryName { get; } = "widget";

        #endregion

        #region function

        protected string ConvertDirectoryName(IPluginIdentifiers pluginId)
        {
            return pluginId.PluginId.ToString();
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


        public IDatabaseContextsPack BarrierRead() => DatabaseBarrierPack.WaitRead();
        public IDatabaseContextsPack BarrierWrite() => DatabaseBarrierPack.WaitWrite();

        protected virtual PluginFiles CreatePluginFile(IPluginInformation pluginInformation)
        {

            var pluginFile = new PluginFiles(
                new PluginFileStorage(GetUserDirectory(pluginInformation.PluginIdentifiers)),
                new PluginFileStorage(GetMachineDirectory(pluginInformation.PluginIdentifiers)),
                new PluginFileStorage(GetTemporaryDirectory(pluginInformation.PluginIdentifiers))
            );

            return pluginFile;
        }

        /// <summary>
        /// 上流依存DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformation"></param>
        /// <param name="databaseContextsPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistence CreatePluginPersistenceContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginPersistence = new PluginPersistence(
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistence;
        }

        /// <summary>
        /// 逐次実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformation"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistence CreatePluginPersistenceBarrier(IPluginInformation pluginInformation, IDatabaseBarrierPack databaseBarrierPack, bool isReadOnly)
        {
            var pluginPersistence = new PluginPersistence(
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistence;
        }

        /// <summary>
        /// 遅延実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformation"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="databaseDelayWriterPack"></param>
        /// <returns></returns>
        protected virtual PluginPersistence CreatePluginPersistenceDelayWriter(IPluginInformation pluginInformation, IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack)
        {
            var pluginPersistence = new PluginPersistence(
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Main, databaseDelayWriterPack.Main, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Large, databaseDelayWriterPack.Large, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseBarrierPack.Temporary, databaseDelayWriterPack.Temporary, DatabaseStatementLoader, LoggerFactory)
            );

            return pluginPersistence;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginInformation),
                CreatePluginPersistenceContext(pluginInformation, databaseContextsPack, isReadOnly)
            );

            return pluginStorage;
        }

        public PluginContext CreateContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginInformation, databaseContextsPack, isReadOnly);
            return new PluginContext(pluginInformation.PluginIdentifiers, pluginStorage);
        }

        static internal NullPluginContext CreateNullContext(ILoggerFactory loggerFactory)
        {
            var nullPluginInformation = new NullPluginInformation();
            var nullStorage = new NullPluginStorage(nullPluginInformation.PluginIdentifiers, loggerFactory);
            return new NullPluginContext(nullStorage);
        }

        /// <summary>
        /// ミスった。。。
        /// </summary>
        public void Save()
        {
            DatabaseBarrierPack.Save();
        }

        #endregion
    }
}
