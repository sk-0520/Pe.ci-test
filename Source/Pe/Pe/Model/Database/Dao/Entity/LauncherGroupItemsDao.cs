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
    public class LauncherGroupItemsDao : ApplicationDatabaseObjectBase
    {
        public LauncherGroupItemsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        public long SelectMaxSort(Guid groupId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.QuerySingle<long>(sql, new { LauncherGroupId = groupId });
        }

        public void InsertNewItems(Guid groupId, IEnumerable<Guid> itemIds)
        {
            var status = DatabaseCommonStatus.CreateUser();
            var sql = StatementLoader.LoadStatementByCurrent();
            var step = 10;
            var counter = 1;
            var currentMaxSort = SelectMaxSort(groupId);
            foreach(var itemId in itemIds) {
                var dto = new LauncherGroupItemsRowDto() {
                    LauncherGroupId = groupId,
                    LauncherItemId = itemId,
                    Sort = currentMaxSort + (step * (counter++)),
                };
                status.WriteCommon(dto);
                Commander.Execute(sql, dto);
            }
        }

        #endregion
    }
}
