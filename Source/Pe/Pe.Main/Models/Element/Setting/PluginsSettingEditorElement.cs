using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginsSettingEditorElement: SettingEditorElementBase
    {
        public PluginsSettingEditorElement(PluginContainer pluginContainer, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
            PluginContainer = pluginContainer;
            DatabaseBarrierPack = databaseBarrierPack;
                DatabaseLazyWriterPack = databaseLazyWriterPack;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
            PluginItems = new ReadOnlyObservableCollection<PluginSettingEditorElement>(PluginItemsImpl);
        }

        #region property

        IDatabaseBarrierPack DatabaseBarrierPack { get; }
        IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
        EnvironmentParameters EnvironmentParameters{get;}
        IUserAgentManager UserAgentManager { get; }

        PluginContainer PluginContainer { get; }

        ObservableCollection<PluginSettingEditorElement> PluginItemsImpl { get; } = new ObservableCollection<PluginSettingEditorElement>();
        public ReadOnlyObservableCollection<PluginSettingEditorElement> PluginItems { get; }

        #endregion


        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            //var p = new PluginLoadContext
            //throw new NotImplementedException();
            foreach(var plugin in PluginContainer.Plugins) {
                var element = new PluginSettingEditorElement(plugin, DatabaseBarrierPack, DatabaseLazyWriterPack, EnvironmentParameters, UserAgentManager, LoggerFactory);
                element.Initialize(); // 無意味だけど呼び出し
                PluginItemsImpl.Add(element);
            }
        }

        protected override void SaveImpl(ApplicationDatabaseCommandsPack commandPack)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
