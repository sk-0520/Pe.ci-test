using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPLuginId
    {
        public PluginSettingEditorElement(IPlugin plugin, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Plugin = plugin;
            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseLazyWriterPack = databaseLazyWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;

            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }
        }

        #region property

        public IPlugin Plugin { get; }

        IDatabaseBarrierPack DatabaseBarrierPack { get; }
        IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IUserAgentManager UserAgentManager { get; }

        public bool SupportedPreferences { get; }
        IPreferences? Preferences { get; }

        public bool StartedPreferences { get; private set; }

        #endregion

        #region function

        PreferencesContextFactory CreateContextFactory()
        {
            var factory = new PreferencesContextFactory(DatabaseLazyWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager, LoggerFactory);
            return factory;
        }

        public UserControl BeginPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(!StartedPreferences);

            UserControl result;
            var factory = CreateContextFactory();
            using(var reader = DatabaseBarrierPack.WaitRead()) {
                var context = factory.CreateLoadContext(Plugin.PluginInformations.PluginIdentifiers, reader);
                result = Preferences.BeginPreferences(context);
            }
            StartedPreferences = true;
            return result;
        }

        public bool CheckPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);

            PreferencesCheckContext context;
            var factory = CreateContextFactory();
            using(var reader = DatabaseBarrierPack.WaitRead()) {
                context = factory.CreateCheckContext(Plugin.PluginInformations.PluginIdentifiers, reader);
                Preferences.CheckPreferences(context);
            }
            return context.HasError;
        }

        public void SavePreferences(IDatabaseCommandsPack databaseCommandPack)
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);

            var factory = CreateContextFactory();
            var context = factory.CreateSaveContext(Plugin.PluginInformations.PluginIdentifiers, databaseCommandPack);
            Preferences.SavePreferences(context);
        }

        public void EndPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);

            // NOTE: 多分ここじゃなくて別んところで呼び出すべき

            var factory = CreateContextFactory();
            using(var reader = DatabaseBarrierPack.WaitRead()) {
                var context = factory.CreateEndContext(Plugin.PluginInformations.PluginIdentifiers, reader);
                Preferences.EndPreferences(context);
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion

        #region IPLuginId

        public Guid PluginId => Plugin.PluginInformations.PluginIdentifiers.PluginId;

        #endregion
    }
}
