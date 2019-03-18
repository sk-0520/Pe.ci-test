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
    public class LauncherItemIconsDao : ApplicationDatabaseObjectBase
    {
        public LauncherItemIconsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        public IReadOnlyList<byte[]> SelectImageBinary(Guid launcherItemId, IconScale iconScale)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.To(iconScale),
            };
            var rows = Commander.Query<byte[]>(sql, param);
            if(rows != null) {
                return rows.ToArray();
            }

            return null;
        }

        public int InsertImageBinary(Guid launcherItemId, IconScale iconScale, IEnumerable<byte> imageBinary)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var binaryImageItems = imageBinary.GroupSplit(80 * 1024).ToArray();
            var status = DatabaseCommonStatus.CreateUser();
            var dto = new LauncherItemIconsDto() {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.To(iconScale),
            };
            var resultCount = 0;
            for(var i = 0; i < binaryImageItems.Length; i++) {
                status.WriteCreate(dto);
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
                IconScale = iconScaleTransfer.To(iconScale),
            };
            return Commander.Execute(sql, param);
        }

        #endregion
    }
}
