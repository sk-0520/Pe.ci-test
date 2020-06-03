using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    public class PreferencesBase: IPreferences
    {
        #region IPreferences

        /// <inheritdoc cref="IPreferences.BeginPreferences(IPreferencesLoadContext)"/>
        public virtual UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.CheckPreferences(IPreferencesCheckContext)"/>
        public virtual void CheckPreferences(IPreferencesCheckContext preferencesCheckContext) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.SavePreferences(IPreferencesSaveContext)"/>
        public virtual void SavePreferences(IPreferencesSaveContext preferencesSaveContext) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.EndPreferences(IPreferencesEndContext)"/>
        public virtual void EndPreferences(IPreferencesEndContext preferencesEndContext) => throw new NotImplementedException();

        #endregion
    }
}
