namespace ContentTypeTextNet.Pe.Library.Database
{
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
