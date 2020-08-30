using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class GeneralsSettingEditorElement: SettingEditorElementBase
    {
        public GeneralsSettingEditorElement(EnvironmentParameters environmentParameters, IReadOnlyList<IPlugin> themePlugins, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IIdFactory idFactory, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, temporaryDatabaseBarrier, databaseStatementLoader, idFactory, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorElement(environmentParameters, MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, themePlugins, DatabaseStatementLoader, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppNotifyLogSettingEditor = new AppNotifyLogSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppLauncherToolbarSettingEditor = new AppLauncherToolbarSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, DatabaseStatementLoader, LoggerFactory);

            Editors = new List<GeneralSettingEditorElementBase>() {
                AppExecuteSettingEditor,
                AppGeneralSettingEditor,
                AppUpdateSettingEditor,
                AppNotifyLogSettingEditor,
                AppLauncherToolbarSettingEditor,
                AppCommandSettingEditor,
                AppNoteSettingEditor,
                AppStandardInputOutputSettingEditor,
            };
        }

        #region property

        public AppExecuteSettingEditorElement AppExecuteSettingEditor { get; }
        public AppGeneralSettingEditorElement AppGeneralSettingEditor { get; }
        public AppUpdateSettingEditorElement AppUpdateSettingEditor { get; }
        public AppNotifyLogSettingEditorElement AppNotifyLogSettingEditor { get; }
        public AppLauncherToolbarSettingEditorElement AppLauncherToolbarSettingEditor { get; }
        public AppCommandSettingEditorElement AppCommandSettingEditor { get; }
        public AppNoteSettingEditorElement AppNoteSettingEditor { get; }
        public AppStandardInputOutputSettingEditorElement AppStandardInputOutputSettingEditor { get; }

        IList<GeneralSettingEditorElementBase> Editors { get; }

        #endregion

        #region function

        #endregion

        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            foreach(var editor in Editors) {
                editor.Initialize();
            }
        }

        protected override void SaveImpl(IDatabaseCommandsPack commandPack)
        {
            foreach(var editor in Editors) {
                editor.Save(commandPack);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in Editors) {
                        editor.Dispose();
                    }
                    Editors.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
