using System;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// 読み込み専用トランザクション。
    /// </summary>
    public sealed class ReadOnlyDatabaseTransaction: DatabaseTransaction
    {
        /// <inheritdoc cref="DatabaseTransaction.DatabaseTransaction(bool, IDatabaseAccessor)" />
        public ReadOnlyDatabaseTransaction(bool beginTransaction, IDatabaseAccessor databaseAccessor)
            : base(beginTransaction, databaseAccessor)
        { }

        /// <inheritdoc cref="DatabaseTransaction.DatabaseTransaction(bool, IDatabaseAccessor, IsolationLevel)" />
        public ReadOnlyDatabaseTransaction(bool beginTransaction, IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
            : base(beginTransaction, databaseAccessor, isolationLevel)
        { }

        #region DatabaseTransaction

        public override void Commit() => throw new NotSupportedException();

        public override int Execute(string statement, object? parameter = null) => throw new NotSupportedException();

        public override Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default) => throw new NotSupportedException();

        #endregion
    }
}
