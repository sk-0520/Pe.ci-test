using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemIconStatusEntityDao: EntityDaoBase
    {
        #region define

        class LauncherItemIconLastUpdatedStatusDto: DtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double IconScale { get; set; } = 1;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3459:Unassigned members should be removed")]
            public DateTime LastUpdatedTimestamp { get; set; }

            #endregion
        }

        #endregion


        public LauncherItemIconStatusEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property

            public static string LauncherItemId => "LauncherItemId";
            public static string IconBox => "IconBox";
            public static string IconScale => "IconScale";
            public static string LastUpdatedTimestamp => "LastUpdatedTimestamp";

            #endregion
        }

        #endregion

        #region function

        LauncherIconStatus ConvertFromDto(LauncherItemIconLastUpdatedStatusDto dto)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();
            return new LauncherIconStatus(
                iconBoxTransfer.ToEnum(dto.IconBox),
                new Point(dto.IconScale, dto.IconScale),
                dto.LastUpdatedTimestamp
            );
        }

        public bool SelecteExistLauncherItemIconState(Guid launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            return Commander.QueryFirstOrDefault<bool>(statement, parameter);
        }

        public IEnumerable<LauncherIconStatus> SelectLauncherItemIconAllSizeStatus(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Query<LauncherItemIconLastUpdatedStatusDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }

        public LauncherIconStatus? SelectLauncherItemIconSingleSizeStatus(Guid launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            var dto = Commander.QueryFirstOrDefault<LauncherItemIconLastUpdatedStatusDto>(statement, parameter);
            if(dto == null) {
                return null;
            }
            return ConvertFromDto(dto);
        }

        public bool InsertLastUpdatedIconTimestamp(Guid launcherItemId, in IconScale iconScale, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconScale.Box);
            parameter[Column.IconScale] = iconScale.Dpi.X;
            parameter[Column.LastUpdatedTimestamp] = timestamp;
            return Commander.Execute(statement, parameter) == 1;
        }

        public bool UpdateLastUpdatedIconTimestamp(Guid launcherItemId, in IconScale iconScale, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var parameter = commonStatus.CreateCommonDtoMapping();
            parameter[Column.LauncherItemId] = launcherItemId;
            parameter[Column.IconBox] = iconBoxTransfer.ToString(iconScale.Box);
            parameter[Column.IconScale] = iconScale.Dpi.X;
            parameter[Column.LastUpdatedTimestamp] = timestamp;
            return Commander.Execute(statement, parameter) == 1;
        }

        public int DeleteAllSizeLauncherItemIconState(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };

            return Commander.Execute(statement, parameter);
        }

        #endregion
    }
}
