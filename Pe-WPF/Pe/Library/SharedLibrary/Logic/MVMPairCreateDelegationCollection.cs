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
		public MVMPairCreateDelegationCollection(ObservableCollection<TModel> modelList, ObservableCollection<TViewModel> viewModelList, Func<TModel,object,TViewModel> creator)
			:base(modelList,viewModelList)
		{
			ViewModelCreator = creator;
		}

		public MVMPairCreateDelegationCollection(ObservableCollection<TModel> modelList, IEnumerable<object> dataList, Func<TModel, object, TViewModel> creator)
			: base(modelList, dataList)
		{
			ViewModelCreator = creator;
		}

		public MVMPairCreateDelegationCollection(ObservableCollection<TModel> modelList, object data, Func<TModel, object, TViewModel> creator)
			: base(modelList, data)
		{
			ViewModelCreator = creator;
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
