using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherGroupItemsEntityDao : EntityDaoBase
    {
        public LauncherGroupItemsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        public long SelectMaxSequence(Guid groupId)
        {
            var statement = LoadStatement();
            return Commander.QuerySingle<long>(statement, new { LauncherGroupId = groupId });
        }

        public IEnumerable<Guid> SelectAllLauncherGroupItemIds()
        {
            var statement = LoadStatement();
            return Commander.Query<Guid>(statement);
        }

        public IEnumerable<Guid> SelectLauncherItemIds(Guid launcherGroupId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            return Commander.Query<Guid>(statement, param);
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
                commonStatus.WriteCommon(dto);
                Commander.Execute(statement, dto);
            }
        }

        #endregion
    }
}
