using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// DBMS の依存は極力考えない処理。

namespace ContentTypeTextNet.Pe.Core.Models.Database
{

    [Serializable]
    public class DatabaseContextException: Exception
    {
        public DatabaseContextException() { }
        public DatabaseContextException(string message) : base(message) { }
        public DatabaseContextException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseContextException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

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
            if(!Regex.IsMatch(statement, @"\bselect\s+count\s*\(\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
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
        /// 単一更新を強制。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <exception cref="DatabaseContextException">未更新。</exception>
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
        /// 単一挿入。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <exception cref="DatabaseContextException">未挿入か複数挿入。</exception>
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
        /// 単一削除。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <exception cref="DatabaseContextException">未削除。</exception>
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
