namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	public class MVMPairCreateDelegationCollection<TModel, TViewModel> : MVMPairCollectionBase<TModel, TViewModel>
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
