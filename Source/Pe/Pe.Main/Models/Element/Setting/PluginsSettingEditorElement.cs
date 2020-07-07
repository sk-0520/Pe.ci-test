using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginsSettingEditorElement: SettingEditorElementBase
    {
        internal PluginsSettingEditorElement(PluginContainer pluginContainer, PreferencesContextFactory preferencesContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, imageLoader, dispatcherWrapper, loggerFactory)
        {
            PluginContainer = pluginContainer;
            PreferencesContextFactory = preferencesContextFactory;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;

            PluginItems = new ReadOnlyObservableCollection<PluginSettingEditorElement>(PluginItemsImpl);
        }

        #region property

        PreferencesContextFactory PreferencesContextFactory { get; }
        IUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }

        PluginContainer PluginContainer { get; }

        ObservableCollection<PluginSettingEditorElement> PluginItemsImpl { get; } = new ObservableCollection<PluginSettingEditorElement>();
        public ReadOnlyObservableCollection<PluginSettingEditorElement> PluginItems { get; }

        #endregion


        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            var pluginStates = MainDatabaseBarrier.ReadData(c => {
                var pluginsEntityDao = new PluginsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return pluginsEntityDao.SelectePlguinStateData().ToList();
            });

            // 標準テーマがなければ追加
            if(!pluginStates.Any(i => i.PluginId == DefaultTheme.Informations.PluginIdentifiers.PluginId)) {
                pluginStates.Insert(0, new Data.PluginStateData() {
                    Name = DefaultTheme.Informations.PluginIdentifiers.PluginName,
                    PluginId = DefaultTheme.Informations.PluginIdentifiers.PluginId,
                    State = Data.PluginState.Enable,
                });
            }

            foreach(var pluginState in pluginStates) {
                var plugin = PluginContainer.Plugins.FirstOrDefault(i => pluginState.PluginId == i.PluginInformations.PluginIdentifiers.PluginId);
                var element = new PluginSettingEditorElement(pluginState, plugin, PreferencesContextFactory, MainDatabaseBarrier, DatabaseStatementLoader, UserAgentFactory, PlatformTheme, ImageLoader, DispatcherWrapper, LoggerFactory);
                element.Initialize();
                PluginItemsImpl.Add(element);
            }
        }

        protected override void SaveImpl(IDatabaseCommandsPack commandPack)
        {
            foreach(var element in PluginItems) {
                if(element.SupportedPreferences && element.StartedPreferences) {
                    element.SavePreferences(commandPack);
                }
            }
        }

        #endregion
    }
}
