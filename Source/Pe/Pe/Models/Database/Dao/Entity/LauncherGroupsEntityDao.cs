using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
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
                Kind = launcherGroupKindTransfer.ToString(data.Kind),
                Name = data.Name,
                ImageName = imgNameEnumTransfer.ToString(data.ImageName),
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
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                Kind = launcherGroupKindTransfer.ToEnum(dto.Kind),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                ImageName = imgNameEnumTransfer.ToEnum(dto.ImageName),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                ImageColor = ToColor(dto.ImageColor),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
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
