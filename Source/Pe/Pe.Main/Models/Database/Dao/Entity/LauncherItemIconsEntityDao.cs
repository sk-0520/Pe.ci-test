using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using System.Buffers;
using ContentTypeTextNet.Pe.Bridge.Models;
using System.Windows;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemIconsEntityDao: EntityDaoBase
    {
        #region define

        private sealed class LauncherItemIconsDto: CreateDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double IconScale { get; set; }
            public byte[] Image { get; set; } = Array.Empty<byte>();

            [DateTimeKind(DateTimeKind.Utc)]
            public DateTime LastUpdatedTimestamp { get; set; }

            #endregion
        }

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

        #endregion

        public LauncherItemIconsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

        private static LauncherIconStatus ConvertFromDto(LauncherItemIconLastUpdatedStatusDto dto)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();
            return new LauncherIconStatus(
                iconBoxTransfer.ToEnum(dto.IconBox),
                new Point(dto.IconScale, dto.IconScale),
                dto.LastUpdatedTimestamp
            );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null")]
        public byte[] SelectImageBinary(LauncherItemId launcherItemId, IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            var result = Context.QueryFirstOrDefault<byte[]>(statement, param);
            if(result != null) {
                return result;
            }

            return Array.Empty<byte>();
        }

        public LauncherIconStatus? SelectLauncherItemIconKeyStatus(LauncherItemId launcherItemId, in IconScale iconScale)
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

        public IEnumerable<LauncherIconStatus> SelectLauncherItemIconAllStatus(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var parameter = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Query<LauncherItemIconLastUpdatedStatusDto>(statement, parameter)
                .Select(i => ConvertFromDto(i))
            ;
        }


        public void InsertImageBinary(LauncherItemId launcherItemId, in IconScale iconScale, IEnumerable<byte> imageBinary, [DateTimeKind(DateTimeKind.Utc)] DateTime lastUpdatedTimestamp, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
                Image = imageBinary.ToArray(),
                LastUpdatedTimestamp = lastUpdatedTimestamp,
            };
            commonStatus.WriteCreateTo(dto);

            Context.InsertSingle(statement, dto);
        }

        public int DeleteAllSizeImageBinary(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };

            return Context.Delete(statement, param);
        }

        public bool DeleteImageBinary(LauncherItemId launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };

            return Context.DeleteByKeyOrNothing(statement, param);
        }

        #endregion
    }
}
