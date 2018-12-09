using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseSetup: DisposerBase
    {
        public DatabaseSetup(DirectoryInfo baseDirectory, ILogFactory logFactory)
        {
            Logger = logFactory.CreateCurrentClass();
            StatementLoader = new ApplicationDatabaseStatementLoader(baseDirectory, TimeSpan.Zero, Logger.Factory);
        }

        #region property

        IDatabaseStatementLoader StatementLoader { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        public void Initialize()
        {

        }

        public DatabasePack Migrate()
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
