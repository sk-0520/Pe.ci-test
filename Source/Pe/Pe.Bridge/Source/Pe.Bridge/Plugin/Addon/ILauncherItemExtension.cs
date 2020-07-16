using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// ランチャーアイテム拡張。
    /// </summary>
    public interface ILauncherItemExtension
    {
        #region property

        /// <summary>
        /// ランチャーアイテム専用文言を使用するか。
        /// </summary>
        bool CustomDisplayText { get; }

        /// <summary>
        /// ランチャーアイテム専用文言。
        /// </summary>
        string DisplayText { get; }

        #endregion

        #region function

        /// <summary>
        /// 初期化処理。
        /// <para>ランチャーアイテムが使用される際に呼び出される。</para>
        /// </summary>
        /// <param name="launcherItemId">ランチャーアイテムID。</param>
        /// <param name="launcherItemAddonContext"></param>
        void Initialize(ILauncherItemId launcherItemId, ILauncherItemAddonContext launcherItemAddonContext);

        /// <summary>
        /// アイコン取得。
        /// <para>UIスレッド上で実行を保証。</para>
        /// </summary>
        /// <param name="iconBox"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(IconScale iconScale, ILauncherItemAddonContext launcherItemAddonContext);

        /// <summary>
        /// アイテムの実行。
        /// <para>ウィンドウを表示する場合はこの処理で行うこと(ユーザー操作を起点にする必要がある)。</para>
        /// </summary>
        /// <param name="commandExecuteParameter">実行パラメータ。</param>
        /// <param name="launcherItemExtensionExecuteParameter">ランチャーアイテムパラメータ。</param>
        /// <param name="launcherItemAddonContext"></param>
        void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
