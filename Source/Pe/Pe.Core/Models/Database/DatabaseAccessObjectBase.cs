using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベースアクセス基底！
    /// <para>詳細なアクセス手法は実装先にて処理する。</para>
    /// </summary>
    public abstract class DatabaseAccessObjectBase
    {
        protected DatabaseAccessObjectBase(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            Context = context;
            StatementLoader = statementLoader;
            Implementation = implementation;
        }

        #region property

        /// <summary>
        /// データベースのお話担当。
        /// </summary>
        protected IDatabaseContext Context { get; }
        /// <summary>
        /// データベース問い合わせ文の読み込みストア。
        /// </summary>
        protected IDatabaseStatementLoader StatementLoader { get; }
        /// <summary>
        /// データベースの実装依存専用窓口。
        /// </summary>
        protected IDatabaseImplementation Implementation { get; }
        /// <summary>
        /// ログ出力担当。
        /// </summary>
        protected ILogger Logger { get; }
        /// <summary>
        /// ログ出力(<see cref="ILogger"/>)生成機構。
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="IDatabaseStatementLoader.LoadStatementByCurrent(Type, string)"/>のヘルパー関数。
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        protected string LoadStatement([CallerMemberName] string callerMemberName = "")
        {
            var type = GetType();
            return StatementLoader.LoadStatementByCurrent(type, callerMemberName);
        }

        /// <summary>
        /// 指定条件に合わせて文を加工する。
        /// <para>必須条件として<see cref="IDatabaseImplementation.SupportedBlockComment"/>が真、<see cref="IDatabaseImplementation.BlockComments"/>が1要素以上。</para>
        /// <para><see cref="IDatabaseImplementation.BlockComments"/>最初の要素が使用される。</para>
        /// </summary>
        /// <example>
        /// select *
        /// from /*/!*//*KEY[改行]
        /// KEY-A: { <paramref name="blocks"/>["KEY"] -> KEY-A
        ///     TABLE_A
        /// }
        /// KEY-B: { <paramref name="blocks"/>["KEY"] -> KEY-B
        ///     TABLE_B
        /// }
        /// KEY-C: NAME <paramref name="blocks"/>["KEY"] -> KEY-C, callerMemberName.NAME.sql
        /// */TABLE_C/*!/*/ <paramref name="blocks"/>["KEY"] not KEY-A,KEY-B,KEY-C
        /// </example>
        /// <param name="statement"></param>
        /// <param name="blocks"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        protected string ProcessStatement(string statement, IReadOnlyDictionary<string, string> blocks, [CallerMemberName] string callerMemberName = "")
        {
            if(!Implementation.SupportedBlockComment) {
                throw new InvalidOperationException(nameof(Implementation.SupportedBlockComment));
            }

            if(string.IsNullOrEmpty(statement)) {
                return statement;
            }
            if(blocks.Count == 0) {
                return statement;
            }

            var blockComment = Implementation.BlockComments.First();

            var process = (
                begin: Regex.Escape(blockComment.Begin + "/!" + blockComment.End),
                end: Regex.Escape(blockComment.Begin + "!/" + blockComment.End)
            );
            var block = (
                begin: Regex.Escape(blockComment.Begin),
                end: Regex.Escape(blockComment.End)
            );

            var regex = new Regex(
                process.begin + @$"{block.begin}(?<KEY>\w+)\s*(?<BODY>[.\s\S]*)" + process.end,
                RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
            );

            return regex.Replace(statement, ReplaceStatement);
        }

        private string ReplaceStatement(Match match)
        {
            var blockComment = Implementation.BlockComments.First();

            var key = match.Groups["KEY"].Value;
            var body = match.Groups["BODY"].Value;

            var bodyLastIndex = body.IndexOf(blockComment.End);
            var bodyContent = body.Substring(0, bodyLastIndex);

            return body;
        }

        #endregion
    }
}
