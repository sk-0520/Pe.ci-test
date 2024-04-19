using System.Windows.Controls;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// 本体設定画面におけるプラグイン設定機能。
    /// </summary>
    /// <remarks>
    /// <para>持ち歩かないこと。</para>
    /// </remarks>
    public interface IPreferences
    {
        #region function

        /// <summary>
        /// プラグイン設定を開始する。
        /// </summary>
        /// <remarks>
        /// <para>以降、<see cref="EndPreferences"/>が呼び出されるまでずーっと設定中。</para>
        /// </remarks>
        /// <param name="preferencesLoadContext"></param>
        /// <returns></returns>
        UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter);

        /// <summary>
        /// プラグイン設定の検証段階に入った時点で呼び出される。
        /// </summary>
        /// <param name="preferencesCheckContext"></param>
        void CheckPreferences(IPreferencesCheckContext preferencesCheckContext);

        /// <summary>
        /// プラグイン設定を保存する際に呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>本体側でキャンセルした際などは呼ばれない。</para>
        /// </remarks>
        /// <param name="preferencesSaveContext"></param>
        void SavePreferences(IPreferencesSaveContext preferencesSaveContext);

        /// <summary>
        /// プラグイン設定の終了。
        /// </summary>
        /// <remarks>
        /// <para>何がどうあろうと呼び出される。本処理終了後、通常モードとなるので設定の再読み込み等が必要であればこのタイミングで実施する。</para>
        /// <para>なお、プラグインそのものの無効化/有効化は Pe 起動時点で処理される。</para>
        /// </remarks>
        void EndPreferences(IPreferencesEndContext preferencesEndContext);

        #endregion
    }
}
