using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherTagsEntityDao: EntityDaoBase
    {
        #region define

        private class LauncherTagsRowDto: RowDtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }
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

        public IEnumerable<string> SelectTags(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<string>(statement, parameter);
        }

        public IEnumerable<string> SelectUniqueTags(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<string>(statement, parameter);
        }

        public IDictionary<Guid, List<string>> SelectAllTags()
        {
            var statement = LoadStatement();
            var map = Context.Query<(Guid id, string tag)>(statement)
                .GroupBy(i => i.id, i => i.tag)
                .ToDictionary(i => i.Key, i => i.ToList())
            ;
            return map;
        }

        public void InsertTags(Guid launcherItemId, IEnumerable<string> tags, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            foreach(var tag in tags) {
                var dto = new LauncherTagsRowDto() {
                    LauncherItemId = launcherItemId,
                    TagName = tag,
                };
                commonStatus.WriteCommonTo(dto);
                Context.Execute(statement, dto);
            }
        }

        public int DeleteTagByLauncherItemId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Execute(statement, parameter);
        }

        #endregion
    }
}
