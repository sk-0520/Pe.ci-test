using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToobarsSettingEditorViewModel : SettingEditorViewModelBase<LauncherToobarsSettingEditorElement>
    {
        #region variable

        LauncherToobarSettingEditorViewModel? _selectedToolbar;

        #endregion
        public LauncherToobarsSettingEditorViewModel(LauncherToobarsSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
            ToolbarCollection = new ActionModelViewModelObservableCollectionManager<LauncherToobarSettingEditorElement, LauncherToobarSettingEditorViewModel>(Model.Toolbars) {
                ToViewModel = m => new LauncherToobarSettingEditorViewModel(m, GeneralTheme, DispatcherWrapper, LoggerFactory),
            };
            ToolbarItems = ToolbarCollection.GetDefaultView();
        }

        #region property
        IGeneralTheme GeneralTheme { get; }
        public RequestSender ShowAllScreensRequest { get; } = new RequestSender();

        ModelViewModelObservableCollectionManagerBase<LauncherToobarSettingEditorElement, LauncherToobarSettingEditorViewModel> ToolbarCollection { get; }
        public ICollectionView ToolbarItems { get; }

        public LauncherToobarSettingEditorViewModel? SelectedToolbar
        {
            get => this._selectedToolbar;
            set => SetProperty(ref this._selectedToolbar, value);
        }

        #endregion

        #region function

        #endregion

        #region command

        public ICommand ShowAllScreensCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ShowAllScreensRequest.Send();
            }
        ));


        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_LauncherToolbars;

        public override void Flush()
        {
        }

        public override void Load()
        {
            base.Load();
            SelectedToolbar = ToolbarCollection.ViewModels.First();
        }

        #endregion
    }
}
