namespace ContentTypeTextNet.Pe.Core.Models.Database
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

    public static class DatabaseContextsExtensions
    {
        #region function

        /// <summary>
        /// <see cref="IDatabaseAccessor"/>から<see cref="IDatabaseContexts"/>を生成。
        /// </summary>
        /// <param name="databaseAccessor"></param>
        /// <returns></returns>
        public static IDatabaseContexts ToContexts(this IDatabaseAccessor databaseAccessor)
        {
            return new DatabaseContexts(databaseAccessor, databaseAccessor.DatabaseFactory.CreateImplementation());
        }

        /// <summary>
        /// <see cref="IDatabaseTransaction"/>から<see cref="IDatabaseContexts"/>を生成。
        /// </summary>
        /// <param name="databaseTransaction"></param>
        /// <returns></returns>
        public static IDatabaseContexts ToContexts(this IDatabaseTransaction databaseTransaction)
        {
            return new DatabaseContexts(databaseTransaction, databaseTransaction.Implementation);
        }

        #endregion
    }
}
