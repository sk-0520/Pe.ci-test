using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    // 既存の EntityRemoverBase はもうだめだ！
    // トランザクション管理できないから死ぬしかない！
    /// <summary>
    /// 特定の削除処理を一括して行う。
    /// <para>ランチャーアイテム削除時とかもうしんどいのよ。</para>
    /// </summary>
    public abstract class EntityEraserBase
    {
        protected EntityEraserBase(IDatabaseContextsPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(contextsPack.Main, contextsPack.File, contextsPack.Temporary, statementLoader, loggerFactory)
        { }

        protected EntityEraserBase(IDatabaseContexts mainContexts, IDatabaseContexts fileContexts, IDatabaseContexts temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainContexts = mainContexts ?? throw new ArgumentNullException(nameof(mainContexts));
            FileContexts = fileContexts ?? throw new ArgumentNullException(nameof(fileContexts));
            TemporaryContexts = temporaryContexts ?? throw new ArgumentNullException(nameof(temporaryContexts));
            StatementLoader = statementLoader ?? throw new ArgumentNullException(nameof(statementLoader));
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private IDatabaseContexts MainContexts { get; }
        private IDatabaseContexts FileContexts { get; }
        private IDatabaseContexts TemporaryContexts { get; }
        private IDatabaseStatementLoader StatementLoader { get; }

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion

        #region function

        protected abstract void ExecuteMain(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteFile(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteTemporary(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);


        public void Execute()
        {
            ExecuteMain(MainContexts.Context, StatementLoader, MainContexts.Implementation);
            ExecuteFile(FileContexts.Context, StatementLoader, FileContexts.Implementation);
            ExecuteTemporary(TemporaryContexts.Context, StatementLoader, TemporaryContexts.Implementation);
        }

        #endregion
    }
}
