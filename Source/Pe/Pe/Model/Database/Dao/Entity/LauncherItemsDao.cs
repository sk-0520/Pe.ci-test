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
    public class LauncherItemsDao : ApplicationDatabaseObjectBase
    {
        public LauncherItemsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        LauncherItemsRowDto ConvertFromData(LauncherItemSimpleNewData data)
        {
            var kindEnumTransfer = new EnumTransfer<LauncherItemKind>();
            var permissionEnumTransfer = new EnumTransfer<LauncherItemPermission>();

            var dto = new LauncherItemsRowDto() {
                LauncherItemId = data.LauncherItemId,
                Code = data.Code,
                Name = data.Name,
                Kind = kindEnumTransfer.To(data.Kind),
                Command = data.Command.Command,
                Option = data.Command.Option,
                WorkDirectory = data.Command.WorkDirectoryPath,
                IconPath = data.Icon.Path,
                IconIndex = data.Icon.Index,
                IsEnabledCommandLauncher = data.IsEnabledCommandLauncher,
                IsEnabledCustomEnvVar = data.IsEnabledCustomEnvVar,
                IsEnabledStandardOutput = data.StandardStream.IsEnabledStandardOutput,
                IsEnabledStandardInput = data.StandardStream.IsEnabledStandardInput,
                Permission = permissionEnumTransfer.To( data.Permission),
                CredentId = Guid.Empty,
                Note = data.Note,
            };

            var status = DatabaseCommonStatus.CreateUser();
            status.WriteCommon(dto);

            return dto;
        }

        LauncherItemData ConvertFromDto(LauncherItemsRowDto dto)
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
            data.Command.Command = dto.Command;
            data.Command.Option = dto.Option;
            data.Command.WorkDirectoryPath = dto.WorkDirectory;

            data.Icon.Path = dto.IconPath;
            data.Icon.Index = (int)Math.Min(0, Math.Max(dto.IconIndex, int.MaxValue));

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

        public void InsertSimpleNew(LauncherItemSimpleNewData data)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data);
            Commander.Execute(sql, dto);
        }

        #endregion
    }
}
