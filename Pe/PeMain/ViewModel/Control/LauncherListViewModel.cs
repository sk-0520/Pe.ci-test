/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
    public class LauncherListViewModel: SingleModelWrapperViewModelBase<LauncherItemCollectionModel>, IHasAppNonProcess, IHasAppSender
    {
        #region variable

        string _filterText;
        string _filterText_impl;

        #endregion

        public LauncherListViewModel(LauncherItemCollectionModel model, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model)
        {
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            LauncherItemPairList = new MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel>(
                Model,
                default(object),
                CreateItemViewModel
            );

            Items = CollectionViewSource.GetDefaultView(LauncherItemPairList.ViewModelList);
            Items.Filter = FilterAction;
            Items.Refresh();
        }


        #region property

        internal MVMPairCreateDelegationCollection<LauncherItemModel, LauncherListItemViewModel> LauncherItemPairList { get; private set; }

        public string FilterText
        {
            get { return this._filterText; }
            set
            {
                SetVariableValue(ref this._filterText, value);
                this._filterText_impl = this._filterText;
                Items.Refresh();
                if(Items.IsEmpty && !string.IsNullOrWhiteSpace(this._filterText)) {
                    this._filterText_impl = string.Empty;
                    Items.Refresh();
                }
            }
        }

        public ICollectionView Items { get; private set; }

        #endregion

        #region function

        LauncherListItemViewModel CreateItemViewModel(LauncherItemModel model, object data)
        {
            return new LauncherListItemViewModel(model, AppNonProcess, AppSender);
        }

        bool FilterAction(object o)
        {
            var s = this._filterText_impl ?? string.Empty;
            var vm = (LauncherListItemViewModel)o;
            return LauncherItemUtility.FilterItemName(vm.Model, s);
        }

        #endregion

        #region IHavingClipboardWatcher

        public IClipboardWatcher ClipboardWatcher { get; set; }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion
    }
}
