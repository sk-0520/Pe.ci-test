using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// ランチャーアイテム設定(読込)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemPreferencesLoadContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        ILauncherItemAddonStorage Storage { get; }


        #endregion
    }

    /// <summary>
    /// ランチャーアイテム設定(チェック)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemPreferencesCheckContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        ILauncherItemAddonStorage Storage { get; }

        /// <summary>
        /// チェック結果。
        /// </summary>
        bool HasError { get; set; }

        #endregion
    }

    /// <summary>
    /// ランチャーアイテム設定(保存)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemPreferencesSaveContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        ILauncherItemAddonStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// ランチャーアイテム設定(終了)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface ILauncherItemPreferencesEndContext
    {
        #region function

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        ILauncherItemAddonStorage Storage { get; }

        /// <summary>
        /// 保存されたか。
        /// <para>本体設定でOKしたかキャンセルしたか、てきな。</para>
        /// </summary>
        bool IsSaved { get; }

        #endregion
    }
}
