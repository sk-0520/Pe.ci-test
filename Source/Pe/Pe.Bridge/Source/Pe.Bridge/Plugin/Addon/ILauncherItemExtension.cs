using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ランチャーアイテムアイコンの表示方法。
    /// </summary>
    public enum LauncherItemIconMode
    {
        /// <summary>
        /// ツールバー表示。
        /// </summary>
        Toolbar,
        /// <summary>
        /// ツールチップ表示。
        /// </summary>
        Tooltip,
        /// <summary>
        /// コマンド型ランチャー表示。
        /// </summary>
        Command,
        /// <summary>
        /// 設定画面。
        /// </summary>
        Setting,
    }

    /// <summary>
    /// ランチャーアイテム拡張。
    /// </summary>
    public interface ILauncherItemExtension: INotifyPropertyChanged
    {
        #region property

        /// <summary>
        /// ランチャーアイテム専用文言を使用するか。
        /// </summary>
        bool CustomDisplayText { get; }

        /// <summary>
        /// ランチャーアイテム専用文言。
        /// <para><see cref="CustomDisplayText"/>が有効な場合に使用される。</para>
        /// <para>変更を通知するには<see cref="INotifyPropertyChanged.PropertyChanged"/>を使用する。</para>
        /// </summary>
        string DisplayText { get; }

        /// <summary>
        /// ランチャーアイテム専用アイコンを使用するか。
        /// </summary>
        bool CustomLauncherIcon { get; }

        /// <summary>
        /// 独自のランチャーアイテム設定をサポートするか。
        /// </summary>
        bool SupportedPreferences { get; }

        #endregion

        #region function

        /// <summary>
        /// アイコン取得。
        /// <para>(TODO: 考え中)<see cref="CustomLauncherIcon"/>が有効な場合に使用される。</para>
        /// <para>UIスレッド上で実行を保証。</para>
        /// </summary>
        /// <param name="iconMode"></param>
        /// <param name="iconScale"></param>
        /// <param name="launcherItemAddonContext"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(LauncherItemIconMode iconMode,  in IconScale iconScale);

        /// <summary>
        /// アイテムの実行。
        /// <para>ウィンドウを表示する場合はこの処理で行うこと(ユーザー操作を起点にする必要がある)。</para>
        /// </summary>
        /// <param name="commandExecuteParameter">実行パラメータ。</param>
        /// <param name="launcherItemExtensionExecuteParameter">ランチャーアイテムパラメータ。</param>
        /// <param name="launcherItemAddonContext"></param>
        void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);

        /// <summary>
        /// 設定処理。
        /// <para><see cref="SupportedPreferences"/>が有効な場合に使用される。</para>
        /// </summary>
        /// <returns></returns>
        ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
