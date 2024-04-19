using System.ComponentModel;
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
        /// </summary>
        /// <remarks>
        /// <para><see cref="CustomDisplayText"/>が有効な場合に使用される。</para>
        /// <para>変更を通知するには<see cref="INotifyPropertyChanged.PropertyChanged"/>を使用する。</para>
        /// </remarks>
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
        /// ランチャーアイテムとして表示(or 非表示)された的な。
        /// </summary>
        void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject);

        /// <summary>
        /// アイコン取得。
        /// </summary>
        /// <remarks>
        /// <para>(TODO: 考え中)<see cref="CustomLauncherIcon"/>が有効な場合に使用される。</para>
        /// <para>UIスレッド上で実行を保証。</para>
        /// </remarks>
        /// <param name="iconMode"></param>
        /// <param name="iconScale"></param>
        /// <param name="launcherItemAddonContext"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale);

        /// <summary>
        /// アイテムの実行。
        /// </summary>
        /// <remarks>
        /// <para>ウィンドウを表示する場合はこの処理で行うこと(ユーザー操作を起点にする必要がある)。</para>
        /// </remarks>
        /// <param name="argument">外部から渡された引数。null の場合は渡されていない。使用有無はアドオン側に任される。</param>
        /// <param name="commandExecuteParameter">実行パラメータ。</param>
        /// <param name="launcherItemExtensionExecuteParameter">ランチャーアイテムパラメータ。</param>
        /// <param name="launcherItemAddonContext"></param>
        void Execute(string? argument, ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);

        /// <summary>
        /// 設定処理。
        /// </summary>
        /// <remarks>
        /// <para><see cref="SupportedPreferences"/>が有効な場合に使用される。</para>
        /// <para>混乱中: データを読み込むには<see cref="ILauncherItemPreferences.BeginPreferences(ILauncherItemPreferencesLoadContext)"/>で行うこと。</para>
        /// </remarks>
        /// <returns></returns>
        ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
