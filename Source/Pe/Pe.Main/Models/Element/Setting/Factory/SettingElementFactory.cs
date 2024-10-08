using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting.Factory
{
    public interface ISettingElementFactory
    {
        #region function

        GeneralsSettingEditorElement CreateGeneralsSettingEditorElement(IEnumerable<IPlugin> themePlugins);
        LauncherItemsSettingEditorElement CreateLauncherItemsSettingEditorElement(ObservableCollection<LauncherItemSettingEditorElement> allLauncherItems, PluginContainer pluginContainer);
        LauncherGroupsSettingEditorElement CreateLauncherGroupsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups);
        LauncherToolbarsSettingEditorElement CreateLauncherToolbarsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups);
        KeyboardSettingEditorElement CreateKeyboardSettingEditorElement();
        PluginsSettingEditorElement CreatePluginsSettingEditorElement(PluginContainer pluginContainer, PauseReceiveLogDelegate pauseReceiveLog);

        #endregion
    }

    public class SettingElementFactory: DiObjectFactoryBase, ISettingElementFactory
    {
        public SettingElementFactory(IDiContainer diContainer)
            : base(diContainer)
        { }

        #region ISettingElementFactory

        public GeneralsSettingEditorElement CreateGeneralsSettingEditorElement(IEnumerable<IPlugin> themePlugins)
        {
            return DiContainer.Build<GeneralsSettingEditorElement>(themePlugins.ToArray());
        }

        public LauncherItemsSettingEditorElement CreateLauncherItemsSettingEditorElement(ObservableCollection<LauncherItemSettingEditorElement> allLauncherItems, PluginContainer pluginContainer)
        {
            return DiContainer.Build<LauncherItemsSettingEditorElement>(allLauncherItems, pluginContainer);
        }

        public LauncherGroupsSettingEditorElement CreateLauncherGroupsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups)
        {
            return DiContainer.Build<LauncherGroupsSettingEditorElement>(allLauncherGroups);
        }

        public LauncherToolbarsSettingEditorElement CreateLauncherToolbarsSettingEditorElement(ObservableCollection<LauncherGroupSettingEditorElement> allLauncherGroups)
        {
            return DiContainer.Build<LauncherToolbarsSettingEditorElement>(allLauncherGroups);
        }

        public KeyboardSettingEditorElement CreateKeyboardSettingEditorElement()
        {
            return DiContainer.Build<KeyboardSettingEditorElement>();
        }

        public PluginsSettingEditorElement CreatePluginsSettingEditorElement(PluginContainer pluginContainer, PauseReceiveLogDelegate pauseReceiveLog)
        {
            return new PluginsSettingEditorElement(
                pluginContainer,
                DiContainer.Build<NewVersionChecker>(),
                DiContainer.Build<NewVersionDownloader>(),
                DiContainer.Build<PluginConstructorContext>(),
                pauseReceiveLog,
                DiContainer.Build<PreferencesContextFactory>(),
                DiContainer.Build<IWindowManager>(),
                DiContainer.Build<IUserTracker>(),
                DiContainer.Build<ISettingNotifyManager>(),
                DiContainer.Build<IClipboardManager>(),
                DiContainer.Build<IMainDatabaseBarrier>(),
                DiContainer.Build<ILargeDatabaseBarrier>(),
                DiContainer.Build<ITemporaryDatabaseBarrier>(),
                DiContainer.Build<IDatabaseStatementLoader>(),
                DiContainer.Build<IIdFactory>(),
                DiContainer.Build<EnvironmentParameters>(),
                DiContainer.Build<GeneralConfiguration>(),
                DiContainer.Build<ApiConfiguration>(),
                DiContainer.Build<IUserAgentManager>(),
                DiContainer.Build<IViewManager>(),
                DiContainer.Build<IPlatformTheme>(),
                DiContainer.Build<IImageLoader>(),
                DiContainer.Build<IMediaConverter>(),
                DiContainer.Build<IPolicy>(),
                DiContainer.Build<IDispatcherWrapper>(),
                DiContainer.Build<ILoggerFactory>()
            );
        }

        #endregion
    }
}
