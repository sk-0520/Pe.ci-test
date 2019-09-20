using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
{
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

        public IReadOnlyList<byte[]>? SelectImageBinary(Guid launcherItemId, IconBox iconScale)
        {
            var iconScaleTransfer = new EnumTransfer<IconBox>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToString(iconScale),
            };
            var rows = Commander.Query<byte[]>(statement, param);
            if(rows != null) {
                return rows.ToArray();
            }

            return null;
        }

        public int InsertImageBinary(Guid launcherItemId, IconBox iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconScaleTransfer = new EnumTransfer<IconBox>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToString(iconScale),
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

        public int DeleteImageBinary(Guid launcherItemId, IconBox iconSize)
        {
            var iconScaleTransfer = new EnumTransfer<IconBox>();

            var statement = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToString(iconSize),
            };
            return Commander.Execute(statement, param);
        }

        #endregion
    }
}
