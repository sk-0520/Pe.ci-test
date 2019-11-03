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
    public class LauncherItemsEntityDao : EntityDaoBase
    {
        public LauncherItemsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation , loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId { get; } = "LauncherItemId";

            #endregion
        }

        #endregion

        #region function

        LauncherItemsRowDto ConvertFromData(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var dto = new LauncherItemsRowDto() {
                LauncherItemId = data.LauncherItemId,
                Code = data.Code,
                Name = data.Name,
                Kind = kindEnumTransfer.ToString(data.Kind),
                IconPath = data.Icon.Path ?? string.Empty,
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
                IsEnabledCommandLauncher = dto.IsEnabledCommandLauncher,
                Icon = new IconData() {
                    Path = dto.IconPath ?? string.Empty,
                    Index = ToInt(dto.IconIndex),
                },
                Comment = dto.Comment,
            };

            return data;
        }

        public IEnumerable<string> SelectFuzzyCodes(string baseCode)
        {
            var statement = LoadStatement();
            return Commander.Query<string>(statement, new { BaseCode = baseCode });
        }

        public LauncherItemData SelectLauncherItem(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherItemsRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        public void InsertLauncherItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(statement, dto);
        }

        public bool UpdateExecuteCountIncrement(Guid launcherItemId, IDatabaseCommonStatus databaseCommonStatus)
        {
            var statement = LoadStatement();
            var param = databaseCommonStatus.CreateCommonDtoMapping();
            param[Column.LauncherItemId] = launcherItemId;

            return Commander.Execute(statement, param) == 1;
        }

        public bool UpdateCustomizeLauncherItem(LauncherItemData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
