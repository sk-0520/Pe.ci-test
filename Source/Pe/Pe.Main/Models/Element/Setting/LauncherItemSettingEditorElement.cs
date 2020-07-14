using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public interface ILauncherItemSettingEditor: ILauncherItemId, INotifyPropertyChanged
    {
        #region property
        string Name { get; }
        string Code { get; }
        LauncherItemKind Kind { get; }
        string Comment { get; }

        #endregion
    }

    public class LauncherItemSettingEditorElement: LauncherItemCustomizeEditorElement, ILauncherItemSettingEditor
    {
        public LauncherItemSettingEditorElement(Guid launcherItemId, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {}

        public LauncherItemSettingEditorElement(Guid launcherItemId, LauncherSettingCommonData setting, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : this(launcherItemId, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            IsLazyLoad = true;

            Name = setting.Name;
            Code = setting.Code;
            Kind = setting.Kind;
            IconData = setting.Icon;
            IsEnabledCommandLauncher = setting.IsEnabledCommandLauncher;
            Comment = setting.Comment;
            TagItems.SetRange(setting.Tags);
        }

        #region property

        #endregion

        #region function

        internal void LazyLoad()
        {
            LoadFile();

            IsLazyLoad = false;
        }

        #endregion

        #region LauncherItemCustomizeEditorElement

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemSettingEditor


        #endregion
    }
}
