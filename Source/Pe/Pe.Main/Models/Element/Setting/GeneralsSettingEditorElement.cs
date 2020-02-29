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
    public class GeneralsSettingEditorElement : SettingEditorElementBase
    {
        public GeneralsSettingEditorElement(EnvironmentParameters environmentParameters, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(clipboardManager, mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, idFactory, dispatcherWrapper, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorElement(environmentParameters, MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorElement(MainDatabaseBarrier, FileDatabaseBarrier, StatementLoader, LoggerFactory);

            Editors = new List<GeneralSettingEditorElementBase>() {
                AppExecuteSettingEditor,
                AppGeneralSettingEditor,
                AppUpdateSettingEditor,
                AppCommandSettingEditor,
                AppNoteSettingEditor,
                AppStandardInputOutputSettingEditor,
            };
        }

        #region property

        public AppExecuteSettingEditorElement AppExecuteSettingEditor { get; }
        public AppGeneralSettingEditorElement AppGeneralSettingEditor { get; }
        public AppUpdateSettingEditorElement AppUpdateSettingEditor { get; }
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

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            foreach(var editor in Editors) {
                editor.Save(commandPack);
            }
        }

        #endregion
    }
}
