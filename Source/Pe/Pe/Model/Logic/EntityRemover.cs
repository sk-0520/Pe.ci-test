using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;

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

        public abstract bool TargetIsMain { get; }
        public abstract bool TargetIsFile { get; }
        public abstract bool TargetIsTemporary { get; }

        #endregion

        #region function

        protected abstract EntityRemoverResult RemoveImpl(Pack pack, IDatabaseCommander commander, IDatabaseImplementation implementation);

        public EntityRemoverResult Remove(Pack pack, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            switch(pack) {
                case Pack.Main:
                    if(!TargetIsMain) {
                        throw new ArgumentException(nameof(pack));
                    }
                    break;

                case Pack.File:
                    if(!TargetIsFile) {
                        throw new ArgumentException(nameof(pack));
                    }
                    break;

                case Pack.Temporary:
                    if(!TargetIsTemporary) {
                        throw new ArgumentException(nameof(pack));
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(commander == null) {
                throw new ArgumentNullException(nameof(commander));
            }
            if(implementation == null) {
                throw new ArgumentNullException(nameof(implementation));
            }

            return RemoveImpl(pack, commander, implementation);
        }

        #endregion
    }
}
