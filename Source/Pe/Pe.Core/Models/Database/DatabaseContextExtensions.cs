using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// 読み込み処理の安全のしおり。
    /// </summary>
    public static class IDatabaseReaderExtensions
    {
        #region function

        [Conditional("DEBUG")]
        private static void EnforceOrderBy(string statement)
        {
            if(!Regex.IsMatch(statement, @"\border\s+by\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("order by");
            }
        }

        /// <summary>
        /// 検索処理時に順序指定を強制する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static IEnumerable<T> SelectOrdered<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            EnforceOrderBy(statement);

            return reader.Query<T>(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static IEnumerable<dynamic> SelectOrdered(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            EnforceOrderBy(statement);

            return reader.Query(statement, parameter, buffered);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<T>> SelectOrderedAsync<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            EnforceOrderBy(statement);

            return reader.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        /// <inheritdoc cref="SelectOrdered{T}(IDatabaseReader, string, object?, bool)"/>
        public static Task<IEnumerable<dynamic>> SelectOrderedAsync(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            EnforceOrderBy(statement);

            return reader.QueryAsync(statement, parameter, buffered, cancellationToken);
        }


        [Conditional("DEBUG")]
        private static void EnforceSingleCount(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bselect\s+count\s*\(", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("select count()");
            }
        }

        /// <summary>
        /// 単一の件数取得を強制。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        public static long SelectSingleCount(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            EnforceSingleCount(statement);

            return reader.QueryFirst<long>(statement, parameter);
        }

        /// <inheritdoc cref="SelectSingleCount(IDatabaseReader, string, object?)"/>
        public static Task<long> SelectSingleCountAsync(this IDatabaseReader reader, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            EnforceSingleCount(statement);

            return reader.QueryFirstAsync<long>(statement, parameter, cancellationToken);
        }

        #endregion
    }

    /// <summary>
    /// 書き込み処理の安全のしおり。
    /// </summary>
    public static class IDatabaseWriterExtensions
    {
        #region function

        [Conditional("DEBUG")]
        private static void EnforceUpdate(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bupdate\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("update");
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
            EnforceUpdate(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Update(IDatabaseWriter, string, object?)"/>
        public static Task<int> UpdateAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            EnforceUpdate(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseContextException">未更新。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void UpdateByKey(this IDatabaseWriter writer, string statement, object parameter)
        {
            EnforceUpdate(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseContextException($"update -> {result}");
            }
        }

        /// <inheritdoc cref="UpdateByKey(IDatabaseWriter, string, object)"/>
        public static async Task UpdateByKeyAsync(this IDatabaseWriter writer, string statement, object parameter, CancellationToken cancellationToken = default)
        {
            EnforceUpdate(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseContextException($"update -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseContextException">複数更新。</exception>
        /// <returns>真: 単一更新、偽: 未更新。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool UpdateByKeyOrNothing(this IDatabaseWriter writer, string statement, object parameter)
        {
            EnforceUpdate(statement);

            var result = writer.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseContextException($"update -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="UpdateByKeyOrNothing(IDatabaseWriter, string, object)"/>
        public static async Task<bool> UpdateByKeyOrNothingAsync(this IDatabaseWriter writer, string statement, object parameter, CancellationToken cancellationToken = default)
        {
            EnforceUpdate(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseContextException($"update -> {result}");
            }

            return result == 1;
        }

        [Conditional("DEBUG")]
        private static void EnforceInsert(string statement)
        {
            if(!Regex.IsMatch(statement, @"\binsert\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("insert");
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
            EnforceInsert(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Insert(IDatabaseWriter, string, object?)"/>
        public static Task<int> InsertAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            EnforceInsert(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一挿入。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseContextException">未挿入か複数挿入。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void InsertSingle(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            EnforceInsert(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseContextException($"insert -> {result}");
            }
        }

        /// <inheritdoc cref="InsertSingle(IDatabaseWriter, string, object?)"/>
        public static async Task InsertSingleAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            EnforceInsert(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseContextException($"insert -> {result}");
            }
        }

        [Conditional("DEBUG")]
        private static void EnforceDelete(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bdelete\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("delete");
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
            EnforceDelete(statement);

            return writer.Execute(statement, parameter);
        }

        /// <inheritdoc cref="Delete(IDatabaseWriter, string, object?)"/>
        public static Task<int> DeleteAsync(this IDatabaseWriter writer, string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            EnforceDelete(statement);

            return writer.ExecuteAsync(statement, parameter, cancellationToken);
        }

        /// <summary>
        /// 単一削除。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <exception cref="DatabaseContextException">未削除。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static void DeleteByKey(this IDatabaseWriter writer, string statement, object parameter)
        {
            EnforceDelete(statement);

            var result = writer.Execute(statement, parameter);
            if(result != 1) {
                throw new DatabaseContextException($"delete -> {result}");
            }
        }
        /// <inheritdoc cref="DeleteByKey(IDatabaseWriter, string, object)"/>
        public static async Task DeleteByKeyAsync(this IDatabaseWriter writer, string statement, object parameter, CancellationToken cancellationToken = default)
        {
            EnforceDelete(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(result != 1) {
                throw new DatabaseContextException($"delete -> {result}");
            }
        }

        /// <summary>
        /// 単一更新か未削除を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement">データベース問い合わせ文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <returns></returns>
        /// <exception cref="DatabaseContextException">複数削除。</exception>
        /// <returns>真: 単一削除、偽: 未削除。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public static bool DeleteByKeyOrNothing(this IDatabaseWriter writer, string statement, object parameter)
        {
            EnforceDelete(statement);

            var result = writer.Execute(statement, parameter);
            if(1 < result) {
                throw new DatabaseContextException($"delete -> {result}");
            }

            return result == 1;
        }

        /// <inheritdoc cref="DeleteByKeyOrNothing(IDatabaseWriter, string, object)"/>
        public static async Task<bool> DeleteByKeyOrNothingAsync(this IDatabaseWriter writer, string statement, object parameter, CancellationToken cancellationToken = default)
        {
            EnforceDelete(statement);

            var result = await writer.ExecuteAsync(statement, parameter, cancellationToken);
            if(1 < result) {
                throw new DatabaseContextException($"delete -> {result}");
            }

            return result == 1;
        }

        #endregion
    }
}
