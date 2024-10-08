using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベース実装依存処理。
    /// </summary>
    public interface IDatabaseImplementation
    {
        #region property

        /// <summary>
        /// トランザクション中の<c>DDL</c>が有効か。
        /// </summary>
        bool SupportedTransactionDDL { get; }
        /// <summary>
        /// トランザクション中の<c>DML</c>が有効か。
        /// </summary>
        bool SupportedTransactionDML { get; }
        /// <summary>
        /// トランザクション中の<c>TRUNCATE</c>が有効か。
        /// </summary>
        bool SupportedTransactionTruncate { get; }

        /// <summary>
        /// 単一行コメントをサポートしているか。
        /// </summary>
        bool SupportedLineComment { get; }
        /// <summary>
        /// ブロックコメントをサポートしているか。
        /// </summary>
        bool SupportedBlockComment { get; }

        /// <summary>
        /// 単一行コメントの開始文字列。
        /// </summary>
        IEnumerable<string> LineComments { get; }
        /// <summary>
        /// ブロックコメントの開始終了文字列。
        /// </summary>
        IEnumerable<DatabaseBlockComment> BlockComments { get; }
        /// <summary>
        /// DAOで文の置き換え処理を行う際の範囲開始・終了文字列。
        /// </summary>
        DatabaseBlockComment ProcessBodyRange { get; }
        /// <summary>
        /// 行終端文字列を取得または初期設定。
        /// </summary>
        string NewLine { get; }

        #endregion

        #region function

        /// <summary>
        /// 文実行前に実行する文に対して変換処理を実行。
        /// </summary>
        /// <param name="statement">問い合わせ文</param>
        /// <returns>変換処理後の問い合わせ文。</returns>
        string PreFormatStatement(string statement);
        /// <summary>
        /// テーブル名を文内で使用可能な文字列に変換。
        /// </summary>
        /// <param name="tableName">テーブル名。</param>
        /// <returns>使用可能な文字列。</returns>
        string ToStatementTableName(string tableName);
        /// <summary>
        /// カラム名を文内で使用可能な文字列に変換。
        /// </summary>
        /// <param name="columnName">カラム名。</param>
        /// <returns>使用可能な文字列。</returns>
        string ToStatementColumnName(string columnName);
        /// <summary>
        /// バインド変数名として使用可能な文字列に変換。
        /// </summary>
        /// <param name="parameterName">パラメータ名。</param>
        /// <param name="index">0基点のパラメータ番号。</param>
        /// <returns>使用可能な文字列。</returns>
        string ToStatementParameterName(string parameterName, int index);

        /// <summary>
        /// 文を単一行コメントに変換。
        /// </summary>
        /// <param name="statement">問い合わせ文。</param>
        /// <returns>変換された文。</returns>
        string ToLineComment(string statement);
        /// <summary>
        /// 文をブロックコメントに変換。
        /// </summary>
        /// <param name="statement">問い合わせ文</param>
        /// <returns>変換された文。</returns>
        string ToBlockComment(string statement);
        /// <summary>
        /// 実行文に対するエスケープ処理。
        /// </summary>
        /// <remarks>
        /// <para>基本的にはバインド処理で対応すること。本処理はしゃあなし動的SQL作成時に無理やり使用する前提。<see cref="DatabaseAccessObjectBase.LoadStatement(string)"/>で動的に組み上げた方が建設的。</para>
        /// </remarks>
        /// <param name="word">対象単語。文全体ではなく値を指定する想定。</param>
        /// <returns>変換された値。</returns>
        string Escape(string word);

        /// <summary>
        /// <c>like</c>句のエスケープ処理を実施。
        /// </summary>
        /// <param name="word">対象単語。</param>
        /// <returns>変換された値。</returns>
        string EscapeLike(string word);

        /// <summary>
        /// 実装依存問い合わせ処理の生成。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IDatabaseManagement CreateManagement(IDatabaseContext context);

        #endregion
    }
}
