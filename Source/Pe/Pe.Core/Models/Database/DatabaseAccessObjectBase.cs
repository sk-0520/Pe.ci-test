using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        /// </summary>
        /// <example>
        /// select *
        /// from /*/!*//*KEY
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
            if(string.IsNullOrEmpty(statement)) {
                return statement;
            }
            if(blocks.Count == 0) {
                return statement;
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}
