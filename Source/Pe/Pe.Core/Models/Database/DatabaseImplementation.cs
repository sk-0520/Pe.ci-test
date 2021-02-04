using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        #endregion

        #region function

        /// <summary>
        /// 文実行前に実行する文に対して変換処理を実行。
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string PreFormatStatement(string statement);

        string ToStatementTableName(string tableName);
        string ToStatementColumnName(string columnName);
        string ToStatementParameterName(string parameterName, int index);

        /// <summary>
        /// 実行文に対するエスケープ処理。
        /// <para>基本的にはバインド処理で対応すること。本処理はしゃあなし動的SQL作成時に無理やり使用する前提。</para>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Escape(string input);

        #endregion
    }

    public class DatabaseImplementation: IDatabaseImplementation
    {
        #region IDatabaseImplementation

        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDDL"/>
        public virtual bool SupportedTransactionDDL { get; } = false;
        /// <inheritdoc cref="IDatabaseImplementation.SupportedTransactionDML"/>
        public virtual bool SupportedTransactionDML { get; } = true;

        /// <inheritdoc cref="IDatabaseImplementation.PreFormatStatement(string)"/>
        public virtual string PreFormatStatement(string statement) => statement;

        /// <inheritdoc cref="IDatabaseImplementation.ToStatementTableName(string)"/>
        public virtual string ToStatementTableName(string tableName) => tableName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementColumnName(string)"/>
        public virtual string ToStatementColumnName(string columnName) => columnName;
        /// <inheritdoc cref="IDatabaseImplementation.ToStatementParameterName(string, int)"/>
        public virtual string ToStatementParameterName(string parameterName, int index) => "@" + parameterName;

        /// <inheritdoc cref="IDatabaseImplementation.Escape(string)"/>
        public virtual string Escape(string input) => input
            .Replace("\\", @"\\")
            .Replace("\'", @"''")
            .Replace("\r", @"\r")
            .Replace("\n", @"\n")
        ;

        #endregion
    }
}
