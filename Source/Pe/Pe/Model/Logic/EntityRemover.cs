using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public struct EntityRemoverResultItem
    {
        public EntityRemoverResultItem(string entityName, int count)
        {
            EntityName = entityName;
            Count = count;
        }

        #region

        public string EntityName { get; }
        public int Count { get; }
        #endregion
    }

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

    public abstract class EntityRemoverBase
    {
        public EntityRemoverBase(ILogger logger)
        {
            Logger = logger;
        }
        public EntityRemoverBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

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

        protected abstract EntityRemoverResult RemoveImpl(Pack pack, IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation);

        public EntityRemoverResult Remove(Pack pack, IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation)
        {
#if DEBUG
            if(!IsTarget(pack)) {
                throw new ArgumentException(nameof(pack));
            }
#endif

            if(commander == null) {
                throw new ArgumentNullException(nameof(commander));
            }
            if(implementation == null) {
                throw new ArgumentNullException(nameof(implementation));
            }

            return RemoveImpl(pack, commander, statementLoader, implementation);
        }

        #endregion
    }

    public sealed class EntityDeleteDapGroup
    {
        #region property

        IList<EntityDaoBase> EntityDaos { get; } = new List<EntityDaoBase>();
        IDictionary<EntityDaoBase, IList<Func<int>>> DeleteFunctions { get; } = new Dictionary<EntityDaoBase, IList<Func<int>>>();

        #endregion

        #region function

        public void Add<TEntityDao>(TEntityDao entityDao, Func<int> deleter)
            where TEntityDao : EntityDaoBase
        {
            EntityDaos.Add(entityDao);
            if(!DeleteFunctions.TryGetValue(entityDao, out var list)) {
                list = new List<Func<int>>();
                DeleteFunctions.Add(entityDao, list);
            }

            list.Add(deleter);
        }

        #endregion
    }
}
