using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public abstract class EntityDaoBase : ApplicationDatabaseObjectBase
    {
        protected EntityDaoBase(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public virtual string TableName
        {
            get
            {
                var className = GetType().Name;
                var suffix = "EntityDao";
                if(className.EndsWith(suffix)) {
                    return className.Substring(0, className.Length - suffix.Length);
                }

                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
