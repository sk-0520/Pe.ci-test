using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Base.Linq;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    /// <summary>
    /// データベースアクセス基底！
    /// </summary>
    /// <remarks>
    /// <para>詳細なアクセス手法は実装先にて処理する。</para>
    /// </remarks>
    public abstract class DatabaseAccessObjectBase
    {
        #region variable

        /// <summary>
        /// <see cref="ProcessBodyRegex"/> 実体。
        /// </summary>
        private Regex? _processBodyRegex;
        /// <summary>
        /// <see cref="ProcessContentRegex"/> 実体。
        /// </summary>
        private Regex? _processContentRegex;

        #endregion

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statementLoader"></param>
        /// <param name="implementation"></param>
        /// <param name="loggerFactory"></param>
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
        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <summary>
        /// 行終端文字列を取得または初期設定。
        /// </summary>
        public string NewLine { get; set; /* TODO: 本来 init */ } = Environment.NewLine;
        /// <summary>
        /// ファイル読み込み時に使用するキーの結合文字列。
        /// </summary>
        /// <remarks>
        /// <para>ファイル名に使用出来てメソッド名に使用できない(できなさそう)なのが良い。</para>
        /// </remarks>
        protected string JoinSeparator { get; set; /* TODO: 本来 init */ } = "!";

        /// <summary>
        /// 処理対象文の取得用正規表現。
        /// </summary>
        /// <remarks>
        /// <para>KEY: 処理対象キー, BODY: 文。</para>
        /// </remarks>
        protected virtual Regex ProcessBodyRegex
        {
            get
            {
                if(this._processBodyRegex is null) {
                    Debug.Assert(Implementation.SupportedBlockComment);
                    Debug.Assert(Implementation.BlockComments.Any());

                    var blockComment = Implementation.BlockComments.First();

                    var process = (
                        begin: Regex.Escape(blockComment.Begin + Implementation.ProcessBodyRange.Begin + blockComment.End),
                        end: Regex.Escape(blockComment.Begin + Implementation.ProcessBodyRange.End + blockComment.End)
                    );
                    var block = (
                        begin: Regex.Escape(blockComment.Begin),
                        end: Regex.Escape(blockComment.End)
                    );

                    this._processBodyRegex = new Regex(
                        process.begin + @$"{block.begin}(?<KEY>\w+)\s*(?<BODY>[.\s\S]*?)" + process.end,
                        RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
                    );
                }

                return this._processBodyRegex;
            }
        }

        /// <summary>
        /// 置き換え対象文取得用正規表現。
        /// </summary>
        /// <remarks>
        /// <para>NAME: 対象名, KIND: 文(CODE)/読込(LOAD)</para>
        /// </remarks>
        protected virtual Regex ProcessContentRegex
        {
            get
            {
                if(this._processContentRegex is null) {
                    this._processContentRegex = new Regex(@"^\s*(?<NAME>\w+)\s*:\s*(?<KIND>(CODE)|(LOAD))\s*$", RegexOptions.Compiled);
                }

                return this._processContentRegex;
            }
        }


        #endregion

        #region function

        /// <summary>
        /// <see cref="IDatabaseStatementLoader.LoadStatementByCurrent(Type, string)"/>のヘルパー関数。
        /// </summary>
        /// <param name="callerMemberName"><see cref="CallerMemberNameAttribute"/></param>
        /// <returns></returns>
        protected string LoadStatement([CallerMemberName] string callerMemberName = "")
        {
            var type = GetType();
            return StatementLoader.LoadStatementByCurrent(type, callerMemberName);
        }

        /// <summary>
        /// 指定条件に合わせて文を加工する。
        /// </summary>
        /// <remarks>
        /// <para>必須条件として<see cref="IDatabaseImplementation.SupportedBlockComment"/>が真、<see cref="IDatabaseImplementation.BlockComments"/>が1要素以上。</para>
        /// <para><see cref="IDatabaseImplementation.BlockComments"/>最初の要素が使用される。</para>
        /// </remarks>
        /// <example>
        /// select *
        /// from /*{{*//*KEY[改行]
        /// KEY-A:CODE[改行] <paramref name="blocks"/>["KEY"] -> KEY-A
        ///     TABLE_A
        /// KEY-B:CODE[改行] <paramref name="blocks"/>["KEY"] -> KEY-B
        ///     TABLE_B
        /// KEY-C:LOAD[改行] <paramref name="blocks"/>["KEY"] -> KEY-C, <see cref="LoadStatement"/>(callerMemberName<see cref="JoinSeparator"/>NAME)
        ///     NAME
        /// */TABLE_C/*}}*/ <paramref name="blocks"/>["KEY"] not KEY-A,KEY-B,KEY-C
        /// </example>
        /// <param name="statement"></param>
        /// <param name="blocks"></param>
        /// <param name="callerMemberName"><see cref="CallerMemberNameAttribute"/></param>
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

            return ProcessBodyRegex.Replace(statement, m => ReplaceStatement(m, blocks, callerMemberName));
        }

        private string ReplaceStatement(Match match, IReadOnlyDictionary<string, string> blocks, string callerMemberName)
        {
            var blockComment = Implementation.BlockComments.First();

            var key = match.Groups["KEY"].Value;
            var body = match.Groups["BODY"].Value;

            var bodyLastIndex = body.IndexOf(blockComment.End);
            var dynamicContent = body.Substring(0, bodyLastIndex - blockComment.End.Length);
            var defaultContent = body.Substring(bodyLastIndex + blockComment.End.Length);

            if(!blocks.TryGetValue(key, out var name)) {
                return defaultContent;
            }

            var lines = TextUtility.ReadLines(dynamicContent)
                .Counting()
                .ToArray()
            ;

            var matchedItems = lines
                .Select(i => (item: i, match: ProcessContentRegex.Match(i.Value)))
                .Where(i => i.match.Success)
                .ToArray()
            ;
            for(var i = 0; i < matchedItems.Length; i++) {
                var matchedItem = matchedItems[i];
                if(matchedItem.match.Groups["NAME"].Value == name) {
                    var nextIndex = i + 1 < matchedItems.Length
                        ? matchedItems[i + 1].item.Number
                        : lines.Length
                    ;
                    var contentIndex = matchedItem.item.Number + 1;
                    var contentLines = lines[contentIndex..nextIndex];
                    var kind = matchedItem.match.Groups["KIND"].Value;
                    if(kind == "CODE") {
                        return contentLines
                            .Select(i => i.Value)
                            .JoinString(NewLine)
                        ;
                    } else {
                        Debug.Assert(kind == "LOAD");
                        return contentLines
                            .Select(i => i.Value.Trim())
                            .Where(i => !string.IsNullOrEmpty(i))
                            .Select(i => LoadStatement(callerMemberName + JoinSeparator + i))
                            .JoinString(NewLine)
                        ;
                    }
                }
            }

            return defaultContent;
        }

        #endregion
    }
}
