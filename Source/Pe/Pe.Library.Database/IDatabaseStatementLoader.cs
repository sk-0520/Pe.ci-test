using System;
using System.Runtime.CompilerServices;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベースに対する問い合わせ文を取得する。
    /// </summary>
    /// <remarks>
    /// <para>RDB を主軸に実装しているので基本的に SQL となる。</para>
    /// </remarks>
    public interface IDatabaseStatementLoader
    {
        #region function

        /// <summary>
        /// キーからデータベース実行文を取得。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string LoadStatement(string key);
        /// <summary>
        /// 呼び出しクラス・メンバ名の完全名からデータベース実行文を取得する。
        /// </summary>
        /// <returns></returns>
        string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "");

        #endregion
    }
}
