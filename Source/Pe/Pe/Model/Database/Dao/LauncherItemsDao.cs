using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao
{
    public class LauncherItemSimpleNewData : DataBase
    {
        #region property

        public Guid LauncherItemId { get; set; }

        public string Name { get; set; }

        public LauncherItemKind Kind { get; set; }

        public LauncherCommandData Command { get; set; }

        public IconData Icon { get; set; }

        public bool IsEnabledCommandLauncher { get; set; }
        public bool IsEnabledCustomEnvVar { get; set; }

        public StandardStreamData StandardStream { get; set; }

        public LauncherItemPermission Permission { get; set; }

        public Guid CredentId { get; set; }

        public string Note { get; set; }

        #endregion
    }

    public class LauncherItemsDao : ApplicationDatabaseObjectBase
    {
        public LauncherItemsDao(IDatabaseCommander commander, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(commander, statementLoader, loggerFactory)
        { }

        #region function

        void InsertSimpleNew(LauncherItemSimpleNewData data)
        {

        }

        #endregion
    }
}
