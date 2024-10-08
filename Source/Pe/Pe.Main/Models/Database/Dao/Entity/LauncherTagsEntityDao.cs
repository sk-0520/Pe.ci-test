using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherTagsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherTagsRowDto: RowDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string TagName { get; set; } = string.Empty;

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";
            public static string TagName { get; } = "TagName";

            #endregion
        }

        #endregion

        public LauncherTagsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        public IEnumerable<string> SelectTags(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<string>(statement, parameter);
        }

        public IEnumerable<string> SelectUniqueTags(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<string>(statement, parameter);
        }

        public IDictionary<LauncherItemId, List<string>> SelectAllTags()
        {
            var statement = LoadStatement();
            var map = Context.Query<(LauncherItemId id, string tag)>(statement)
                .GroupBy(i => i.id, i => i.tag)
                .ToDictionary(i => i.Key, i => i.ToList())
            ;
            return map;
        }

        public void InsertTags(LauncherItemId launcherItemId, IEnumerable<string> tags, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            foreach(var tag in tags) {
                var dto = new LauncherTagsRowDto() {
                    LauncherItemId = launcherItemId,
                    TagName = tag,
                };
                commonStatus.WriteCommonTo(dto);
                Context.InsertSingle(statement, dto);
            }
        }

        public int DeleteTagByLauncherItemId(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
