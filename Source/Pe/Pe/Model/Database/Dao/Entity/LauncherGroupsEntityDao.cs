using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherGroupsEntityDao : EntityDaoBase
    {
        public LauncherGroupsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        LauncherGroupsRowDto ConvertFromData(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var imgNameEnumTransfer = new EnumTransfer<LauncherGroupImageName>();
            var launcherGroupKindTransfer = new EnumTransfer<LauncherGroupKind>();

            var dto = new LauncherGroupsRowDto() {
                LauncherGroupId = data.LauncherGroupId,
                Kind = launcherGroupKindTransfer.ToText(data.Kind),
                Name = data.Name,
                ImageName = imgNameEnumTransfer.ToText(data.ImageName),
                ImageColor = FromColor(data.ImageColor),
                Sort = data.Sort,
            };

            commonStatus.WriteCommon(dto);

            return dto;

        }

        LauncherGroupData ConvertFromDto(LauncherGroupsRowDto dto)
        {
            var imgNameEnumTransfer = new EnumTransfer<LauncherGroupImageName>();
            var launcherGroupKindTransfer = new EnumTransfer<LauncherGroupKind>();

            var data = new LauncherGroupData() {
                LauncherGroupId = dto.LauncherGroupId,
                Name = dto.Name,
                Kind = launcherGroupKindTransfer.ToEnum(dto.Kind),
                ImageName = imgNameEnumTransfer.ToEnum(dto.ImageName),
                ImageColor = ToColor(dto.ImageColor),
                Sort = dto.Sort,
            };

            return data;
        }

        public long SelectMaxSort()
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            return Commander.QuerySingle<long>(statement);
        }

        public IEnumerable<Guid> SelectAllLauncherGroupIds()
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<Guid>(statement);
        }

        public LauncherGroupData SelectLauncherGroup(Guid launcherGroupId)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            var dto = Commander.QuerySingle<LauncherGroupsRowDto>(statement, param);
            var data = ConvertFromDto(dto);

            return data;
        }

        public void InsertNewGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(statement, dto);
        }

        #endregion
    }
}
