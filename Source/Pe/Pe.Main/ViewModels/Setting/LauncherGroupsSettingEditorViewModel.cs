using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupsSettingEditorViewModel : SettingEditorViewModelBase<LauncherGroupsSettingEditorElement>
    {
        #region variable

        LauncherGroupSettingEditorViewModel? _selectedGroup;
        #endregion

        public LauncherGroupsSettingEditorViewModel(LauncherGroupsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupSettingEditorViewModel>(Model.GroupItems, LoggerFactory) {
                ToViewModel = m => new LauncherGroupSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory)
            };
            GroupItems = GroupCollection.GetCollectionView();
        }

        #region property

        ModelViewModelObservableCollectionManagerBase<LauncherGroupElement, LauncherGroupSettingEditorViewModel> GroupCollection { get; }
        public ICollectionView GroupItems { get; }

        public LauncherGroupSettingEditorViewModel? SelectedGroup
        {
            get => this._selectedGroup;
            set
            {
                var prev = this._selectedGroup;
                if(prev != null && !prev.IsDisposed) {
                    if(prev.Validate()) {
                    }
                }
                SetProperty(ref this._selectedGroup, value);
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_LauncherGroups;

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    GroupCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
