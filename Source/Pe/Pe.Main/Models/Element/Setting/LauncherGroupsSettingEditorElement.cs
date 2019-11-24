using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class LauncherGroupsSettingEditorElement : SettingEditorElementBase
    {
        public LauncherGroupsSettingEditorElement(IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public ObservableCollection<LauncherGroupElement> GroupItems { get; } = new ObservableCollection<LauncherGroupElement>();
        public ObservableCollection<LauncherElementWithIconElement<LauncherItemElement>> LauncherItems { get; } = new ObservableCollection<LauncherElementWithIconElement<LauncherItemElement>>();

        #endregion

        #region function
        #endregion

        #region SettingEditorElementBase

        public override void Load()
        {
        }

        public override void Save()
        {
        }

        #endregion
    }
}
