using System;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベース実装依存処理。
    /// </summary>
    public interface IDatabaseImplementation
    {
        #region property

        /// <summary>
        /// DDLのトランザクションが有効か。
        /// </summary>
        bool SupportedTransactionDDL { get; }
        /// <summary>
        /// DMLのトランザクションが有効か。
        /// </summary>
        bool SupportedTransactionDML { get; }

        /// <summary>
        /// 単一行コメントをサポートしているか。
        /// </summary>
        bool SupportedSingleLineComment { get; }
        /// <summary>
        /// 複数行コメントをサポートしているか。
        /// </summary>
        bool SupportedMultiLineComment { get; }

        #endregion

        #region function

        /// <summary>
        /// 文実行前に実行する文に対して変換処理を実行。
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string PreFormatStatement(string statement);
        /// <summary>
        /// テーブル名を文内で使用可能な文字列に変換。
        /// </summary>
        /// <param name="tableName">テーブル名。</param>
        /// <returns></returns>
        string ToStatementTableName(string tableName);
        /// <summary>
        /// カラム名を文内で使用可能な文字列に変換。
        /// </summary>
        /// <param name="columnName">カラム名。</param>
        /// <returns></returns>
        string ToStatementColumnName(string columnName);
        /// <summary>
        /// バインド変数名を使用可能な文字列に変換。
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="index">0基点のパラメータ番号。</param>
        /// <returns></returns>
        string ToStatementParameterName(string parameterName, int index);

        /// <summary>
        /// 文を単一行コメントに変換。
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string ToSingleLineComment(string statement);
        /// <summary>
        /// 文を複数行コメントに変換。
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string ToMultiLineComment(string statement);
        /// <summary>
        /// 実行文に対するエスケープ処理。
        /// <para>基本的にはバインド処理で対応すること。本処理はしゃあなし動的SQL作成時に無理やり使用する前提。</para>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Escape(string input);

        /// <summary>
        /// like句のエスケープ処理を実施。
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        string EscapeLike(string pattern);

        #endregion
    }

    public class DatabaseImplementation: IDatabaseImplementation
    {
        #region IDatabaseImplementation

        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDDL"/>
        public virtual bool SupportedTransactionDDL { get; } = false;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDML"/>
        public virtual bool SupportedTransactionDML { get; } = true;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedSingleLineComment"/>
        public virtual bool SupportedSingleLineComment { get; } = true;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedMultiLineComment"/>
        public virtual bool SupportedMultiLineComment { get; } = true;


        /// <inheritdoc cref="IDatabaseImplementation.PreFormatStatement(string)"/>
        public virtual string PreFormatStatement(string statement) => statement;

        /// <inheritdoc cref="IDatabaseImplementation.ToStatementTableName(string)"/>
        public virtual string ToStatementTableName(string tableName) => tableName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementColumnName(string)"/>
        public virtual string ToStatementColumnName(string columnName) => columnName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementParameterName(string, int)"/>
        public virtual string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        /// <inheritdoc cref="IDatabaseImplementation.ToSingleLineComment(string)"/>
        public virtual string ToSingleLineComment(string statement)
        {
            return TextUtility.ReadLines(statement)
                .Select(i => "--" + i)
                .JoinString(Environment.NewLine)
            ;
        }

        /// <inheritdoc cref="IDatabaseImplementation.ToMultiLineComment(string)"/>
        public virtual string ToMultiLineComment(string statement)
        {
            var builder = new StringBuilder(Environment.NewLine.Length * 4 + "/**/".Length + statement.Length);
            builder.AppendLine(Environment.NewLine);
            builder.AppendLine("/*");
            builder.AppendLine(Environment.NewLine);
            builder.AppendLine(statement);
            builder.AppendLine(Environment.NewLine);
            builder.AppendLine("*/");
            builder.AppendLine(Environment.NewLine);

            return builder.ToString();
        }

        /// <inheritdoc cref="IDatabaseImplementation.Escape(string)"/>
        public virtual string Escape(string input) => input
            .Replace("\\", @"\\")
            .Replace("\'", @"''")
            .Replace("\r", @"\r")
            .Replace("\n", @"\n")
        ;

        /// <inheritdoc cref="IDatabaseImplementation.EscapeLike(string)"/>
        public virtual string EscapeLike(string pattern) => pattern
            .Replace("\\", @"\\")
            .Replace("%", @"\%")
            .Replace("_", @"\_")
        ;

        #endregion
    }
}
