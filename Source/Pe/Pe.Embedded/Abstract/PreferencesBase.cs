using System;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    /// <summary>
    /// 設定基底処理。
    /// </summary>
    public class PreferencesBase: IPreferences
    {
        protected PreferencesBase(IPlugin plugin)
        {
            Plugin = plugin;
        }

        #region property

        IPlugin Plugin { get; }

        #endregion

        #region IPreferences

        /// <inheritdoc cref="IPreferences.BeginPreferences(IPreferencesLoadContext, IPreferencesParameter)"/>
        public virtual UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.CheckPreferences(IPreferencesCheckContext)"/>
        public virtual void CheckPreferences(IPreferencesCheckContext preferencesCheckContext) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.SavePreferences(IPreferencesSaveContext)"/>
        public virtual void SavePreferences(IPreferencesSaveContext preferencesSaveContext) => throw new NotImplementedException();

        /// <inheritdoc cref="IPreferences.EndPreferences(IPreferencesEndContext)"/>
        public virtual void EndPreferences(IPreferencesEndContext preferencesEndContext) => throw new NotImplementedException();

        #endregion
    }
}
