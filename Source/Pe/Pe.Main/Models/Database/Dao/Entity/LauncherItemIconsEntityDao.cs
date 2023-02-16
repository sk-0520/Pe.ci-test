using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemIconsEntityDao: EntityDaoBase
    {
        #region define

        private class LauncherItemIconsDto: CreateDtoBase
        {
            #region property

            public LauncherItemId LauncherItemId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double IconScale { get; set; }
            public long Sequence { get; set; }
            public byte[]? Image { get; set; }

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
        public IReadOnlyList<byte[]>? SelectImageBinary(LauncherItemId launcherItemId, IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            var rows = Context.Query<byte[]>(statement, param);
            if(rows != null) {
                return rows.ToArray();
            }

            return null;
        }

        public void InsertImageBinary(LauncherItemId launcherItemId, in IconScale iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            
            for(var i = 0; i < binaryImageItems.Length; i++) {
                commonStatus.WriteCreateTo(dto);
                dto.Sequence = i;
                dto.Image = binaryImageItems[i].ToArray();
                Context.InsertSingle(statement, dto);
            }
        }

        public int DeleteAllSizeImageBinary(LauncherItemId launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };

            return Context.Delete(statement, param);
        }

        public int DeleteImageBinary(LauncherItemId launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };

            return Context.Delete(statement, param);
        }

        #endregion
    }
}
