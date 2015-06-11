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

	public abstract class HavingViewSingleModelWrapperViewModelBase<TView, TModel> : SingleModelWrapperViewModelBase<TModel>, IHavingView<TView>
		where TView: UIElement
		where TModel: ModelBase
	{
		#region variable

		protected EventHandlerDisposer _closedEvent = null; 

		#endregion

		public HavingViewSingleModelWrapperViewModelBase(TView view, TModel model) 
			:base(model)
		{
			View = view;
			if(View != null) {
				InitializeView();
			}
		}

		#region property

		public TView View { get; private set; }
		public bool HasView { get { return View != null; } }

		#endregion

		#region function

		protected virtual void InitializeView()
		{
			Debug.Assert(View != null);

			var vm = this as IHavingView<Window>;
			if(vm != null && HasView) {
				vm.View.Closed += EventUtility.Create(
					(object sender, EventArgs e) => UninitializeView_Impl(), 
					handler => vm.View.Closed -= handler,
					out this._closedEvent
				);
			}
		}

		void UninitializeView_Impl()
		{
			Debug.Assert(HasView);

			this._closedEvent.Dispose();
			UninitializeView();
		}

		protected void UninitializeView()
		{
			Debug.Assert(HasView);
		}

		#endregion
	}
}
