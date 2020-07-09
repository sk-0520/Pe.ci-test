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
    internal class LauncherItemIconsDto : CreateDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string IconBox { get; set; } = string.Empty;
        public double IconScale { get; set; }
        public long Sequence { get; set; }
        public byte[]? Image { get; set; }

        #endregion

    }

    public class LauncherItemIconsEntityDao : EntityDaoBase
    {
        public LauncherItemIconsEntityDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, IDatabaseImplementation implementation, ILoggerFactory loggerFactory)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null")]
        public IReadOnlyList<byte[]>? SelectImageBinary(Guid launcherItemId, IconBox iconBox, Point iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox),
                IconScale = iconScale.X,
            };
            var rows = Commander.Query<byte[]>(statement, param);
            if(rows != null) {
                return rows.ToArray();
            }

            return null;
        }

        public int InsertImageBinary(Guid launcherItemId, IconBox iconBox, Point iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox),
                IconScale = iconScale.X,
            };
            var resultCount = 0;
            for(var i = 0; i < binaryImageItems.Length; i++) {
                commonStatus.WriteCreate(dto);
                dto.Sequence = i;
                dto.Image = binaryImageItems[i].ToArray();
                resultCount += Commander.Execute(statement, dto);
            }

            return resultCount;
        }

        public int DeleteAllSizeImageBinary(Guid launcherItemId)
        {
            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
            };
            return Commander.Execute(statement, param);
        }

        public int DeleteImageBinary(Guid launcherItemId, IconBox iconBox, Point iconScale)
        {
            var iconBoxTransfer = new EnumTransfer<IconBox>();

            var statement = LoadStatement();
            var param = new {
                LauncherItemId = launcherItemId,
                IconBox = iconBoxTransfer.ToString(iconBox),
                IconScale = iconScale.X,
            };
            return Commander.Execute(statement, param);
        }

        #endregion
    }
}
