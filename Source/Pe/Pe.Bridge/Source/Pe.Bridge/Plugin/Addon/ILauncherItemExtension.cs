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
        /// </summary>
        /// <param name="launcherItemId"></param>
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
        /// </summary>
        /// <param name="executeParameter">実行パラメータ。</param>
        /// <param name="launcherItemExtensionParameter">ランチャーアイテムパラメータ。</param>
        void Execute(ICommandExecuteParameter executeParameter, IAddonParameter addonParameter, ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
