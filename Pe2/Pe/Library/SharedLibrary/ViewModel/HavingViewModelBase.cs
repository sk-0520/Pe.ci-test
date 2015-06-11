namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public abstract class HavingViewModelBase<TView> : ViewModelBase, IHavingView<TView>
		where TView: UIElement
	{
		public HavingViewModelBase(TView view)
		{
			View = view;
			InitializeView();
		}

		#region IHavingView

		public TView View { get; private set; }
		public bool HasView { get { return View != null; } }

		#endregion

		#region function

		protected virtual void InitializeView()
		{ }

		protected virtual void UninitializeView()
		{ }

		#endregion
	}
}
