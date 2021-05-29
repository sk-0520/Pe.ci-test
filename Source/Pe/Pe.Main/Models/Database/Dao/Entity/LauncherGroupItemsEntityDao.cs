using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherGroupItemsEntityDao: EntityDaoBase
    {
        #region define

        private class LauncherGroupItemsRowDto: RowDtoBase
        {
            #region property

            public Guid LauncherGroupId { get; set; }
            public Guid LauncherItemId { get; set; }
            public long Sequence { get; set; }

            #endregion
        }

        #endregion

        public LauncherGroupItemsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherGroupId { get; } = "LauncherGroupId";
            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string Sequence { get; } = "Sequence";

            #endregion
        }

        #endregion

        #region function

        public long SelectMaxSequence(Guid groupId)
        {
            var statement = LoadStatement();
            return Context.QuerySingle<long>(statement, new { LauncherGroupId = groupId });
        }

        public IEnumerable<Guid> SelectAllLauncherGroupItemIds()
        {
            var statement = LoadStatement();
            return Context.Query<Guid>(statement);
        }

        public IEnumerable<Guid> SelectLauncherItemIds(Guid launcherGroupId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            return Context.Query<Guid>(statement, param);
        }

        public void InsertNewItems(Guid groupId, IEnumerable<Guid> itemIds, long startSequence, int sortStep, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var counter = 0;
            foreach(var itemId in itemIds) {
                var dto = new LauncherGroupItemsRowDto() {
                    LauncherGroupId = groupId,
                    LauncherItemId = itemId,
                    Sequence = startSequence + (sortStep * (counter++)),
                };
                commonStatus.WriteCommonTo(dto);
                Context.Execute(statement, dto);
            }
        }

        public int DeleteGroupItemsByLauncherItemId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Execute(statement, parameter);
        }

        public int DeleteGroupItemsByLauncherGroupId(Guid launcherGroupId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherGroupId = launcherGroupId,
            };
            return Context.Execute(statement, parameter);
        }

        public bool DeleteGroupItemsLauncherItem(Guid launcherGroupId, Guid launcherItemId, int index)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherGroupId = launcherGroupId,
                LauncherItemId = launcherItemId,
                ItemIndex = index,
            };
            return Context.Execute(statement, parameter) == 1;
        }

        #endregion
    }
}
