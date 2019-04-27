using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

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
                var suffix = "EntityDao";
                if(className.EndsWith(suffix)) {
                    return className.Substring(0, className.Length - suffix.Length);
                }

                throw new NotImplementedException();
            }
        }

        protected DatabaseUpdateStatementBuilder CreateUpdateBuilder(IDatabaseCommonStatus databaseCommonStatus)
        {
            var result = new DatabaseUpdateStatementBuilder(Implementation, Logger.Factory);
            result.SetTable(TableName);
            foreach(var ignoreColumn in CommonCreateColumns.Concat(CommonUpdateColumns)) {
                result.AddIgnoreWhere(ignoreColumn);
            }
            var mapping = databaseCommonStatus.CreateCommonDtoMapping();
            foreach(var pair in mapping) {
                result.AddValue(pair.Key, pair.Value);
            }

            return result;
        }

        protected int ExecuteUpdate(DatabaseUpdateStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.Execute(statement, param);
        }

        #endregion
    }
}
