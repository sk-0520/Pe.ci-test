using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Application;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Application
{
    public class ApplicationLauncherItemDao : ApplicationDatabaseObjectBase
    {
        public ApplicationLauncherItemDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region function

        LauncherIconData ConvertFromDto(IReadOnlyLauncherItemsIconRowDto dto)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();

            var data = new LauncherIconData() {
                Kind = kindEnumTransfer.From(dto.Kind),
            };

            data.Command.Path = dto.CommandPath;
            data.Command.Index = (int)Math.Min(0, Math.Max(dto.CommandIndex, int.MaxValue));

            data.Icon.Path = dto.IconPath;
            data.Icon.Index = (int)Math.Min(0, Math.Max(dto.IconIndex, int.MaxValue));

            return data;
        }

        public LauncherIconData SelectIcon(Guid launcherItemId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            var dto = Commander.QuerySingle<LauncherItemIconRowDto>(sql, param);
            var data = ConvertFromDto(dto);
            return data;
        }


        #endregion
    }
}
