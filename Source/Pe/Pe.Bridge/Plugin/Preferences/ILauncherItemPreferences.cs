using System.Windows.Controls;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// ランチャーアイテム設定。
    /// <para>持ち歩かないこと。</para>
    /// </summary>
    public interface ILauncherItemPreferences
    {
        #region function

        /// <summary>
        /// ランチャーアイテム設定を開始する。
        /// </summary>
        /// <remarks>
        /// <para>以降、<see cref="EndPreferences"/>が呼び出されるまでずーっと設定中。</para>
        /// </remarks>
        /// <returns></returns>
        UserControl BeginPreferences(ILauncherItemPreferencesLoadContext preferencesLoadContext);

        /// <summary>
        /// ランチャーアイテム設定の検証段階に入った時点で呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>単純な検証処理じゃなくて重めの処理OK。</para>
        /// </remarks>
        /// <param name="preferencesCheckContext"></param>
        void CheckPreferences(ILauncherItemPreferencesCheckContext preferencesCheckContext);

        /// <summary>
        /// ランチャーアイテム設定を保存する際に呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>本体側でキャンセルした際などは呼ばれない。</para>
        /// </remarks>
        /// <param name="preferencesSaveContext"></param>
        void SavePreferences(ILauncherItemPreferencesSaveContext preferencesSaveContext);

        /// <summary>
        /// ランチャーアイテム設定の終了。
        /// </summary>
        /// <remarks>
        /// <para>何がどうあろうと呼び出される。本処理終了後、通常モードとなるので設定の再読み込み等が必要であればこのタイミングで実施する。</para>
        /// <para>なお、プラグインそのものの無効化/有効化は Pe 起動時点で処理される。</para>
        /// </remarks>
        void EndPreferences(ILauncherItemPreferencesEndContext preferencesEndContext);

        #endregion
    }
}
