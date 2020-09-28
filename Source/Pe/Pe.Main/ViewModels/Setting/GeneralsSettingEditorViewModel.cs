using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class GeneralsSettingEditorViewModel: SettingEditorViewModelBase<GeneralsSettingEditorElement>
    {
        public GeneralsSettingEditorViewModel(GeneralsSettingEditorElement model, ApplicationConfiguration applicationConfiguration, IGeneralTheme generalTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AppExecuteSettingEditor = new AppExecuteSettingEditorViewModel(Model.AppExecuteSettingEditor, DispatcherWrapper, LoggerFactory);
            AppGeneralSettingEditor = new AppGeneralSettingEditorViewModel(Model.AppGeneralSettingEditor, applicationConfiguration.General.SupportCultures, imageLoader, DispatcherWrapper, LoggerFactory);
            AppUpdateSettingEditor = new AppUpdateSettingEditorViewModel(Model.AppUpdateSettingEditor, DispatcherWrapper, LoggerFactory);
            AppNotifyLogSettingEditor = new AppNotifyLogSettingEditorViewModel(Model.AppNotifyLogSettingEditor, DispatcherWrapper, LoggerFactory);
            AppLauncherToolbarSettingEditor = new AppLauncherToolbarSettingEditorViewModel(Model.AppLauncherToolbarSettingEditor, DispatcherWrapper, LoggerFactory);
            AppCommandSettingEditor = new AppCommandSettingEditorViewModel(Model.AppCommandSettingEditor, generalTheme, DispatcherWrapper, LoggerFactory);
            AppNoteSettingEditor = new AppNoteSettingEditorViewModel(Model.AppNoteSettingEditor, generalTheme, DispatcherWrapper, LoggerFactory);
            AppStandardInputOutputSettingEditor = new AppStandardInputOutputSettingEditorViewModel(Model.AppStandardInputOutputSettingEditor, generalTheme, DispatcherWrapper, LoggerFactory);

            EditorItems = new ObservableCollection<IGeneralSettingEditor>() {
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

        AppExecuteSettingEditorViewModel AppExecuteSettingEditor { get; }
        AppGeneralSettingEditorViewModel AppGeneralSettingEditor { get; }
        AppUpdateSettingEditorViewModel AppUpdateSettingEditor { get; }
        AppNotifyLogSettingEditorViewModel AppNotifyLogSettingEditor { get; }
        AppLauncherToolbarSettingEditorViewModel AppLauncherToolbarSettingEditor { get; }
        AppCommandSettingEditorViewModel AppCommandSettingEditor { get; }
        AppNoteSettingEditorViewModel AppNoteSettingEditor { get; }
        AppStandardInputOutputSettingEditorViewModel AppStandardInputOutputSettingEditor { get; }

        [IgnoreValidation]
        public ObservableCollection<IGeneralSettingEditor> EditorItems { get; }

        #endregion

        #region function
        #endregion

        #region command
        #endregion

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Generals_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in EditorItems) {
                        editor.Dispose();
                    }
                    EditorItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
