using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// 書き込み処理の安全のしおり。
    /// </summary>
    /// <remarks>
    /// <para>問い合わせ文として非ユーザー入力でデバッグ中に検証可能なものを想定している。</para>
    /// <para>そのため問い合わせ文の確認自体もデバッグ時のみ有効となる。</para>
    /// <para>なお確認自体はただの文字列比較であるため該当ワードをコメントアウトしても通過する点に注意。</para>
    /// </remarks>
    public static class IDatabaseWriterExtensions
    {
        #region function

        [Conditional("DEBUG")]
        private static void ThrowIfNotUpdate(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bupdate\b", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("update");
            }
        }

        /// <summary>
        /// 更新処理を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>更新件数。</returns>
        public static int Update(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Update(IDatabaseWriter, string, object?)"/>
        public static Task<int> UpdateAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未更新。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void UpdateByKey(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"update -> {result}");
            }
        }

        /// <inheritdoc cref="UpdateByKey(IDatabaseWriter, string, object?)"/>
        public static async Task UpdateByKeyAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"update -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">複数更新。</exception>
        /// <returns>真: 単一更新、偽: 未更新。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool UpdateByKeyOrNothing(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotUpdate(statement);

            var result = writer.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseManipulationException($"update -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="UpdateByKeyOrNothing(IDatabaseWriter, string, object)"/>
        public static async Task<bool> UpdateByKeyOrNothingAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotUpdate(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseManipulationException($"update -> {result}");
            }

            return result == 1;
        }

        [Conditional("DEBUG")]
        private static void ThrowIfNotInsert(string statement)
        {
            if(!Regex.IsMatch(statement, @"\binsert\b", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("insert");
            }
        }

        /// <summary>
        /// 挿入を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>挿入件数。</returns>
        public static int Insert(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotInsert(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Insert(IDatabaseWriter, string, object?)"/>
        public static Task<int> InsertAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotInsert(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一挿入。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未挿入か複数挿入。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void InsertSingle(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotInsert(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"insert -> {result}");
            }
        }

        /// <inheritdoc cref="InsertSingle(IDatabaseWriter, string, object?)"/>
        public static async Task InsertSingleAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotInsert(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"insert -> {result}");
            }
        }

        [Conditional("DEBUG")]
        private static void ThrowIfNotDelete(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bdelete\b", RegexOptions.IgnoreCase | RegexOptions.Multiline, Timeout.InfiniteTimeSpan)) {
                throw new DatabaseStatementException("delete");
            }
        }

        /// <summary>
        /// 削除を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns>削除件数。</returns>
        public static int Delete(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Delete(IDatabaseWriter, string, object?)"/>
        public static Task<int> DeleteAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一削除。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseStatementException">未削除。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void DeleteByKey(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }
        }
        /// <inheritdoc cref="DeleteByKey(IDatabaseWriter, string, object?)"/>
        public static async Task DeleteByKeyAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未削除を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        /// <exception cref="DatabaseStatementException">複数削除。</exception>
        /// <returns>真: 単一削除、偽: 未削除。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool DeleteByKeyOrNothing(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            ThrowIfNotDelete(statement);

            var result = writer.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="DeleteByKeyOrNothing(IDatabaseWriter, string, object?)"/>
        public static async Task<bool> DeleteByKeyOrNothingAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfNotDelete(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseManipulationException($"delete -> {result}");
            }

            return result == 1;
        }

        #endregion
    }
}
