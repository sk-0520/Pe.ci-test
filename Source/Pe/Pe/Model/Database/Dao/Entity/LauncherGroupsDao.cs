using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherGroupsDao : ApplicationDatabaseObjectBase
    {
        public LauncherGroupsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        LauncherGroupsRowDto ConvertFromData(LauncherGroupData data)
        {
            var imgNameEnumTransfer = new EnumTransfer<LauncherGroupImageName>();

            var dto = new LauncherGroupsRowDto() {
                LauncherGroupId = data.LauncherGroupId,
                Name = data.Name,
                ImageName = imgNameEnumTransfer.To(data.ImageName),
                ImageColor = data.ImageColor.ToString(),
            };

            var status = DatabaseCommonStatus.CreateUser();
            status.WriteCommon(dto);

            return dto;

        }

        public void InsertNewGroup(LauncherGroupData data)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data);
            Commander.Execute(sql, dto);
        }

        #endregion
    }
}
