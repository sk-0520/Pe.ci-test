using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherToobarsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherToobarsSettingEditorElement(IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public IList<LauncherToobarSettingEditorElement> Toolbars { get; } = new List<LauncherToobarSettingEditorElement>();

        #endregion

        #region function
        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IReadOnlyList<Guid> launcherToolbarIds;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var ids = launcherToolbarsEntityDao.SelectAllLauncherToolbarIds();
                launcherToolbarIds = ids.ToList();
            }

            Toolbars.Clear();
            foreach(var launcherToolbarId in launcherToolbarIds) {
                var element = new LauncherToobarSettingEditorElement(launcherToolbarId, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
                Toolbars.Add(element);
            }
            foreach(var toolbar in Toolbars) {
                toolbar.Initialize();
            }
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            foreach(var toolbar in Toolbars) {
                toolbar.Save(commandPack);
            }
        }

        #endregion
    }
}
