namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベース会話・データベース実装処理のペア。
    /// </summary>
    public interface IDatabaseContexts
    {
        #region property

        /// <summary>
        /// データベース会話処理。
        /// </summary>
        /// <remarks>
        /// <para>トランザクション状態は上位で管理。</para>
        /// </remarks>
        IDatabaseContext Context { get; }
        /// <summary>
        /// データベース実装依存。
        /// </summary>
        IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
