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
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public abstract class HavingViewSingleModelWrapperViewModelBase<TModel, TView> : SingleModelWrapperViewModelBase<TModel>, IHavingView<TView>
		where TModel: IModel
		where TView: UIElement
	{
		#region variable

		//protected EventHandlerDisposer _closedEvent = null; 

		#endregion

		public HavingViewSingleModelWrapperViewModelBase(TModel model, TView view) 
			:base(model)
		{
			View = view;
			if(HasView) {
				InitializeView();
			}
		}

		#region property

		public TView View { get; private set; }
		public bool HasView { get { return HavingViewUtility.GetHasView(this); } }
		public bool IsClosed { get; private set; }

		#endregion

		#region function

		protected virtual void InitializeView()
		{
			Debug.Assert(View != null);
			IsClosed = false;
			var vm = this as IHavingView<Window>;
			if(vm != null && HasView) {
				vm.View.Closed += View_Closed;
			}
		}

		void UninitializeView_Impl()
		{
			Debug.Assert(HasView);

			UninitializeView();
		}

		protected virtual void UninitializeView()
		{
			Debug.Assert(HasView);
		}

		#endregion

		void View_Closed(object sender, EventArgs e)
		{
			Debug.Assert(HasView);
			IsClosed = true;
			var vm = (IHavingView<Window>)this;
			vm.View.Closed -= View_Closed;
			UninitializeView_Impl();
		}
	}
}
