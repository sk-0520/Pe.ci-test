using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
    public class LauncherItemIconsEntityDao : EntityDaoBase
    {
        #region define

        private class LauncherItemIconsDto: CreateDtoBase
        {
            #region property

            public Guid LauncherItemId { get; set; }
            public string IconBox { get; set; } = string.Empty;
            public double IconScale { get; set; }
            public long Sequence { get; set; }
            public byte[]? Image { get; set; }

            #endregion

        }

        #endregion

        public LauncherItemIconsEntityDao(IDatabaseContext context, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
            : base(context, statementLoader, implementation, loggerFactory)
        { }

        #region property

        public static class Column
        {
            #region property


            #endregion
        }

        #endregion

        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null")]
        public IReadOnlyList<byte[]>? SelectImageBinary(Guid launcherItemId, IconScale iconScale)
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

        public int InsertImageBinary(Guid launcherItemId, in IconScale iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            var resultCount = 0;
            for(var i = 0; i < binaryImageItems.Length; i++) {
                commonStatus.WriteCreate(dto);
                dto.Sequence = i;
                dto.Image = binaryImageItems[i].ToArray();
                resultCount += Context.Execute(statement, dto);
            }

            return resultCount;
        }

        public int DeleteAllSizeImageBinary(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            return Context.Execute(statement, param);
        }

        public int DeleteImageBinary(Guid launcherItemId, in IconScale iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconScale.Box),
                IconScale = iconScale.Dpi.X,
            };
            return Context.Execute(statement, param);
        }

        #endregion
    }
}
