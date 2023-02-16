using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base.Models;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// DBMS依存DBブロックコメントとコメント文中で特殊処理する起点・終点。
    /// <para>あえてなんかすることはないはず。</para>
    /// </summary>
    public readonly struct DatabaseBlockComment
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="begin">開始。</param>
        /// <param name="end">終了。</param>
        public DatabaseBlockComment(string begin, string end)
        {
            Begin = begin;
            End = end;
        }

        #region property

        /// <summary>
        /// 開始。
        /// </summary>
        public string Begin { get; }
        /// <summary>
        /// 終了。
        /// </summary>
        public string End { get; }

        #endregion
    }

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
        string NewLine { get; init; }

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
        /// <para>基本的にはバインド処理で対応すること。本処理はしゃあなし動的SQL作成時に無理やり使用する前提。<see cref="DatabaseAccessObjectBase.LoadStatement(string)"/>で動的に組み上げた方が建設的。</para>
        /// </summary>
        /// <param name="word">対象単語。文全体ではなく値を指定する想定。</param>
        /// <returns>変換された値。</returns>
        string Escape(string word);

        /// <summary>
        /// <c>like</c>句のエスケープ処理を実施。
        /// </summary>
        /// <param name="word">対象単語。</param>
        /// <returns>変換された値。</returns>
        string EscapeLike(string word);

        #endregion
    }

    /// <inheritdoc cref="IDatabaseImplementation"/>
    public class DatabaseImplementation: IDatabaseImplementation
    {
        #region IDatabaseImplementation

        /// <inheritdoc cref="IDatabaseImplementation.NewLine"/>
        public string NewLine { get; init; } = Environment.NewLine;

        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDDL"/>
        public virtual bool SupportedTransactionDDL { get; } = false;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDML"/>
        public virtual bool SupportedTransactionDML { get; } = true;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionTruncate"/>
        public virtual bool SupportedTransactionTruncate { get; } = false;

        /// <inheritdoc cref="IDatabaseImplementation.SupportedLineComment"/>
        public virtual bool SupportedLineComment { get; } = true;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedBlockComment"/>
        public virtual bool SupportedBlockComment { get; } = true;

        /// <inheritdoc cref="IDatabaseImplementation.LineComments"/>
        public virtual IEnumerable<string> LineComments => new[] { "--", };
        /// <inheritdoc cref="IDatabaseImplementation.BlockComments"/>
        public virtual IEnumerable<DatabaseBlockComment> BlockComments { get; } = new[] { new DatabaseBlockComment("/*", "*/"), };

        /// <inheritdoc cref="IDatabaseImplementation.ProcessBodyRange"/>
        public virtual DatabaseBlockComment ProcessBodyRange { get; } = new DatabaseBlockComment("{{", "}}");

        /// <inheritdoc cref="IDatabaseImplementation.PreFormatStatement(string)"/>
        public virtual string PreFormatStatement(string statement) => statement;

        /// <inheritdoc cref="IDatabaseImplementation.ToStatementTableName(string)"/>
        public virtual string ToStatementTableName(string tableName) => tableName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementColumnName(string)"/>
        public virtual string ToStatementColumnName(string columnName) => columnName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementParameterName(string, int)"/>
        public virtual string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        /// <inheritdoc cref="IDatabaseImplementation.ToLineComment(string)"/>
        public virtual string ToLineComment(string statement)
        {
            if(!SupportedLineComment) {
                throw new InvalidOperationException(nameof(SupportedLineComment));
            }

            return TextUtility.ReadLines(statement)
                .Select(i => LineComments.First() + i)
                .JoinString(NewLine)
            ;
        }

        /// <inheritdoc cref="IDatabaseImplementation.ToBlockComment(string)"/>
        public virtual string ToBlockComment(string statement)
        {
            if(!SupportedBlockComment) {
                throw new InvalidOperationException(nameof(SupportedBlockComment));
            }
            var blockComment = BlockComments.First();

            var builder = new StringBuilder(NewLine.Length * 4 + blockComment.Begin.Length + blockComment.End.Length + statement.Length);
            builder.AppendLine(NewLine);
            builder.AppendLine(blockComment.Begin);
            builder.AppendLine(NewLine);
            builder.AppendLine(statement);
            builder.AppendLine(NewLine);
            builder.AppendLine(blockComment.End);
            builder.AppendLine(NewLine);

            return builder.ToString();
        }

        /// <inheritdoc cref="IDatabaseImplementation.Escape(string)"/>
        public virtual string Escape(string word) => word
            .Replace("\\", @"\\")
            .Replace("\'", @"''")
            .Replace("\r", @"\r")
            .Replace("\n", @"\n")
        ;

        /// <inheritdoc cref="IDatabaseImplementation.EscapeLike(string)"/>
        public virtual string EscapeLike(string word) => word
            .Replace("\\", @"\\")
            .Replace("%", @"\%")
            .Replace("_", @"\_")
        ;

        #endregion
    }
}
