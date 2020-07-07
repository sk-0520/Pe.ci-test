using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public interface ILauncherItemSettingEditor : ILauncherItemId, INotifyPropertyChanged
    {
        #region property
        string Name { get; }
        string Code { get; }
        LauncherItemKind Kind { get; }
        string Comment { get; }

        LauncherIconElement Icon { get; }

        #endregion
    }

    public class LauncherItemSettingEditorElement : LauncherItemCustomizeEditorElement, ILauncherItemSettingEditor
    {
        public LauncherItemSettingEditorElement(Guid launcherItemId, LauncherIconElement iconElement, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            if(!iconElement.IsInitialized) {
                throw new ArgumentException(nameof(iconElement));
            }

            Icon = iconElement;
        }

        #region property
        #endregion

        #region function

        #endregion

        #region LauncherItemCustomizeEditorElement

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Icon.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemSettingEditor

        public LauncherIconElement Icon { get; }

        #endregion
    }
}
