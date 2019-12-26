using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class GeneralsSettingEditorViewModel : SettingEditorViewModelBase<GeneralsSettingEditorElement>
    {
        public GeneralsSettingEditorViewModel(GeneralsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorViewModel(Model.AppExecuteSettingEditor, DispatcherWrapper, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorViewModel(Model.AppGeneralSettingEditor, DispatcherWrapper, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorViewModel(Model.AppUpdateSettingEditor, DispatcherWrapper, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorViewModel(Model.AppCommandSettingEditor, DispatcherWrapper, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorViewModel(Model.AppNoteSettingEditor, DispatcherWrapper, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorViewModel(Model.AppStandardInputOutputSettingEditor, DispatcherWrapper, LoggerFactory);
            AppWindowSettingEditor = new AppWindowSettingEditorViewModel(Model.AppWindowSettingEditor, DispatcherWrapper, LoggerFactory);

            EditorItems = new ObservableCollection<IGeneralSettingEditor>() {
                AppExecuteSettingEditor,
                AppGeneralSettingEditor,
                AppUpdateSettingEditor,
                AppCommandSettingEditor,
                AppNoteSettingEditor,
                AppStandardInputOutputSettingEditor,
                AppWindowSettingEditor,
            };
        }

        #region property

        AppExecuteSettingEditorViewModel AppExecuteSettingEditor { get; }
        AppGeneralSettingEditorViewModel AppGeneralSettingEditor { get; }
        AppUpdateSettingEditorViewModel AppUpdateSettingEditor { get; }
        AppCommandSettingEditorViewModel AppCommandSettingEditor { get; }
        AppNoteSettingEditorViewModel AppNoteSettingEditor { get; }
        AppStandardInputOutputSettingEditorViewModel AppStandardInputOutputSettingEditor { get; }
        AppWindowSettingEditorViewModel AppWindowSettingEditor { get; }

        public ObservableCollection<IGeneralSettingEditor> EditorItems { get; }

        #endregion

        #region function
        #endregion

        #region command
        #endregion

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Header_General;

        public override void Flush()
        {
        }

        #endregion
    }
}
