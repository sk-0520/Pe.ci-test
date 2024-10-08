using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherGroupItemsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherGroupItemsRowDto: RowDtoBase
        {
            #region property

            public LauncherGroupId LauncherGroupId { get; set; }
            public LauncherItemId LauncherItemId { get; set; }
            public long Sequence { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherGroupId { get; } = "LauncherGroupId";
            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string Sequence { get; } = "Sequence";

            #endregion
        }

        #endregion

        public LauncherGroupItemsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public long SelectMaxSequence(LauncherGroupId groupId)
        {
            var statement = LoadStatement();
            return Context.QuerySingle<long>(statement, new { LauncherGroupId = groupId });
        }

        public IEnumerable<LauncherItemId> SelectLauncherItemIds(LauncherGroupId launcherGroupId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            return Context.Query<LauncherItemId>(statement, param);
        }

        public void InsertNewItems(LauncherGroupId groupId, IEnumerable<LauncherItemId> itemIds, long startSequence, int sortStep, IDatabaseCommonStatus commonStatus)
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
                Context.InsertSingle(statement, dto);
            }
        }

        public int DeleteGroupItemsByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Delete(statement, parameter);
        }

        public int DeleteGroupItemsByLauncherGroupId(LauncherGroupId launcherGroupId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherGroupId = launcherGroupId,
            };
            return Context.Delete(statement, parameter);
        }

        public void DeleteGroupItemsLauncherItem(LauncherGroupId launcherGroupId, LauncherItemId launcherItemId, int index)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherGroupId = launcherGroupId,
                LauncherItemId = launcherItemId,
                ItemIndex = index,
            };
            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
