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
        public ClockLauncherItemPreferences(PluginBase plugin, Guid launcherItemId, IAddonExecutor addonExecutor, IDispatcherWrapper dispatcherWrapper, ISkeletonImplements skeletonImplements, IImageLoader imageLoader, IHttpUserAgentFactory httpUserAgentFactory, ILoggerFactory loggerFactory)
            : base(plugin, launcherItemId, addonExecutor, dispatcherWrapper, skeletonImplements, imageLoader, httpUserAgentFactory, loggerFactory)
        { }

        #region property

        ClockLauncherItemSetting? Setting { get; set; }
        ClockLauncherItemPreferencesViewModel? ViewModel { get; set; }
        UserControl? View { get; set; }

        #endregion

        #region LauncherItemPreferencesBase

        public override UserControl BeginPreferences(ILauncherItemPreferencesLoadContext preferencesLoadContext)
        {
            //TODO: 二重に呼ばれてる・・・
            if(Setting == null) {
                if(!preferencesLoadContext.Storage.Persistent.Normal.TryGet<ClockLauncherItemSetting>(LauncherItemId, string.Empty, out var value)) {
                    value = new ClockLauncherItemSetting();
                }

                Setting = value;
            }

            ViewModel = new ClockLauncherItemPreferencesViewModel(Setting, SkeletonImplements, DispatcherWrapper, LoggerFactory);

            View = new ClockLauncherItemPreferencesControl() {
                DataContext = ViewModel,
            };

            return View;
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
            preferencesSaveContext.Storage.Persistent.Normal.Set(LauncherItemId, string.Empty, Setting);
        }

        public override void EndPreferences(ILauncherItemPreferencesEndContext preferencesEndContext)
        {
            Debug.Assert(Setting != null);
            Debug.Assert(ViewModel != null);
            Debug.Assert(View != null);

            View.DataContext = null;
            View = null;

            ViewModel.Dispose();
            ViewModel = null;

            Setting = null;
        }

        #endregion
    }
}
