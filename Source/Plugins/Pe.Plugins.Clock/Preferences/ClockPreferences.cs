using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Clock.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Clock.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Clock.Views;

namespace ContentTypeTextNet.Pe.Plugins.Clock.Preferences
{
    public class ClockPreferences: PreferencesBase
    {
        public ClockPreferences(IPlugin plugin)
            : base(plugin)
        { }

        #region proeprty

        ClockSettingViewModel? SettingViewModel { get; set; }
        #endregion

        #region function
        #endregion

        #region PreferencesBase

        public override UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter)
        {
            ClockWidgetSetting? clockWidgetSetting;
            if(!preferencesLoadContext.Storage.Persistent.Normal.TryGet<ClockWidgetSetting>(ClockConstants.WidgetSettengKey, out clockWidgetSetting)) {
                clockWidgetSetting = new ClockWidgetSetting();
            }

            SettingViewModel = new ClockSettingViewModel(clockWidgetSetting, preferencesParameter.SkeletonImplements, preferencesParameter.DispatcherWrapper, preferencesParameter.LoggerFactory);

            var control = new ClockSettingControl() {
                DataContext = SettingViewModel,
            };

            return control;
        }

        public override void CheckPreferences(IPreferencesCheckContext preferencesCheckContext)
        {
        }

        public override void SavePreferences(IPreferencesSaveContext preferencesSaveContext)
        {
            Debug.Assert(SettingViewModel != null);

            preferencesSaveContext.Storage.Persistent.Normal.Set(ClockConstants.WidgetSettengKey, SettingViewModel.WidgetSetting);
        }

        public override void EndPreferences(IPreferencesEndContext preferencesEndContext)
        {
            SettingViewModel = null;
        }

        #endregion
    }
}
