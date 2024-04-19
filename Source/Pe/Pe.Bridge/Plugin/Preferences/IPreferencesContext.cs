namespace ContentTypeTextNet.Pe.Bridge.Plugin.Preferences
{
    /// <summary>
    /// プラグイン設定(読込)と Pe の架け橋。
    /// </summary>
    /// <remarks>
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPreferencesLoadContext
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
    /// </summary>
    /// <remarks>
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPreferencesCheckContext
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
    /// </summary>
    /// <remarks>
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPreferencesSaveContext
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
    /// </summary>
    /// <remarks>
    /// <para>持ち歩かないこと。</para>
    /// <para>Pe から提供される。</para>
    /// </remarks>
    public interface IPreferencesEndContext
    {
        #region property

        /// <summary>
        /// ストレージ操作。
        /// </summary>
        IPluginStorage Storage { get; }

        /// <summary>
        /// 保存されたか。
        /// </summary>
        /// <remarks>
        /// <para>本体設定でOKしたかキャンセルしたか、てきな。</para>
        /// </remarks>
        bool IsSaved { get; }

        #endregion
    }
}
