using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherItemsEntityDao : EntityDaoBase
    {
        public LauncherItemsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation , loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        LauncherItemsRowDto ConvertFromData(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();
            var permissionEnumTransfer = new EnumTransfer<LauncherItemPermission>();

            var dto = new LauncherItemsRowDto() {
                LauncherItemId = data.LauncherItemId,
                Code = data.Code,
                Name = data.Name,
                Kind = kindEnumTransfer.ToText(data.Kind),
                IconPath = data.Icon.Path,
                IconIndex = data.Icon.Index,
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                Comment = Implementation.ToNullValue(data.Comment),
            };

            commonStatus.WriteCommon(dto);

            return dto;
        }

        LauncherItemData ConvertFromDto(IReadOnlyLauncherItemsRowDto dto)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var data = new LauncherItemData() {
                LauncherItemId = dto.LauncherItemId,
                Name = dto.Name,
                Code = dto.Code,
                Kind = kindEnumTransfer.ToEnum(dto.Kind),
                IsEnabledCommandLauncher= dto.IsEnabledCommandLauncher,
                Comment = dto.Comment,
            };

            return data;
        }

        public IEnumerable<string> SelectFuzzyCodes(string baseCode)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<string>(statement, new { BaseCode = baseCode });
        }

        public LauncherItemData SelectLauncherItem(Guid launcherItemId)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherItemsRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(statement, dto);
        }

        #endregion
    }
}
