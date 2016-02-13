/**
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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.IF;

    /// <summary>
    /// 何やってんのかもうわっけわからんけどテンプレートとクリップボードで共有したいのさ。
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TCollectionModel"></typeparam>
    /// <typeparam name="TItemModel"></typeparam>
    /// <typeparam name="TItemViewModel"></typeparam>
    public abstract class HasViewSingleModelWrapperIndexViewModelBase<TModel, TView, TCollectionModel, TItemModel, TItemViewModel>: HasViewSingleModelWrapperViewModelBase<TModel, TView>, IHasAppNonProcess, IHasAppSender
        where TModel : IModel
        where TView : UIElement
        where TCollectionModel : IndexItemCollectionModel<TItemModel>, new()
        where TItemModel : IndexItemModelBase
        where TItemViewModel : SingleModelWrapperViewModelBase<TItemModel>
    {
        #region variable

        string _filterText;
        string _filterText_impl;

        #endregion

        public HasViewSingleModelWrapperIndexViewModelBase(TModel model, TView view, IndexSettingModelBase<TCollectionModel, TItemModel> indexModel, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view)
        {
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            IndexModel = indexModel;

            IndexPairList = new MVMPairCreateDelegationCollection<TItemModel, TItemViewModel>(
                IndexModel.Items,
                default(object),
                CreateIndexViewModel
            );
            Items = CollectionViewSource.GetDefaultView(IndexPairList.ViewModelList);
            Items.Filter = FilterAction;
            Items.Refresh();
        }


        #region property

        protected IndexSettingModelBase<TCollectionModel, TItemModel> IndexModel { get; private set; }

        public MVMPairCreateDelegationCollection<TItemModel, TItemViewModel> IndexPairList { get; private set; }

        public CollectionModel<TItemViewModel> IndexItems { get { return IndexPairList.ViewModelList; } }

        public ICollectionView Items { get; private set; }

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


        #endregion

        #region function

        protected abstract TItemViewModel CreateIndexViewModel(TItemModel model, object data);

        bool FilterAction(object o)
        {
            var s = this._filterText_impl ?? string.Empty;
            var vm = (TItemViewModel)o;
            return FilterAction_Impl(s, vm);
        }

        protected virtual bool FilterAction_Impl(string filterText, TItemViewModel viewModelItem)
        {
            return viewModelItem.Model.Name.StartsWith(filterText, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion
    }
}
