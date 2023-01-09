using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    [Obsolete]
    public struct EntityRemoverResultItem
    {
        public EntityRemoverResultItem(string entityName, int count)
        {
            EntityName = entityName;
            Count = count;
        }

        #region property

        public string EntityName { get; }
        public int Count { get; }

        #endregion
    }

    [Obsolete]
    public class EntityRemoverResult
    {
        public EntityRemoverResult(Pack target)
        {
            Target = target;
        }

        #region property

        public Pack Target { get; }

        public IList<EntityRemoverResultItem> Items { get; } = new List<EntityRemoverResultItem>();

        #endregion
    }

    [Obsolete]
    public abstract class EntityRemoverBase
    {
        protected EntityRemoverBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        #endregion

        #region function

        public abstract bool IsTarget(Pack pack);

        protected EntityRemoverResultItem ExecuteRemove(string entityName, Func<int> func)
        {
            Debug.Assert(func != null);
            var count = func();
            return new EntityRemoverResultItem(entityName, count);
        }

        protected abstract EntityRemoverResult RemoveMain(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract EntityRemoverResult RemoveFile(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);
        protected abstract EntityRemoverResult RemoveTemporary(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);

        public EntityRemoverResult Remove(Pack pack, IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
#if DEBUG
            if(!IsTarget(pack)) {
                throw new ArgumentException(nameof(pack));
            }
#endif

            if(context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if(implementation == null) {
                throw new ArgumentNullException(nameof(implementation));
            }

            switch(pack) {
                case Pack.Main:
                    return RemoveMain(context, statementLoader, implementation);

                case Pack.Large:
                    return RemoveMain(context, statementLoader, implementation);

                case Pack.Temporary:
                    return RemoveMain(context, statementLoader, implementation);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }

    [Obsolete]
    public sealed class EntityDeleteDaoGroup
    {
        #region property

        private IList<EntityDaoBase> EntityDaos { get; } = new List<EntityDaoBase>();
        private IDictionary<EntityDaoBase, IList<Func<int>>> DeleteFunctions { get; } = new Dictionary<EntityDaoBase, IList<Func<int>>>();

        #endregion

        #region function

        public void Add<TEntityDao>(TEntityDao entityDao, Func<TEntityDao, int> deleter)
            where TEntityDao : EntityDaoBase
        {
            EntityDaos.Add(entityDao);
            if(!DeleteFunctions.TryGetValue(entityDao, out var list)) {
                list = new List<Func<int>>();
                DeleteFunctions.Add(entityDao, list);
            }

            list.Add(() => deleter(entityDao));
        }

        public IList<EntityRemoverResultItem> Execute()
        {
            var result = new List<EntityRemoverResultItem>(EntityDaos.Count);

            foreach(var entityDao in EntityDaos) {
                var funcs = DeleteFunctions[entityDao];
                var totalCount = 0;
                foreach(var func in funcs) {
                    var count = func();
                    totalCount += count;
                }
                result.Add(new EntityRemoverResultItem(entityDao.TableName, totalCount));
            }

            return result;
        }

        #endregion
    }

    [Obsolete]
    public sealed class EntitiesRemover
    {
        public EntitiesRemover(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader)
        {
            Barriers = new Dictionary<Pack, IApplicationDatabaseBarrier>() {
                [Pack.Main] = mainDatabaseBarrier,
                [Pack.Large] = largeDatabaseBarrier,
                [Pack.Temporary] = temporaryDatabaseBarrier,
            };
            StatementLoader = statementLoader;
        }

        #region property

        private IDictionary<Pack, IApplicationDatabaseBarrier> Barriers { get; }
        private IDatabaseStatementLoader StatementLoader { get; }

        public IList<EntityRemoverBase> Items { get; } = new List<EntityRemoverBase>();

        #endregion

        #region function

        private IDatabaseTransaction? BeginTransaction(Pack pack)
        {
            if(Items.Any(i => i.IsTarget(pack))) {
                var barrier = Barriers[pack];
                return barrier.WaitWrite();
            }

            return null;
        }

        private IEnumerable<EntityRemoverResult> ExecuteCore(Pack pack, IDatabaseContext context, IDatabaseImplementation implementation)
        {
            var items = Items.Where(i => i.IsTarget(pack));
            foreach(var item in items) {
                var entityResult = item.Remove(pack, context, StatementLoader, implementation);
                yield return entityResult;
            }
        }

        public IList<EntityRemoverResult> Execute()
        {

            var packs = new[] {
                Pack.Main,
                Pack.Large,
                Pack.Temporary,
            };
#if DEBUG
            Debug.Assert(Enum.GetValues<Pack>().Count() == packs.Length);
#endif
            var transactions = new Dictionary<Pack, IDatabaseTransaction>();
            try {
                var result = new List<EntityRemoverResult>();

                foreach(var pack in packs) {
                    var transaction = BeginTransaction(pack);
                    if(transaction == null) {
                        continue;
                    }
                    transactions.Add(pack, transaction);
                    foreach(var entityResult in ExecuteCore(pack, transaction, transaction.Implementation)) {
                        result.Add(entityResult);
                    }
                }
                foreach(var transaction in transactions.Values) {
                    transaction.Commit();
                }

                return result;
            } catch(Exception) {
                foreach(var transaction in transactions.Values) {
                    transaction.Rollback();
                }
                throw;
            } finally {
                foreach(var transaction in transactions.Values) {
                    transaction.Dispose();
                }
            }
        }

        #endregion
    }
}
