using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemSettingViewModel : SingleModelViewModelBase<LauncherItemSettingElement>
    {
        #region variable

        LauncherItemCustomizeViewModel? _selectedCustomizeItem;
        #endregion

        public LauncherItemSettingViewModel(LauncherItemSettingElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            CustomizeCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemCustomizeElement, LauncherItemCustomizeViewModel>(Model.CustomizeItems, LoggerFactory) {
                ToViewModel = m => new LauncherItemCustomizeViewModel(m, dispatcherWapper, LoggerFactory),
            };
            CustomizeItems = CustomizeCollection.GetCollectionView();
        }

        #region function

        ModelViewModelObservableCollectionManagerBase<LauncherItemCustomizeElement, LauncherItemCustomizeViewModel> CustomizeCollection { get; }
        public ICollectionView CustomizeItems { get; }

        public LauncherItemCustomizeViewModel? SelectedCustomizeItem
        {
            get => this._selectedCustomizeItem;
            set
            {
                SetProperty(ref this._selectedCustomizeItem, value);
            }
        }

        #endregion
    }
}
