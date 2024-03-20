using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToolbarsSettingEditorViewModel: SettingEditorViewModelBase<LauncherToolbarsSettingEditorElement>
    {
        #region variable

        private LauncherToolbarSettingEditorViewModel? _selectedToolbar;

        #endregion

        public LauncherToolbarsSettingEditorViewModel(LauncherToolbarsSettingEditorElement model, ModelViewModelObservableCollectionManager<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> allLauncherGroupCollection, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AllLauncherGroupCollection = allLauncherGroupCollection;
            AllLauncherGroupItems = AllLauncherGroupCollection.CreateView();
            GeneralTheme = generalTheme;
            ToolbarCollection = new ModelViewModelObservableCollectionManager<LauncherToolbarSettingEditorElement, LauncherToolbarSettingEditorViewModel>(Model.Toolbars, new ModelViewModelObservableCollectionOptions<LauncherToolbarSettingEditorElement, LauncherToolbarSettingEditorViewModel>() {
                ToViewModel = m => new LauncherToolbarSettingEditorViewModel(m, AllLauncherGroupCollection, () => IsSelected, GeneralTheme, DispatcherWrapper, LoggerFactory),
            });
            ToolbarItems = ToolbarCollection.GetDefaultView();
        }

        #region property
        private IGeneralTheme GeneralTheme { get; }
        public RequestSender ShowAllScreensRequest { get; } = new RequestSender();

        private ModelViewModelObservableCollectionManager<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> AllLauncherGroupCollection { get; }
        public ICollectionView AllLauncherGroupItems { get; }

        private ModelViewModelObservableCollectionManager<LauncherToolbarSettingEditorElement, LauncherToolbarSettingEditorViewModel> ToolbarCollection { get; }
        public ICollectionView ToolbarItems { get; }

        public LauncherToolbarSettingEditorViewModel? SelectedToolbar
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

        public override string Header => Properties.Resources.String_Setting_LauncherToolbars_Header;

        public override void Flush()
        {
        }

        public override void Load()
        {
            base.Load();
            SelectedToolbar = ToolbarCollection.ViewModels.First();
        }

        public override void Refresh()
        {
            SelectedToolbar?.Refresh();
        }

        #endregion
    }
}
