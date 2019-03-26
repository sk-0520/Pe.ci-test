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
using ContentTypeTextNet.Pe.Main.Model.Launcher;

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
                Kind = kindEnumTransfer.To(data.Kind),
                IconPath = data.Icon.Path,
                IconIndex = data.Icon.Index,
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                Note = Implementation.IsNull(data.Note) ? Implementation.GetNullValue<string>(): data.Note,
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
                Kind = kindEnumTransfer.From(dto.Kind),
                IsEnabledCommandLauncher= dto.IsEnabledCommandLauncher,
                Note = dto.Note,
            };

            return data;
        }

        public IEnumerable<string> SelectFuzzyCodes(string baseCode)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<string>(sql, new { BaseCode = baseCode });
        }

        public LauncherItemData SelectLauncherItem(Guid launcherItemId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherItemsRowDto>(sql, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(sql, dto);
        }

        #endregion
    }
}
