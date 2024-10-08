using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain
{
    public class LauncherItemDomainDao: DomainDaoBase
    {
        #region define

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "<保留中>")]
        private sealed class LauncherItemIconRowDto: RowDtoBase
        {
            #region property

            public string? Kind { get; set; }
            public string? FilePath { get; set; }
            public long FileIndex { get; set; }
            public string? IconPath { get; set; }
            public long IconIndex { get; set; }

            #endregion
        }

        #endregion

        public LauncherItemDomainDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherIconData ConvertFromDto(LauncherItemIconRowDto dto)
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

        public LauncherIconData SelectFileIcon(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Context.QuerySingle<LauncherItemIconRowDto>(statement, param);
            var data = ConvertFromDto(dto);
            return data;
        }

        #endregion
    }
}
