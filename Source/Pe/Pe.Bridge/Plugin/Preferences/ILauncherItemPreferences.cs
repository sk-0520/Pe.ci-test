using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    public interface ILauncherItemPreferences
    {
        #region function

        /// <summary>
        /// ランチャーアイテム設定を開始する。
        /// <para>以降、<see cref="EndPreferences"/>が呼び出されるまでずーっと設定中。</para>
        /// </summary>
        /// <returns></returns>
        UserControl BeginPreferences(ILauncherItemPreferencesLoadContext preferencesLoadContext);

        /// <summary>
        /// ランチャーアイテム設定の検証段階に入った時点で呼び出される。
        /// <para>単純な検証処理じゃなくて重めの処理OK。</para>
        /// </summary>
        /// <param name="preferencesCheckContext"></param>
        void CheckPreferences(ILauncherItemPreferencesCheckContext preferencesCheckContext);

        /// <summary>
        /// ランチャーアイテム設定を保存する際に呼び出される。
        /// <para>本体側でキャンセルした際などは呼ばれない。</para>
        /// </summary>
        /// <param name="preferencesSaveContext"></param>
        void SavePreferences(ILauncherItemPreferencesSaveContext preferencesSaveContext);

        /// <summary>
        /// ランチャーアイテム設定の終了。
        /// <para>何がどうあろうと呼び出される。本処理終了後、通常モードとなるので設定の再読み込み等が必要であればこのタイミングで実施する。</para>
        /// <para>なお、プラグインそのものの無効化/有効化は Pe 起動時点で処理される。</para>
        /// </summary>
        void EndPreferences(ILauncherItemPreferencesEndContext preferencesEndContext);

        #endregion
    }
}
