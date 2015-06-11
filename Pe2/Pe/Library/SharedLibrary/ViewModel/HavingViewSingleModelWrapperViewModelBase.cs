namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public abstract class HavingViewSingleModelWrapperViewModelBase<TView, TModel> : SingleModelWrapperViewModelBase<TModel>, IHavingView<TView>
		where TView: UIElement
		where TModel: ModelBase
	{
		public HavingViewSingleModelWrapperViewModelBase(TView view, TModel model) 
			:base(model)
		{
			View = view;
		}

		public TView View { get; private set; }
	}
}
