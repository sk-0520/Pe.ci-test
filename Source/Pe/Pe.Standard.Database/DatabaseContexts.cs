namespace ContentTypeTextNet.Pe.Standard.Database
{
    /// <summary>
    /// データベース会話・データベース実装処理のペア。
    /// </summary>
    public interface IDatabaseContexts
    {
        #region property

        /// <summary>
        /// データベース会話処理。
        /// <para>トランザクション状態は上位で管理。</para>
        /// </summary>
        IDatabaseContext Context { get; }
        /// <summary>
        /// データベース実装依存。
        /// </summary>
        IDatabaseImplementation Implementation { get; }

        #endregion
    }

    /// <inheritdoc cref="IDatabaseContexts"/>
    public class DatabaseContexts: IDatabaseContexts
    {
        public DatabaseContexts(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            Context = context;
            Implementation = implementation;
        }

        #region IDatabaseContext

        /// <inheritdoc cref="IDatabaseContexts.Context"/>
        public IDatabaseContext Context { get; }

        /// <inheritdoc cref="IDatabaseContexts.Implementation"/>
        public IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
