using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// プラグイン設定(読込)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPreferencesLoadContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }


        #endregion
    }

    /// <summary>
    /// プラグイン設定(チェック)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPreferencesCheckContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        /// <summary>
        /// チェック結果。
        /// </summary>
        bool HasError { get; set; }

        #endregion
    }

    /// <summary>
    /// プラグイン設定(保存)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPreferencesSaveContext: IPluginCommonContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        #endregion
    }

    /// <summary>
    /// プラグイン設定(終了)と Pe の架け橋。
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IPreferencesEndContext: IPluginCommonContext
    {
        #region function

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        /// <summary>
        /// 保存されたか。
        /// <para>本体設定でOKしたかキャンセルしたか、てきな。</para>
        /// </summary>
        bool IsSaved { get; }

        #endregion
    }
}
