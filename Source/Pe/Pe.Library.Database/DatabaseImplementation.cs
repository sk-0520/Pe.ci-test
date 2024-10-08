using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <inheritdoc cref="IDatabaseImplementation"/>
    public class DatabaseImplementation: IDatabaseImplementation
    {
        #region IDatabaseImplementation

        /// <inheritdoc cref="IDatabaseImplementation.NewLine"/>
        public string NewLine { get; } = Environment.NewLine;

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
            builder.Append(NewLine);
            builder.Append(blockComment.Begin);
            builder.Append(NewLine);
            builder.Append(statement);
            builder.Append(NewLine);
            builder.Append(blockComment.End);
            builder.Append(NewLine);

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

        public virtual IDatabaseManagement CreateManagement(IDatabaseContext context)
        {
            return new DatabaseManagementWithContext(context, this);
        }

        #endregion
    }
}
