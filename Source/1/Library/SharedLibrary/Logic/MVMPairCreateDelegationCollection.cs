/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    public class MVMPairCreateDelegationCollection<TModel, TViewModel>: MVMPairCollectionBase<TModel, TViewModel>
        where TModel : ModelBase
        where TViewModel : ViewModelBase
    {
        public MVMPairCreateDelegationCollection(CollectionModel<TModel> modelList, CollectionModel<TViewModel> viewModelList, Func<TModel, object, TViewModel> creator)
            : base()
        {
            ViewModelCreator = creator;

            ModelList = modelList;
            ViewModelList = viewModelList;
        }

        public MVMPairCreateDelegationCollection(CollectionModel<TModel> modelList, IEnumerable<object> dataList, Func<TModel, object, TViewModel> creator)
            : base()
        {
            if(dataList == null) {
                throw new ArgumentNullException(nameof(dataList));
            }

            ViewModelCreator = creator;

            ModelList = modelList;
            ViewModelList = new CollectionModel<TViewModel>(CreateViewModelList(modelList, dataList));
        }

        public MVMPairCreateDelegationCollection(CollectionModel<TModel> modelList, object data, Func<TModel, object, TViewModel> creator)
            : base()
        {
            ViewModelCreator = creator;

            ModelList = modelList;
            ViewModelList = new CollectionModel<TViewModel>(CreateViewModelList(modelList, Enumerable.Repeat(data, ModelList.Count)));
        }

        #region property

        Func<TModel, object, TViewModel> ViewModelCreator { get; set; }

        #endregion

        #region MVMPairCollectionBase

        public override TViewModel CreateViewModel(TModel model, object data)
        {
            return ViewModelCreator(model, data);
        }

        #endregion
    }
}
