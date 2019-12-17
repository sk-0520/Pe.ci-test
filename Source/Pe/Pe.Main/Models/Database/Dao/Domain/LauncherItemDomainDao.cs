using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Core.Models.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    internal interface IReadOnlyLauncherItemsIconRowDto : IReadOnlyRowDtoBase
    {
        #region property

        string? Kind { get; }
        string? FilePath { get; }
        long FileIndex { get; }
        string? IconPath { get; }
        long IconIndex { get; }

        #endregion
    }

    internal class LauncherItemIconRowDto : RowDtoBase, IReadOnlyLauncherItemsIconRowDto
    {
        #region IReadOnlyLauncherItemsIconRowDto

        public string? Kind { get; set; }
        public string? FilePath { get; set; }
        public long FileIndex { get; set; }
        public string? IconPath { get; set; }
        public long IconIndex { get; set; }

        #endregion
    }

    public class LauncherItemDomainDao : DomainDaoBase
    {
        public LauncherItemDomainDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        LauncherIconData ConvertFromDto(IReadOnlyLauncherItemsIconRowDto dto)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var data = new LauncherIconData() {
                Kind = kindEnumTransfer.ToEnum(dto.Kind),
            };

            data.Path.Path = dto.FilePath ?? string.Empty;
            data.Path.Index = ToInt(dto.FileIndex);

            data.Icon.Path = dto.IconPath ?? string.Empty;
            data.Icon.Index = ToInt(dto.IconIndex);

            return data;
        }

        public LauncherIconData SelectFileIcon(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherItemIconRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }


        #endregion
    }
}
