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
    public class LauncherGroupsDao : EntityDaoBase
    {
        public LauncherGroupsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
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
                Kind = launcherGroupKindTransfer.To(data.Kind),
                Name = data.Name,
                ImageName = imgNameEnumTransfer.To(data.ImageName),
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
                Kind = launcherGroupKindTransfer.From(dto.Kind),
                ImageName = imgNameEnumTransfer.From(dto.ImageName),
                ImageColor = ToColor(dto.ImageColor),
                Sort = dto.Sort,
            };

            return data;
        }

        public long SelectMaxSort()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.QuerySingle<long>(sql);
        }

        public IEnumerable<Guid> SelectAllLauncherGroupIds()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<Guid>(sql);
        }

        public LauncherGroupData SelectLauncherGroup(Guid launcherGroupId)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            var dto = Commander.QuerySingle<LauncherGroupsRowDto>(sql, param);
            var data = ConvertFromDto(dto);

            return data;
        }

        public void InsertNewGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(sql, dto);
        }

        #endregion
    }
}
