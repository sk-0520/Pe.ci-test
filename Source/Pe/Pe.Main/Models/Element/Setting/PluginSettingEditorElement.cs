using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPLuginId
    {
        public PluginSettingEditorElement(IPlugin plugin, PreferencesContextFactory preferencesContextFactory, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Plugin = plugin;
            PreferencesContextFactory = preferencesContextFactory;
            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;

            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }
        }

        #region property

        public IPlugin Plugin { get; }

        PreferencesContextFactory PreferencesContextFactory { get; }
        IUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public bool SupportedPreferences { get; }
        IPreferences? Preferences { get; }

        public bool StartedPreferences { get; private set; }

        #endregion

        #region function

        public UserControl BeginPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(!StartedPreferences);

            UserControl result;
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                var context = PreferencesContextFactory.CreateLoadContext(Plugin.PluginInformations, reader);
                var skeleton = new SkeletonImplements();
                var parameter = new PreferencesParameter(skeleton, UserAgentFactory, PlatformTheme, DispatcherWrapper, LoggerFactory);
                result = Preferences.BeginPreferences(context, parameter);
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
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                context = PreferencesContextFactory.CreateCheckContext(Plugin.PluginInformations, reader);
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

            var context = PreferencesContextFactory.CreateSaveContext(Plugin.PluginInformations, databaseCommandPack);
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

            using(var reader = PreferencesContextFactory.BarrierRead()) {
                var context = PreferencesContextFactory.CreateEndContext(Plugin.PluginInformations, reader);
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
