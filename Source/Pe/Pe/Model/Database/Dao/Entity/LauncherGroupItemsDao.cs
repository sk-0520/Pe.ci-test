using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherGroupItemsDao : EntityDaoBase
    {
        public LauncherGroupItemsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        public long SelectMaxSort(Guid groupId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.QuerySingle<long>(sql, new { LauncherGroupId = groupId });
        }

        public IEnumerable<Guid> SelectAllLauncherGroupItemIds()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<Guid>(sql);
        }

        public IEnumerable<Guid> SelectLauncherItemIds(Guid launcherGroupId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            return Commander.Query<Guid>(sql, param);
        }

        public void InsertNewItems(Guid groupId, IEnumerable<Guid> itemIds, long startSort, int sortStep, DatabaseCommonStatus commonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var counter = 0;
            foreach(var itemId in itemIds) {
                var dto = new LauncherGroupItemsRowDto() {
                    LauncherGroupId = groupId,
                    LauncherItemId = itemId,
                    Sort = startSort + (sortStep * (counter++)),
                };
                commonStatus.WriteCommon(dto);
                Commander.Execute(sql, dto);
            }
        }

        #endregion
    }
}
