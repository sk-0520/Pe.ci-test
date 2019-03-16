using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class LauncherItemIconsDao : ApplicationDatabaseObjectBase
    {
        public LauncherItemIconsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        public byte[] SelectImageBinary(Guid launcherItemId, IconScale iconScale)
        {
            var iconScaleTransfer = new EnumTransfer<IconScale>();

            var sql = StatementLoader.LoadStatementByCurrent();
            var param = new {
                LauncherItemId = launcherItemId,
                IconScale = iconScaleTransfer.To(iconScale),
            };
            return Commander.QueryFirstOrDefault<byte[]>(sql, param);
        }

        #endregion
    }
}
