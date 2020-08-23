using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences
{
    internal class ClockLauncherItemPreferences: LauncherItemPreferencesBase
    {
        public ClockLauncherItemPreferences(PluginBase plugin, IAddonExecutor addonExecutor, IDispatcherWrapper dispatcherWrapper, ISkeletonImplements skeletonImplements, IImageLoader imageLoader, IHttpUserAgentFactory httpUserAgentFactory, ILoggerFactory loggerFactory)
            : base(plugin, addonExecutor, dispatcherWrapper, skeletonImplements, imageLoader, httpUserAgentFactory, loggerFactory)
        { }

        #region property

        ClockLauncherItemSetting? Setting { get; set; }

        #endregion

        #region LauncherItemPreferencesBase

        public override UserControl BeginPreferences(ILauncherItemPreferencesLoadContext preferencesLoadContext)
        {
            //preferencesLoadContext.Storage.Persistent.Normal

            // 暫定
            Setting = new ClockLauncherItemSetting();

            var view = new ClockLauncherItemPreferencesControl() {
                DataContext = new ClockLauncherItemPreferencesViewModel(Setting, SkeletonImplements, DispatcherWrapper, LoggerFactory),
            };

            return view;
        }

        public override void CheckPreferences(ILauncherItemPreferencesCheckContext preferencesCheckContext)
        {
            Debug.Assert(Setting != null);

            try {
                DateTime.Now.ToString(Setting.Format);
            } catch {
                preferencesCheckContext.HasError = true;
            }
        }

        public override void SavePreferences(ILauncherItemPreferencesSaveContext preferencesSaveContext)
        {
            Debug.Assert(Setting != null);
            throw new NotImplementedException();
        }

        public override void EndPreferences(ILauncherItemPreferencesEndContext preferencesEndContext)
        {
            Debug.Assert(Setting != null);
            throw new NotImplementedException();
        }

        #endregion
    }
}
