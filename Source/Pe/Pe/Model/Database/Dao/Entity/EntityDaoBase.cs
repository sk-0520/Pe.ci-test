using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public abstract class EntityDaoBase : ApplicationDatabaseObjectBase
    {
        public EntityDaoBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILogger logger)
            : base(commander, statementLoader, implementation, logger)
        { }
        public EntityDaoBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        protected static IReadOnlyList<string> CommonCreateColumns { get; } = new[] {
            "CreatedTimestamp",
            "CreatedAccount",
            "CreatedProgramName",
            "CreatedProgramVersion",
        };
        protected static IReadOnlyList<string> CommonUpdateColumns { get; } = new[] {
            "UpdatedTimestamp",
            "UpdatedAccount",
            "UpdatedProgramName",
            "UpdatedProgramVersion",
            "UpdatedCount",
        };

        public virtual string TableName
        {
            get
            {
                var className = GetType().Name;
                var suffix = "Dao";
                if(className.EndsWith(suffix)) {
                    return className.Substring(0, className.Length - suffix.Length);
                }

                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
