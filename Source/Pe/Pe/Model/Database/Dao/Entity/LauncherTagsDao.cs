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
    public class LauncherTagsDao : EntityDaoBase
    {
        public LauncherTagsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
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

        public void InsertNewTags(Guid launcherItemId, IEnumerable<string> tags, DatabaseCommonStatus commonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            foreach(var tag in tags) {
                var dto = new LauncherTagsRowDto() {
                    LauncherItemId = launcherItemId,
                    TagName = tag,
                };
                commonStatus.WriteCommon(dto);
                Commander.Execute(sql, dto);
            }
        }

        #endregion
    }
}
