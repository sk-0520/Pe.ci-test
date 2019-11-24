using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public sealed class CommonLauncherItemElement : ElementBase, ILauncherItemId
    {
        public CommonLauncherItemElement(Guid launcherItemId, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public LauncherItemKind Kind { get; private set; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var item = launcherItemsEntityDao.SelectLauncherItem(LauncherItemId);
                Code = item.Code;
                Name = item.Name;
                Kind = item.Kind;
            }
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion
    }

}
