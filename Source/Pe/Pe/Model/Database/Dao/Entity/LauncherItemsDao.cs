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

        public IEnumerable<string> SelectFuzzyCodes(string baseCode)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<string>(sql, new { BaseCode = baseCode });
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
