using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
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
            throw new NotImplementedException();
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
