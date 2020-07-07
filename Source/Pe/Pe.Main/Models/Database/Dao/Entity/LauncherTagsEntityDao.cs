using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    internal class LauncherTagsRowDto: RowDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string TagName { get; set; } = string.Empty;

        #endregion
    }

    public class LauncherTagsEntityDao: EntityDaoBase
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
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Query<string>(statement, parameter);
        }

        public IEnumerable<string> SelectUniqueTags(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Query<string>(statement, parameter);
        }

        public IDictionary<Guid, List<string>> SelectAllTags()
        {
            var statement = LoadStatement();
            var map = Commander.Query<(Guid id, string tag)>(statement)
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
                commonStatus.WriteCommon(dto);
                Commander.Execute(statement, dto);
            }
        }

        public int DeleteTagByLauncherItemId(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Execute(statement, parameter);
        }

        #endregion
    }
}
