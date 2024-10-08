using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    /// <summary>
    /// 特定の削除処理を一括して行う。
    /// </summary>
    /// <remarks>
    /// <para>ランチャーアイテム削除時とかもうしんどいのよ。</para>
    /// </remarks>
    public abstract class EntityEraserBase
    {
        protected EntityEraserBase(IDatabaseContextsPack contextsPack, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : this(contextsPack.Main, contextsPack.Large, contextsPack.Temporary, statementLoader, loggerFactory)
        { }

        protected EntityEraserBase(IDatabaseContexts mainContexts, IDatabaseContexts fileContexts, IDatabaseContexts temporaryContexts, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
        {
            MainContexts = mainContexts ?? throw new ArgumentNullException(nameof(mainContexts));
            LargeContexts = fileContexts ?? throw new ArgumentNullException(nameof(fileContexts));
            TemporaryContexts = temporaryContexts ?? throw new ArgumentNullException(nameof(temporaryContexts));
            StatementLoader = statementLoader ?? throw new ArgumentNullException(nameof(statementLoader));
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private IDatabaseContexts MainContexts { get; }
        private IDatabaseContexts LargeContexts { get; }
        private IDatabaseContexts TemporaryContexts { get; }
        private IDatabaseStatementLoader StatementLoader { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        #endregion

        #region function

        protected abstract void ExecuteMainImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteLargeImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract void ExecuteTemporaryImpl(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);


        public void Execute()
        {
            ExecuteMainImpl(MainContexts.Context, StatementLoader, MainContexts.Implementation);
            ExecuteLargeImpl(LargeContexts.Context, StatementLoader, LargeContexts.Implementation);
            ExecuteTemporaryImpl(TemporaryContexts.Context, StatementLoader, TemporaryContexts.Implementation);
        }

        #endregion
    }
}
