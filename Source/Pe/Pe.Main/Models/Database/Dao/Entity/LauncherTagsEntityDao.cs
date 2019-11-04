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
    public class LauncherTagsEntityDao : EntityDaoBase
    {
        public LauncherTagsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string TagName { get; } = "TagName";

            #endregion
        }

        #endregion

        #region function

        public IEnumerable<string> SelectTags(Guid launcherItemId)
        {
            var builder = CreateSelectBuilder();
            builder.AddSelect(Column.TagName);
            builder.AddValueParameter(Column.LauncherItemId, launcherItemId);

            return Select<string>(builder);
        }

        public void InsertNewTags(Guid launcherItemId, IEnumerable<string> tags, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            foreach(var tag in tags) {
                var dto = new LauncherTagsRowDto() {
                    LauncherItemId = launcherItemId,
                    TagName = tag,
                };
                commonStatus.WriteCommon(dto);
                Commander.Execute(statement, dto);
            }
        }

        public int DeleteTagByLauncherItemId(Guid launcherItemId)
        {
            var builder = CreateDeleteBuilder();
            builder.AddKey(Column.LauncherItemId, launcherItemId);
            return ExecuteDelete(builder);
        }

        #endregion
    }
}
