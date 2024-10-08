using System;
using System.ComponentModel;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public interface ILauncherItemSettingEditor: ILauncherItemId, INotifyPropertyChanged
    {
        #region property
        string Name { get; }
        LauncherItemKind Kind { get; }
        string Comment { get; }

        #endregion
    }

    public class LauncherItemSettingEditorElement: LauncherItemCustomizeEditorElement, ILauncherItemSettingEditor
    {
        public LauncherItemSettingEditorElement(LauncherItemId launcherItemId, ILauncherItemAddonFinder launcherItemAddonFinder, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(launcherItemId, launcherItemAddonFinder, launcherItemAddonContextFactory, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, loggerFactory)
        { }

        public LauncherItemSettingEditorElement(LauncherItemId launcherItemId, ILauncherItemAddonFinder launcherItemAddonFinder, LauncherSettingCommonData setting, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : this(launcherItemId, launcherItemAddonFinder, launcherItemAddonContextFactory, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, loggerFactory)
        {
            IsLazyLoad = true;

            Name = setting.Name;
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
            switch(Kind) {
                case LauncherItemKind.File:
                    LoadFile();
                    break;

                case LauncherItemKind.Addon:
                    LoadAddon();
                    break;

                case LauncherItemKind.Separator:
                    LoadSeparator();
                    break;

                default:
                    throw new NotImplementedException();
            }

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
