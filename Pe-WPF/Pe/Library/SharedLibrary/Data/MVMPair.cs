namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	public struct MVMPair<TModel, TViewModel>
		where TModel : ModelBase
		where TViewModel : ViewModelBase
	{
		#region variable

		TModel _model;
		TViewModel _viewModel;

		#endregion

		public MVMPair(TModel model, TViewModel viewModel)
		{
			this._model = model;
			this._viewModel = viewModel;
		}

		#region property

		public TModel Model { get { return this._model; } }
		public TViewModel ViewModel { get { return this._viewModel; } }

		#endregion

		#region object

		public override string ToString()
		{
			return string.Format(
				"Model=>{0},ViewModel=>{1}",
				Model,
				ViewModel
			);
		}

		#endregion
	}

	/// <summary>
	/// MVMPair生成のヘルパ。
	/// </summary>
	public static class MVMPair
	{
		public static MVMPair<TModel, TViewModel> Create<TModel, TViewModel>(TModel model, TViewModel viewModel)
			where TModel : ModelBase
			where TViewModel : ViewModelBase
		{
			return new MVMPair<TModel, TViewModel>(model, viewModel);
		}

		public static MVMPair<TModel, TViewModel> Create<TModel, TViewModel>(TViewModel viewModel)
			where TModel : ModelBase
			where TViewModel : SingleModelWrapperViewModelBase<TModel>
		{
			return new MVMPair<TModel, TViewModel>(viewModel.Model, viewModel);
		}

	}
}
