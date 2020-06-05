using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.FileFinder.Views;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Preferences
{
    public class FileFinderPreferences: PreferencesBase
    {
        #region proeprty
        #endregion

        #region function
        #endregion

        #region PreferencesBase

        public override UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext)
        {
            var control = new FileFinderSettingControl();
            return control;
        }

        public override void CheckPreferences(IPreferencesCheckContext preferencesCheckContext)
        {
        }

        public override void SavePreferences(IPreferencesSaveContext preferencesSaveContext)
        {
        }

        public override void EndPreferences(IPreferencesEndContext preferencesEndContext)
        {
        }

        #endregion
    }
}
