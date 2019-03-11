using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity
{
    public class ToolbarsDao : ApplicationDatabaseObjectBase
    {
        public ToolbarsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        public IEnumerable<ToolbarsScreenRowDto> SelectAllToolbars()
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            return Commander.Query<ToolbarsScreenRowDto>(sql);
        }

        public bool InsertNewToolbar(Guid toolbarId, Screen screen)
        {
            var sql = StatementLoader.LoadStatementByCurrent();
            var dto = new ToolbarsScreenRowDto() {
                ToolbarId = toolbarId,
                Screen = screen.DeviceName,
                X = (long)screen.DeviceBounds.X,
                Y = (long)screen.DeviceBounds.Y,
                Width = (long)screen.DeviceBounds.Width,
                Height = (long)screen.DeviceBounds.Height,
            };

            var status = DatabaseCommonStatus.CreateUser();
            status.WriteCommon(dto);

            return Commander.Execute(sql, dto) == 1;
        }

        #endregion
    }
}
