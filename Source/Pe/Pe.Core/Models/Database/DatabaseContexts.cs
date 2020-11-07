using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベース会話・データベース実装処理のペア。
    /// </summary>
    public interface IDatabaseContexts
    {
        #region property

        IDatabaseContext Context { get; }
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
