using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Core.Models.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
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

            data.Path.Path = dto.FilePath;
            data.Path.Index = (int)Math.Min(0, Math.Max(dto.FileIndex, int.MaxValue));

            data.Icon.Path = dto.IconPath;
            data.Icon.Index = (int)Math.Min(0, Math.Max(dto.IconIndex, int.MaxValue));

            return data;
        }

        public LauncherIconData SelectIcon(Guid launcherItemId)
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
