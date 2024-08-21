using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Standard.Base;
using System.Buffers;

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

            #endregion

        }

        private static class Column
        {
            #region property


            #endregion
        }

        #endregion

        public LauncherItemIconsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region function

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

        public void InsertImageBinary(LauncherItemId launcherItemId, in IconScale iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
                Image = imageBinary.ToArray(),
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
