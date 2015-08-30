namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public abstract class HavingViewModelBase<TView> : ViewModelBase, IHavingView<TView>
		where TView: UIElement
	{
		public HavingViewModelBase(TView view)
		{
			View = view;
			InitializeView();
		}

		#region function

		protected virtual void InitializeView()
		{ }

		protected virtual void UninitializeView()
		{ }

		#endregion

		#region IHavingView

		public TView View { get; private set; }
		public bool HasView { get { return HavingViewUtility.GetHasView(this); } }

		#endregion
	}
}
