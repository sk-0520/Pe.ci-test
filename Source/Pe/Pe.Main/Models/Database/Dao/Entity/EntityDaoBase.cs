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
            UpdatedCount,
        };
        protected static string UpdatedCount { get; } = "UpdatedCount";

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

        [Obsolete]
        protected DatabaseSelectStatementBuilder CreateSelectBuilder()
        {
            var result = new DatabaseSelectStatementBuilder(Implementation, LoggerFactory);
            result.SetTable(TableName);

            return result;
        }

        [Obsolete]
        protected DatabaseUpdateStatementBuilder CreateUpdateBuilder(IDatabaseCommonStatus databaseCommonStatus)
        {
            var result = new DatabaseUpdateStatementBuilder(Implementation, LoggerFactory);
            result.SetTable(TableName);
            foreach(var ignoreColumn in CommonUpdateColumns.Where(i => i != UpdatedCount)) {
                result.AddIgnoreWhere(ignoreColumn);
            }
            var mapping = databaseCommonStatus.CreateCommonDtoMapping();
            foreach(var pair in mapping) {
                if(pair.Key == UpdatedCount) {
                    result.AddPlainParameter(UpdatedCount, $"{Implementation.ToStatementColumnName(UpdatedCount)} + 1");
                } else if(CommonUpdateColumns.Contains(pair.Key)) {
                    result.AddValueParameter(pair.Key, pair.Value);
                }
            }

            return result;
        }

        [Obsolete]
        protected DatabaseDeleteStatementBuilder CreateDeleteBuilder()
        {
            var result = new DatabaseDeleteStatementBuilder(Implementation, LoggerFactory);
            result.SetTable(TableName);

            return result;
        }

        //[Obsolete]
        protected T SelectSingle<T>(DatabaseSelectStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.QuerySingle<T>(statement, param);
        }

        //[Obsolete]
        protected T SelectFirst<T>(DatabaseSelectStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.QueryFirst<T>(statement, param);
        }

        //[Obsolete]
        protected IEnumerable<T> Select<T>(DatabaseSelectStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.Query<T>(statement, param);
        }

        //[Obsolete]
        protected int ExecuteUpdate(DatabaseUpdateStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.Execute(statement, param);
        }

        //[Obsolete]
        protected int ExecuteDelete(DatabaseDeleteStatementBuilder builder)
        {
            var statement = builder.BuildStatement();
            var param = builder.Parameters;
            return Commander.Execute(statement, param);
        }

        #endregion
    }
}
