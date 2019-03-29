using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
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

        public IReadOnlyList<byte[]> SelectImageBinary(Guid launcherItemId, IconScale iconScale)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToText(iconScale),
            };
            var rows = Commander.Query<byte[]>(sql, param);
            if(rows != null) {
                return rows.ToArray();
            }

            return null;
        }

        public int InsertImageBinary(Guid launcherItemId, IconScale iconScale, IEnumerable<byte> imageBinary, IDatabaseCommonStatus commonStatus)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToText(iconScale),
            };
            var resultCount = 0;
            for(var i = 0; i < binaryImageItems.Length; i++) {
                commonStatus.WriteCreate(dto);
                dto.Sequence = i;
                dto.Image = binaryImageItems[i].ToArray();
                resultCount += Commander.Execute(sql, dto);
            }

            return resultCount;
        }

        public int DeleteImageBinary(Guid launcherItemId, IconScale iconScale)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.ToText(iconScale),
            };
            return Commander.Execute(sql, param);
        }

        #endregion
    }
}
