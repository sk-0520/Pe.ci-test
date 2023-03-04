using System;
using System.Diagnostics;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Standard.Database;
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

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        protected IDatabaseBarrierPack DatabaseBarrierPack { get; }
        protected IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
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

        protected virtual PluginFiles CreatePluginFile(IPluginInformations pluginInformations)
        {

            var pluginFile = new PluginFiles(
                new PluginFileStorage(GetUserDirectory(pluginInformations.PluginIdentifiers)),
                new PluginFileStorage(GetMachineDirectory(pluginInformations.PluginIdentifiers)),
                new PluginFileStorage(GetTemporaryDirectory(pluginInformations.PluginIdentifiers))
            );

            return pluginFile;
        }

        /// <summary>
        /// 上流依存DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseContextsPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseContextsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        /// <summary>
        /// 逐次実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentBarrier(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, bool isReadOnly)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistent;
        }

        /// <summary>
        /// 遅延実行DBアクセス処理の生成。
        /// </summary>
        /// <param name="pluginInformations"></param>
        /// <param name="databaseBarrierPack"></param>
        /// <param name="databaseLazyWriterPack"></param>
        /// <returns></returns>
        protected virtual PluginPersistents CrteatePluginPersistentLazyWriter(IPluginInformations pluginInformations, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack)
        {
            var pluginPersistent = new PluginPersistents(
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Main, databaseLazyWriterPack.Main, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Large, databaseLazyWriterPack.Large, DatabaseStatementLoader, LoggerFactory),
                new PluginPersistentStorage(pluginInformations.PluginIdentifiers, pluginInformations.PluginVersions, databaseBarrierPack.Temporary, databaseLazyWriterPack.Temporary, DatabaseStatementLoader, LoggerFactory)
            );

            return pluginPersistent;
        }

        protected virtual PluginStorage CreatePluginStorage(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = new PluginStorage(
                CreatePluginFile(pluginInformations),
                CrteatePluginPersistentContext(pluginInformations, databaseContextsPack, isReadOnly)
            );

            return pluginStorage;
        }

        public PluginContext CreateContext(IPluginInformations pluginInformations, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = CreatePluginStorage(pluginInformations, databaseContextsPack, isReadOnly);
            return new PluginContext(pluginInformations.PluginIdentifiers, pluginStorage);
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
