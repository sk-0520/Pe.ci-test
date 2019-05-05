using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public class LauncherFileItemLoader : LauncherItemLoaderBase
    {
        public LauncherFileItemLoader(Guid launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, mainDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region LauncherItemBase
        #endregion
    }
}
