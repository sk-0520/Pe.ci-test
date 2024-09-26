using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class SettingExporterDomainDao: DomainDaoBase
    {
        #region define

        private sealed record class SettingGroupDao(
            LauncherGroupId LauncherGroupId,
            string LauncherGroupName,
            LauncherItemId LauncherItemId,
            string LauncherItemName,
            string Kind
        );

        #endregion

        public SettingExporterDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        public IEnumerable<SettingGroup> SelectSettingGroups()
        {
            var launcherItemKindTransfer = new EnumTransfer<LauncherItemKind>();

            var statement = LoadStatement();
            var groups = Context.Query<SettingGroupDao>(statement)
                .GroupBy(a => (a.LauncherGroupId, a.LauncherGroupName))
            ;

            foreach(var group in groups) {
                yield return new SettingGroup(
                    group.Key.LauncherGroupId,
                    group.Key.LauncherGroupName,
                    group.Select(a => new SettingGroupItem(
                        a.LauncherItemId,
                        a.LauncherItemName,
                        launcherItemKindTransfer.ToEnum(a.Kind)
                    )).ToArray()
                );
            }
        }

        public IEnumerable<SettingLauncherItem> SelectSettingLauncherItems()
        {

        }

        #endregion
    }
}
