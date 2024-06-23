using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemIconStatusEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherItemIconLastUpdatedStatusDto: DtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double IconScale { get; set; } = 1;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed")]
            public DateTime LastUpdatedTimestamp { get; set; }

            #endregion
        }

        private static class Column
        {
            #region property

            public static string LauncherItemId => "LauncherItemId";
            public static string IconBox => "IconBox";
            public static string IconScale => "IconScale";
            public static string LastUpdatedTimestamp => "LastUpdatedTimestamp";

            #endregion
        }

        #endregion

        public LauncherItemIconStatusEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private LauncherIconStatus ConvertFromDto(LauncherItemIconLastUpdatedStatusDto dto)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();
            return new LauncherIconStatus(
                iconBoxTransfer.ToEnum(dto.IconBox),
                new Point(dto.IconScale, dto.IconScale),
                dto.LastUpdatedTimestamp
            );
        }

        public bool SelectExistsLauncherItemIconState(LauncherItemId launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            return Context.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public IEnumerable<LauncherIconStatus> SelectLauncherItemIconAllSizeStatus(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<LauncherItemIconLastUpdatedStatusDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public LauncherIconStatus? SelectLauncherItemIconSingleSizeStatus(LauncherItemId launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            var dto = Context.QueryFirstOrDefault<LauncherItemIconLastUpdatedStatusDto>(statement, parameter);
            if(dto == null) {
                return null;
            }
            return ConvertFromDto(dto);
        }

        public void InsertLastUpdatedIconTimestamp(LauncherItemId launcherItemId, in IconScale iconScale, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconScale.Box);
            parameter[Column.IconScale] = iconScale.Dpi.X;
            parameter[Column.LastUpdatedTimestamp] = timestamp;

            Context.InsertSingle(statement, parameter);
        }

        public void UpdateLastUpdatedIconTimestamp(LauncherItemId launcherItemId, in IconScale iconScale, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconScale.Box);
            parameter[Column.IconScale] = iconScale.Dpi.X;
            parameter[Column.LastUpdatedTimestamp] = timestamp;

            Context.Update(statement, parameter);
        }

        public int DeleteAllSizeLauncherItemIconState(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Context.Delete(statement, parameter);
        }

        #endregion
    }
}
