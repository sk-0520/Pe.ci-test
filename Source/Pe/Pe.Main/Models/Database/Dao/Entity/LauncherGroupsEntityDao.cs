using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherGroupsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherGroupsRowDto: RowDtoBase
        {
            #region property

            public LauncherGroupId LauncherGroupId { get; set; }
            public string Kind { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string ImageName { get; set; } = string.Empty;
            public string ImageColor { get; set; } = string.Empty;
            public long Sequence { get; set; }

            #endregion
        }

        private static class Column
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

        public LauncherGroupsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherGroupsRowDto ConvertFromData(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
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

            commonStatus.WriteCommonTo(dto);

            return dto;
        }

        private LauncherGroupData ConvertFromDto(LauncherGroupsRowDto dto)
        {
            var imgNameEnumTransfer = new EnumTransfer<LauncherGroupImageName>();
            var launcherGroupKindTransfer = new EnumTransfer<LauncherGroupKind>();

            var data = new LauncherGroupData() {
                LauncherGroupId = dto.LauncherGroupId,
                Name = dto.Name,
                Kind = launcherGroupKindTransfer.ToEnum(dto.Kind),
                ImageName = imgNameEnumTransfer.ToEnum(dto.ImageName),
                ImageColor = ToColor(dto.ImageColor),
                Sequence = dto.Sequence,
            };

            return data;
        }

        public long SelectMaxSequence()
        {
            var statement = LoadStatement();
            return Context.QuerySingle<long>(statement);
        }

        public IEnumerable<LauncherGroupId> SelectAllLauncherGroupIds()
        {
            var statement = LoadStatement();
            return Context.Query<LauncherGroupId>(statement);
        }

        public IEnumerable<string> SelectAllLauncherGroupNames()
        {
            var statement = LoadStatement();
            return Context.Query<string>(statement);
        }


        public LauncherGroupData SelectLauncherGroup(LauncherGroupId launcherGroupId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherGroupId = launcherGroupId,
            };
            var dto = Context.QuerySingle<LauncherGroupsRowDto>(statement, param);
            var data = ConvertFromDto(dto);

            return data;
        }

        public void InsertNewGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);

            Context.InsertSingle(statement, dto);
        }

        public void UpdateGroup(LauncherGroupData data, IDatabaseCommonStatus commonStatus)
        {
            var statement = LoadStatement();
            var dto = ConvertFromData(data, commonStatus);

            Context.UpdateByKey(statement, dto);
        }

        public void DeleteGroup(LauncherGroupId launcherGroupId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherGroupId = launcherGroupId,
            };

            Context.DeleteByKey(statement, parameter);
        }

        #endregion
    }
}
