namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public abstract class HavingViewSingleModelWrapperViewModelBase<TView, TModel> : SingleModelWrapperViewModelBase<TModel>, IHavingView<TView>
		where TView: UIElement
		where TModel: ModelBase
	{
		public HavingViewSingleModelWrapperViewModelBase(TView view, TModel model) 
			:base(model)
		{
			View = view;
			InitializeView();
		}

		#region property

		public TView View { get; private set; }

		#endregion

		#region function

		protected virtual void InitializeView()
		{
			Debug.Assert(View != null);

			var vm = this as IHavingView<Window>;
			if(vm != null) {
				HavingViewUtility.Initialize(vm);
			}
		}

		protected virtual void UninitializeView()
		{ }

		#endregion
	}
}
