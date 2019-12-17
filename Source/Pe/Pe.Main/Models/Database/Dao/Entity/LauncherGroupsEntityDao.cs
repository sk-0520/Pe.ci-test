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
    internal class LauncherGroupsRowDto : RowDtoBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public string Kind { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageColor { get; set; } = string.Empty;
        public long Sequence { get; set; }

        #endregion
    }

    public class LauncherGroupsEntityDao : EntityDaoBase
    {
        public LauncherGroupsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherGroupId { get; } = "LauncherGroupId";
            public static string Name { get; } = "Name";
            public static string Kind { get; } = "Kind";
            public static string ImageName { get; } = "ImageName";
            public static string ImageColor { get; } = "ImageColor";
            public static string Sequence { get; } = "Sequence";

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
                Sequence = data.Sequence,
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
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                ImageColor = ToColor(dto.ImageColor),
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                Sequence = dto.Sequence,
            };

            return data;
        }

        public long SelectMaxSequence()
        {
            var statement = LoadStatement();
            return Commander.QuerySingle<long>(statement);
        }

        public IEnumerable<Guid> SelectAllLauncherGroupIds()
        {
            var statement = LoadStatement();
            return Commander.Query<Guid>(statement);
        }

        public LauncherGroupData SelectLauncherGroup(Guid launcherGroupId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            var dto = Commander.QuerySingle<LauncherGroupsRowDto>(statement, param);
            var data = ConvertFromDto(dto);

            return data;
        }

        public void InsertNewGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            Commander.Execute(statement, dto);
        }

        public bool UpdateGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);
            return Commander.Execute(statement, dto) == 1;
        }

        #endregion
    }
}
