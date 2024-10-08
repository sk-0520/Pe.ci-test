using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonContextFactory: PluginContextFactoryBase
    {
        public LauncherItemAddonContextFactory(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(databaseBarrierPack, databaseDelayWriterPack, databaseStatementLoader, environmentParameters, userAgentManager, loggerFactory)
        {
            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IViewManager ViewManager { get; }
        private IPlatformTheme PlatformTheme { get; }
        private IImageLoader ImageLoader { get; }
        private IMediaConverter MediaConverter { get; }
        private IPolicy Policy { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region function

        public LauncherItemAddonContextWorker CreateWorker(IPluginInformation pluginInformation, LauncherItemId launcherItemId)
        {
            return new LauncherItemAddonContextWorker(this, pluginInformation, launcherItemId, LoggerFactory);
        }

        private LauncherItemAddonFiles CreateLauncherItemAddonFile(IPluginInformation pluginInformation)
        {
            var dirName = ConvertDirectoryName(pluginInformation.PluginIdentifiers);

            var pluginFile = new LauncherItemAddonFiles(
                new LauncherItemAddonFileStorage(GetUserDirectory(pluginInformation.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetMachineDirectory(pluginInformation.PluginIdentifiers)),
                new LauncherItemAddonFileStorage(GetTemporaryDirectory(pluginInformation.PluginIdentifiers))
            );

            return pluginFile;
        }

        private LauncherItemAddonPersistence CreateLauncherItemAddonPersistenceContext(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginPersistence = new LauncherItemAddonPersistence(
                new LauncherItemAddonPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Main, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Large, DatabaseStatementLoader, isReadOnly, LoggerFactory),
                new LauncherItemAddonPersistenceStorage(pluginInformation.PluginIdentifiers, pluginInformation.PluginVersions, databaseContextsPack.Temporary, DatabaseStatementLoader, isReadOnly, LoggerFactory)
            );

            return pluginPersistence;
        }

        private LauncherItemAddonStorage CreateLauncherItemAddonStorage(IPluginInformation pluginInformation, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var pluginStorage = new LauncherItemAddonStorage(
                CreateLauncherItemAddonFile(pluginInformation),
                CreateLauncherItemAddonPersistenceContext(pluginInformation, databaseContextsPack, isReadOnly)
            );

            return pluginStorage;
        }

        public LauncherItemAddonContext CreateContext(IPluginInformation pluginInformation, LauncherItemId launcherItemId, IDatabaseContextsPack databaseContextsPack, bool isReadOnly)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformation, databaseContextsPack, isReadOnly);
            return new LauncherItemAddonContext(pluginInformation.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesLoadContext CreatePreferencesLoadContext(IPluginInformation pluginInformation, LauncherItemId launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformation, databaseContextsPack, true);
            return new LauncherItemPreferencesLoadContext(pluginInformation.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesCheckContext CreatePreferencesCheckContext(IPluginInformation pluginInformation, LauncherItemId launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformation, databaseContextsPack, true);
            return new LauncherItemPreferencesCheckContext(pluginInformation.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesSaveContext CreatePreferencesSaveContext(IPluginInformation pluginInformation, LauncherItemId launcherItemId, IDatabaseContextsPack databaseContextsPack)
        {
            var launcherItemAddonStorage = CreateLauncherItemAddonStorage(pluginInformation, databaseContextsPack, false);
            return new LauncherItemPreferencesSaveContext(pluginInformation.PluginIdentifiers, launcherItemId, launcherItemAddonStorage);
        }

        public LauncherItemPreferencesEndContext CreatePreferencesEndContext(IPluginInformation pluginInformation, LauncherItemId launcherItemId)
        {
            return new LauncherItemPreferencesEndContext(pluginInformation.PluginIdentifiers, launcherItemId);
        }

        /// <summary>
        /// ここにあるのは設計ミス！
        /// </summary>
        /// <param name="pluginInformation"></param>
        /// <param name="launcherItemId"></param>
        /// <returns></returns>
        internal LauncherItemExtensionExecuteParameter CreateExtensionExecuteParameter(IPluginInformation pluginInformation, LauncherItemId launcherItemId, ILauncherItemAddonViewSupporter launcherItemAddonViewSupporter)
        {
            var launcherItemExtensionExecuteParameter = new LauncherItemExtensionExecuteParameter(
                launcherItemId,
                launcherItemAddonViewSupporter,
                new SkeletonImplements(),
                pluginInformation,
                UserAgentManager,
                ViewManager,
                PlatformTheme,
                ImageLoader,
                MediaConverter,
                Policy,
                DispatcherWrapper,
                LoggerFactory
            );
            return launcherItemExtensionExecuteParameter;
        }

        #endregion

        #region PluginContextFactoryBase

        protected override string BaseDirectoryName => "launcher-item-data";

        #endregion
    }
}
