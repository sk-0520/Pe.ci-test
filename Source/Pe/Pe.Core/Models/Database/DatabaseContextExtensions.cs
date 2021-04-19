using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// 読み込み処理の安全のしおり。
    /// </summary>
    public static class DatabaseReaderExtensions
    {
        #region function

        [Conditional("DEBUG")]
        static void EnforceOrderBy(string statement)
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
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectOrdered<T>(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            EnforceOrderBy(statement);

            return reader.Query<T>(statement, parameter, buffered);
        }

        /// <summary>
        /// 検索処理時に順序指定を強制する。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> SelectOrdered(this IDatabaseReader reader, string statement, object? parameter = null, bool buffered = true)
        {
            EnforceOrderBy(statement);

            return reader.Query<dynamic>(statement, parameter, buffered);
        }

        [Conditional("DEBUG")]
        static void EnforceSingleCount(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bselect\s+count\s*\(", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("select count()");
            }
        }

        /// <summary>
        /// 単一の件数取得を強制。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static long SelectSingleCount(this IDatabaseReader reader, string statement, object? parameter = null)
        {
            EnforceSingleCount(statement);

            return reader.QueryFirst<long>(statement, parameter);
        }

        #endregion
    }

    /// <summary>
    /// 書き込み処理の安全のしおり。
    /// </summary>
    public static class DatabaseWriterExtensions
    {
        #region function

        [Conditional("DEBUG")]
        static void EnforceUpdate(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bupdate\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("update");
            }
        }

        /// <summary>
        /// 更新処理を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns>更新件数。</returns>
        public static int Update(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            EnforceUpdate(statement);

            return writer.Execute(statement, parameter);
        }


        /// <summary>
        /// 単一更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
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

        /// <summary>
        /// 単一更新か未更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
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

        [Conditional("DEBUG")]
        static void EnforceInsert(string statement)
        {
            if(!Regex.IsMatch(statement, @"\binsert\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("insert");
            }
        }

        /// <summary>
        /// 挿入を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns>挿入件数。</returns>
        public static int Insert(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            EnforceInsert(statement);

            return writer.Execute(statement, parameter);
        }

        /// <summary>
        /// 単一挿入。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
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

        [Conditional("DEBUG")]
        static void EnforceDelete(string statement)
        {
            if(!Regex.IsMatch(statement, @"\bdelete\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                throw new DatabaseContextException("delete");
            }
        }

        /// <summary>
        /// 削除を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns>削除件数。</returns>
        public static int Delete(this IDatabaseWriter writer, string statement, object? parameter = null)
        {
            EnforceDelete(statement);

            return writer.Execute(statement, parameter);
        }

        /// <summary>
        /// 単一削除。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
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

        /// <summary>
        /// 単一更新か未削除を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
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


        #endregion
    }
}
